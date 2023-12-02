// Copyright © 2016-2023  ASM-SW
//asmeyers@outlook.com  https://github.com/asm-sw
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace DonorStatement
{

    /// <summary>
    /// Class to contain one donor.  This is put into a list and searialized out to a csv file in DoucmentCreator.SaveFilesList().
    /// </summary>
    public class DonorRecord
    {
        private DonorRecord() { }

        public DonorRecord(string name, string fileName, string email, string nameLastFirst)
        {
            Name = name;
            FileName = fileName;
            Email = email;
            NameLastFirst = nameLastFirst;
        }

        public string Name { get; set; }
        public string FileName { get; set; }
        public string Email { get; set; }
        public string NameLastFirst { get; set; }
    }

    /// <summary>
    /// Class to contain one payement.
    /// </summary>
    public class PaymentItem
    {
        public PaymentItem()
        {
            Fields = [];
            IsDonation = false;             // tax deductible based on which items are selected.
        }
        public List<string> Fields { get; set; }
        public bool IsDonation { get; set; }
    }
    
    // this class handles creating, updating and saving the document
    public class DocumentCreator
    {
        private Word.Application m_word = null;
        private object m_oMissing = System.Reflection.Missing.Value;
        //private object oEndOfDoc = @"\endofdoc"; /* \endofdoc is a predefined bookmark */
        private readonly LogMessageDelegate m_logger;
        private readonly List<DonorRecord> m_Files = [];

        // List of colun names used for report.  This must be manually maintained.
        // used to check if DataTable has the correct columns
        /*
        column mapping
        Req Desktop          online           note
        --- ---------------  ---------------- ---------------------------------------
        X   Date             Date   
        x   Item             Name             new product servcie does not have the  xxxx:
        X   Memo             Description    
        X   Name             Customer name    last name, first names
        x   Name City        Billing city   
        X   Name Contact                      first last,  no commas
        x   Name E-Mail      Email address
        x   Name State       Billing state  
        x   Name Street1     Billing address  billing address may include multiple lines separate by ctrl chars
        x   Name Street2        
        x   Name Zip         Billing ZIP code   
        x   Paid Amount      Amount line   new has $ and commas
            Account          Account name     old had # + name, new just name
            Name Phone #        
            Type        

        */
        private static readonly List<string> m_requiredColumnNames =
        [
            "Date",
            "Name",       // name of purchase
            "Billing city",
            "Customer name",   // last, first
            "Email address",
            "Billing state",
            "Billing address",
            "Billing ZIP code",
            "Amount line"          // has $ and commas
        ];

        private static readonly List<string> m_requiredBookmarkNames =
        [
            "Name",
            "StatementDate",
            "ToAddress",
            "YearDateRange",
            "Total",
        ];

        private DocumentCreator() { }

        public DocumentCreator(LogMessageDelegate logger)
        {
            m_logger = logger;
        }

        public void CreateDoc()
        {
            if (string.IsNullOrWhiteSpace(FormMain.Config.OutputDirectory))
            {
                string msg = "Output directory is empty, cannot continue";
                MessageBox.Show(msg);
                m_logger(msg);
                return;
            }
            try
            {
                if (Directory.Exists(FormMain.Config.OutputDirectory))
                {
                    // IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon);
                    DialogResult res = MessageBox.Show("Output directory: " + FormMain.Config.OutputDirectory + " exists, is it OK to delete it and continue?", 
                        "Continue", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (res == DialogResult.Cancel)
                    {
                        m_logger("Output directory exists: " + FormMain.Config.OutputDirectory);
                        m_logger("User selected to cancel");
                        return;
                    }
                    Directory.Delete(FormMain.Config.OutputDirectory, true);
                    System.Threading.Thread.Sleep(1000);
                    m_Files.Clear();
                }
                Directory.CreateDirectory(FormMain.Config.OutputDirectory);
            }
            catch (Exception ex)
            {
                m_logger("Error: " + ex);
                return;
            }

        }

        public void CreateDocsDone()
        {
            if (m_word != null)
            {
                m_word.Quit();
                m_word = null;
            }
        }

        public bool CheckForColumns(ref List<string> columnNames)
        {
            StringBuilder msg = new("ERROR - CSV input file is missing the following columns: ");
            int cnt = 0;
            foreach (string name in m_requiredColumnNames)
            {
                if (!columnNames.Contains(name))
                {
                    if (cnt++ > 0)
                        msg.Append(", ");
                    msg.AppendFormat(" \"{0}\"", name);
                }
            }
            if (cnt > 0)
            {
                MessageBox.Show(msg.ToString());
                return false;
            }
            return true;
        }

        public bool CheckForBookmarksAndTables()
        {
            StringBuilder msg = new("WARNING-  Word template file is missing the following bookmarks: ");
            int cnt = 0;
            Word.Document oDoc;

            m_word ??= new Word.Application();  // only creates new if m_word is null

            object oTemplate = FormMain.Config.WordTemplateFileName;
            oDoc = m_word.Documents.Add(ref oTemplate, ref m_oMissing, ref m_oMissing, ref m_oMissing);
            bool result = true;
            foreach (string bookMarkName in m_requiredBookmarkNames)
            {
                Word.Bookmark bookMark = null;
                if (!FindBookMark(bookMarkName, oDoc, ref bookMark))
                {
                    if (cnt++ > 0)
                        msg.Append(", ");
                    msg.AppendFormat(" \"{0}\"", bookMarkName);
                }
            }
            if (cnt > 0)
            {
                MessageBox.Show(msg.ToString());
                result = false;
            }

            // now check for tables.  The table Title is the Title text box on the Alt Text tab of the Table Properties
            msg = new StringBuilder("WARNING-  Word template file is missing the following tables: ");
            cnt = 0;
            if (!FindTable("TablePayments", oDoc, out Word.Table table))
            {
                cnt++;
                msg.AppendFormat(" \"{0}\"", "TablePayments");
            }
            if (FormMain.Config.ReportOtherPayments)
                if (!FindTable("TableOtherPayments", oDoc, out table))
                {
                    if (cnt++ > 0)
                        msg.Append(", ");
                    msg.AppendFormat(" \"{0}\"", "TableOtherPaymetns");
                }
            if (cnt > 0)
            {
                MessageBox.Show(msg.ToString());
                result = false;
            }

            oDoc.Close(Word.WdSaveOptions.wdDoNotSaveChanges, m_oMissing, m_oMissing);
            return result;
        }

        public void CreateDoc(DataTable table)
        {
            m_word ??= new Word.Application(); // only creates new if m_word is null
            if (table.Rows.Count == 0)
                return;  // should never happen

            // Get name, in case one is empty, make them both equal
            // if both empty give up
            string nameLastFirst = table.Rows[0]["Customer name"].ToString();
            if (string.IsNullOrWhiteSpace(nameLastFirst))
                return;  // should never happen
            RemoveDeletedFromString(ref nameLastFirst);

            string customerName = nameLastFirst;

            //split "last, first" into customer name
            string first = "";
            string last = "";
            int commaPos = nameLastFirst.IndexOf(',');
            if (commaPos > 0)
            {
                first = nameLastFirst.Substring(commaPos + 1);
                first = first.Trim();
                last = nameLastFirst.Substring(0, commaPos);
                last = last.Trim();
                last = last.Trim('_');
                customerName = string.Format("{0} {1}", first, last);
            }

            // If the first row does not have an item, skip because it is probably a time stamp
            if (string.IsNullOrWhiteSpace(table.Rows[0]["Name"].ToString()))
            {
                m_logger("Skipping user: " + customerName + " because Item column is empty");
                return;
            }

            //create a new document.
            Word.Document oDoc;
            try
            {
                m_word.Visible = true;
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                // user probably closed Word, create a new instance
                m_word = new Word.Application
                {
                    Visible = true
                };
            }
            object oTemplate = FormMain.Config.WordTemplateFileName;
            oDoc = m_word.Documents.Add(ref oTemplate, ref m_oMissing,
                ref m_oMissing, ref m_oMissing);

            // create replacement values for the bookmarks
            List<KeyValuePair<string, string>> bookMarks = [];
            bookMarks.Add(new KeyValuePair<string, string>("Name", customerName));
            string date = DateTime.Now.ToString("M") + ", " + DateTime.Now.ToString("yyyy");
            bookMarks.Add(new KeyValuePair<string, string>("StatementDate", date));

            StringBuilder builderToAddress = new();
            builderToAddress.AppendLine(customerName);
            builderToAddress.AppendLine(table.Rows[0]["Billing Address"].ToString());
            bool bSkippedLine = false;
            builderToAddress.AppendFormat("{0}, {1}  {2}", table.Rows[0]["Billing city"].ToString(), table.Rows[0]["Billing state"].ToString(), table.Rows[0]["Billing ZIP code"].ToString());
            if (bSkippedLine)
                builderToAddress.AppendLine();

            bookMarks.Add(new KeyValuePair<string, string>("ToAddress", builderToAddress.ToString()));
            bookMarks.Add(new KeyValuePair<string, string>("YearDateRange", FormMain.Config.DateRange));

            // used to convert numbers to strings
            const string formatNumberSmall = ",0.00";  // 1.12
            const string formatNumberLarge = "0,0.00"; // 123,456.78

            //table of payments
            decimal totalDoations = 0;
            decimal total = 0;
            List<PaymentItem> payments = [];
            foreach (DataRow row in table.Rows)
            {
                PaymentItem payment = new();
                string item = row["Name"].ToString();
                if (item == "--")
                    continue;
                if (string.IsNullOrWhiteSpace(item))
                    continue;

                RemoveDeletedFromString(ref item);
                // Binary search returns  0 based index of find, negative number if not found
                if (FormMain.Config.ItemListSelected.BinarySearch(item) >= 0)
                    payment.IsDonation = true;

                payment.Fields.Add(row["Date"].ToString());
                payment.Fields.Add(item);
                payment.Fields.Add(row["Description"].ToString());
                string paid = row["Amount line"].ToString();
                if (decimal.TryParse(paid, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal thisAmount))
                {
                    total += thisAmount;
                    if (payment.IsDonation)
                        totalDoations += thisAmount;
                }
                payment.Fields.Add(thisAmount.ToString(thisAmount < 10 ? formatNumberSmall : formatNumberLarge, System.Globalization.CultureInfo.InvariantCulture));

                //check to see if the item should be ignored, if so drop it
                if (FormMain.Config.ItemListIgnore.BinarySearch(item) >= 0)
                    continue;

                payments.Add(payment);
            }
            if (total == 0)
            {
                m_logger("Skipping: " + customerName + ", No items were found, or item amount was zero.");
                oDoc.Close(Word.WdSaveOptions.wdDoNotSaveChanges, m_oMissing, m_oMissing);
                return;
            }
            string email = table.Rows[0]["Email address"].ToString();
            if (email == "--")
                email = string.Empty;

            // check for second email address and include it in semicolon separated list
            int idxEmail2 = table.Columns.IndexOf("Email2");
            int idx2 = table.Columns.IndexOf("entity_column_customer_udcf_9");  // weired column name in report
            string email2 = string.Empty;
            if (idx2 >= 0)
                email2 =  table.Rows[0][idx2].ToString();
            else if (idxEmail2 >= 0)
                email2 = table.Rows[0][idxEmail2].ToString();
            if (!string.IsNullOrEmpty(email2) && email2 != "--")
                email += "; " + email2;

            // create filename replacing certain characters
            string fileName = nameLastFirst;
            fileName = fileName.Replace(',', '.');
            fileName = fileName.Replace('&', '-');
            fileName = fileName.Replace('/', '-');
            fileName = fileName.Replace('\'', '_');
            fileName = fileName.Replace(" ", string.Empty);
            fileName += ".pdf";
            fileName = Path.Combine(FormMain.Config.OutputDirectory, fileName);

            string amount = string.Format("{0:C2}", totalDoations);
            bookMarks.Add(new KeyValuePair<string, string>("Total", amount));

            CreatePdfFile(bookMarks, payments, oDoc, fileName);

            DonorRecord donorRecord = new(customerName, fileName, email, nameLastFirst);
            m_Files.Add(donorRecord);
            m_word.Visible = false;
        } // end CreateDoc

        private void RemoveDeletedFromString(ref string item)
        {
            const string strDeleted = " (deleted)";
            if (item.EndsWith(strDeleted))
                item = item.Substring(0, item.LastIndexOf(strDeleted));
            return;
        }

        /// <summary>
        /// Creates the PDF document for a given record
        /// </summary>
        /// <param name="bookMarks">Each item in this list is a bookmark (key) in the word document that must be updated with the value</param>
        /// <param name="payments">list of payments</param>
        /// <param name="oDoc">The document</param>
        /// <param name="fileName">name of PDF file</param>
        /// <returns>false on any error</returns>
        private bool CreatePdfFile(List<KeyValuePair<string, string>> bookMarks, List<PaymentItem> payments, Word.Document oDoc, string fileName)
        {
            m_logger("Creating: " + fileName);
            bool result = false;
            foreach (KeyValuePair<string, string> item in bookMarks)
            {
                if (!UpdateBookmark(item.Key, oDoc, item.Value))
                    result = false;
            }
            if (!UpdateTable(oDoc, "TablePayments", payments, true))
            {
                result = false;
            }

            if (FormMain.Config.ReportOtherPayments)
            {
                if (!UpdateTable(oDoc, "TableOtherPayments", payments, false))
                {
                    result = false;
                }
            }
            SavePdf(oDoc, fileName);
            oDoc.Close(Word.WdSaveOptions.wdDoNotSaveChanges, m_oMissing, m_oMissing);

            return result;
        }

        /// <summary>
        /// Update a table in the word document
        /// </summary>
        /// <param name="oDoc"></param>
        /// <param name="tableName">This is the Alt text for the table in word.  Used to find the table to udpate</param>
        /// <param name="rows">list of payments</param>
        /// <param name="isDonation">Only adds row if the isDonation property of the row matches this parameter </param>
        /// <returns></returns>
        private bool UpdateTable(Word.Document oDoc, string tableName, List<PaymentItem> rows, bool isDonation)
        {
            if (!FindTable(tableName, oDoc, out Word.Table table))
            {
                m_logger("Unable to find table to update: " + tableName);
                return false;
            }
            bool bAddedRow = false;
            foreach (PaymentItem item in rows)
            {
                if (item.IsDonation == isDonation)
                {
                    AppendRowToTable(item.Fields, ref table);
                    bAddedRow = true;
                }
            }

            if (!bAddedRow)
            {
                // Add a comment to the table about no payments
                PaymentItem item = new()
                {
                    IsDonation = isDonation
                };
                item.Fields.Add(string.Empty);
                if (isDonation)
                    item.Fields.Add("No Donations");
                else
                    item.Fields.Add("No Other Payments");
                AppendRowToTable(item.Fields, ref table);
            }
                

            return true;
        }

        /// <summary>
        /// Does the actual work of adding the row into the table in the document
        /// </summary>
        /// <param name="values">list of values used to create the row in the table</param>
        /// <param name="table">the table to modify</param>
        private void AppendRowToTable(List<string> values, ref Word.Table table)
        {
            Word.Row row = table.Rows.Add(System.Reflection.Missing.Value);
            int i = 0;
            foreach (string item in values)
            {
                row.Cells[++i].Range.Text = item;
            }
        }

        /// <summary>
        /// UPdates a bookmark in the word document
        /// </summary>
        /// <param name="bookMarkName">name of the bookmark to update</param>
        /// <param name="oDoc">the word document</param>
        /// <param name="value">the value to set the bookmark to</param>
        /// <returns></returns>
        private bool UpdateBookmark(string bookMarkName, Word.Document oDoc, string value)
        {
            Word.Bookmark bookMark = null;
            if (FindBookMark(bookMarkName, oDoc, ref bookMark))
            {
                bookMark.Range.Text = value;
                return true;
            }
            m_logger("Unable to find bookmark to update: " + bookMarkName);
            return false;
        }

        private bool FindBookMark(string bookMarkName, Word.Document oDoc, ref Word.Bookmark bookMark)
        {
            bookMark = null;

            foreach (Word.Bookmark item in oDoc.Bookmarks)
            {
                if (item.Name == bookMarkName)
                {
                    bookMark = item;
                    return true;
                }
            }
            return false;
        }

        private bool FindTable(string tableAltText, Word.Document oDoc, out Word.Table table)
        {
            table = null;
            foreach (Word.Table item in oDoc.Tables)
            {
                if (item.Title == tableAltText)
                {
                    table = item;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Save the word doc as a PDF file
        /// </summary>
        /// <param name="oDoc">the document</param>
        /// <param name="fileName">full path/name of PDF file</param>
        private void SavePdf(Word.Document oDoc, string fileName)
        {
            try
            {
                oDoc.SaveAs(fileName, Word.WdSaveFormat.wdFormatPDF);
            }
            catch (Exception ex)
            {
                m_logger("Error saving file: " + fileName);
                m_logger("Error: " + ex.ToString());
            }
        }

        /// <summary>
        /// writes out m_files to the 1FileList.csv
        /// </summary>
        public void SaveFileList()
        {
            string fileName = Path.Combine(FormMain.Config.OutputDirectory, "1FileList.csv");
            try
            {
                using StreamWriter file = new(fileName);
                file.WriteLine("NameLastFirst, Name,FileName, Email");
                foreach (var item in m_Files)
                    file.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\"", item.NameLastFirst, item.Name, item.FileName, item.Email));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving file: " + fileName + "\n" + ex.Message);
            }
        }

        public void Close()
        {
            try
            {
                m_word?.Quit();
            }
            catch (Exception)
            {
                //MessageBox.Show(ex.ToString());
            }

        } // close

    }
}
