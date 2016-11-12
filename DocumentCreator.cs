// Copyright © 2016  ASM-SW
//asmeyers@outlook.com  https://github.com/asm-sw
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace DonorStatement
{
    public class DonorRecord
    {
        private DonorRecord() { }

        public DonorRecord(string name, string fileName, string email)
        {
            Name = name;
            FileName = fileName;
            Email = email;
        }

        public string Name { get; set; }
        public string FileName { get; set; }
        public string Email { get; set; }
    }
    // this class handles creating, updating and saving the document
    public class DocumentCreator
    {
        private Word.Application m_word = null;
        private object m_oMissing = System.Reflection.Missing.Value;
        //private object oEndOfDoc = @"\endofdoc"; /* \endofdoc is a predefined bookmark */
        private LogMessageDelegate m_logger;
        private List<DonorRecord> m_Files = new List<DonorRecord>();

        // List of colun names used for report.  This must be manually maintained.
        // used to check if DataTable has the correct columns
        private List<string> m_requiredColumnNames = new List<string>
        {
            "Date",
            "Memo",
            "Name Contact",
            "Name",
            "Item",
            "Name E-Mail",
            "Name Street1",
            "Name Street2",
            "Name City",
            "Name State",
            "Name Zip",
            "Paid Amount"
        };

        private List<string> m_requiredBookmarkNames = new List<string>
        {
            "Name",
            "StatementDate",
            "ToAddress",
            "YearDateRange",
            "Total",
            "TablePayments"
        };

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
            StringBuilder msg = new StringBuilder("ERROR - CSV input file is missing the following columns: ");
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

        public bool CheckForBookmarks()
        {
            StringBuilder msg = new StringBuilder("WARNING-  Word template file is missing the following bookmarks: ");
            int cnt = 0;
            Word.Document oDoc;

            if (m_word == null)
                m_word = new Word.Application();

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
            oDoc.Close(Word.WdSaveOptions.wdDoNotSaveChanges, m_oMissing, m_oMissing);
            if (cnt > 0)
            {
                MessageBox.Show(msg.ToString());
                result = false;
            }
            return result;
        }

        public void CreateDoc(DataTable table)
        {
            if(m_word == null)
                m_word = new Word.Application();

            if (table.Rows.Count == 0)
                return;  // should never happen

            // Get name
            string customerName = table.Rows[0]["Name Contact"].ToString();
            if(string.IsNullOrWhiteSpace(customerName))
                customerName = table.Rows[0]["Name"].ToString();
            if (string.IsNullOrWhiteSpace(customerName))
                return;  // should never happen

            // If the first row does not have an item, skip because it is probably a time stamp
            if (string.IsNullOrWhiteSpace(table.Rows[0]["Item"].ToString()))
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
                m_word = new Word.Application();
                m_word.Visible = true;
            }
            object oTemplate = FormMain.Config.WordTemplateFileName;
            oDoc = m_word.Documents.Add(ref oTemplate, ref m_oMissing,
                ref m_oMissing, ref m_oMissing);

            List<KeyValuePair<string, string>> bookMarks = new List<KeyValuePair<string, string>>();
            bookMarks.Add(new KeyValuePair<string, string>("Name", customerName));
            string date = DateTime.Now.ToString("M") + ", " + DateTime.Now.ToString("yyyy");
            bookMarks.Add(new KeyValuePair<string, string>("StatementDate", date));
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(customerName);
            builder.AppendLine(table.Rows[0]["Name Street1"].ToString());
            string street2 = table.Rows[0]["Name Street2"].ToString();
            bool bSkippedLine = false;
            if (string.IsNullOrWhiteSpace(street2))
                bSkippedLine = true;

            builder.AppendFormat("{0}, {1}  {2}", table.Rows[0]["Name City"].ToString(), table.Rows[0]["Name State"].ToString(), table.Rows[0]["Name Zip"].ToString());
            if (bSkippedLine)
                builder.AppendLine();
            bookMarks.Add(new KeyValuePair<string, string>("ToAddress", builder.ToString()));
            bookMarks.Add(new KeyValuePair<string, string>("YearDateRange", FormMain.Config.DateRange));


            //table of payments
            float total = 0;
            List<List<string>> donations = new List<List<string>>();
            foreach (DataRow row in table.Rows)
            {
                List<string> aDonation = new List<string>();
                string item = row["Item"].ToString();
                if (FormMain.Config.ItemListSelected.BinarySearch(item) < 0)
                    continue;

                aDonation.Add(row["Date"].ToString());
                aDonation.Add(item);                
                aDonation.Add(row["Memo"].ToString());
                string paid = row["Paid Amount"].ToString();
                aDonation.Add(paid);

                donations.Add(aDonation);

                float thisAmount = 0;
                if (float.TryParse(paid, out thisAmount))
                    total += thisAmount;
            }

            string fileName = customerName = table.Rows[0]["Name"].ToString();
            string email = table.Rows[0]["Name E-Mail"].ToString();
            fileName = fileName.Replace(',', '.');
            fileName = fileName.Replace('&', '-');
            fileName = fileName.Replace('\'', '_');
            fileName = fileName.Replace(" ", string.Empty);
            if (total == 0)
            {
                m_logger("Skipping: " + customerName + ", No items were found, or the item amount was $0.");
                oDoc.Close(Word.WdSaveOptions.wdDoNotSaveChanges, m_oMissing, m_oMissing);
                return;
            }
            fileName += ".pdf";

            string amount = string.Format("{0:C2}", total);
            bookMarks.Add(new KeyValuePair<string, string>("Total", amount));

            CreateDocument(bookMarks, donations, "TablePayments", fileName, oDoc, customerName, email);
            m_word.Visible = false;
        }


        /// <summary>
        /// Creates the PDF document for a given record
        /// </summary>
        /// <param name="bookMarks">Each item in this list is a bookmark (key) in the word document that must be updated with the value</param>
        /// <param name="donations">Each item in the list represents one row in the table with name tableName</param>
        /// <param name="name">name of file to create</param>
        /// <param name="tableName">boorkMark for table to dupate</param>
        /// <param name="email">email representing this document</param>
        /// <param name="oDoc">The document</param>
        /// <returns>false on any error</returns>
        private bool CreateDocument(List<KeyValuePair<string, string>> bookMarks, List<List<string>> donations, string tableName, string fileName,
            Word.Document oDoc, string customerName, string email)
        {
            string fullFileName = Path.Combine(FormMain.Config.OutputDirectory, fileName);
            m_logger("Creating: " + fullFileName);
            bool result = false;
            foreach (KeyValuePair<string, string> item in bookMarks)
            {
                if (!UpdateBookmark(item.Key, oDoc, item.Value))
                    result = false;
            }
            if (!UpdateTable(oDoc, tableName, donations))
            {
                result = false;
            }
            m_Files.Add(new DonorRecord(customerName, fullFileName, email));

            SavePdf(oDoc, fullFileName);
            oDoc.Close(Word.WdSaveOptions.wdDoNotSaveChanges, m_oMissing, m_oMissing);

            return result;
        }

        private bool UpdateTable(Word.Document oDoc, string tableName, List<List<string>> rows)
        {
            Word.Table table;
            if (!FindTable(tableName, oDoc, out table))
            {
                m_logger("Unable to find table to update: " + tableName);
                return false;
            }
            foreach (List<string> rowItem in rows)
            {
                AppendRowToTable(rowItem, ref table);
            }

            return true;
        }

        private void AppendRowToTable(List<string> values, ref Word.Table table)
        {
            Word.Row row = table.Rows.Add(System.Reflection.Missing.Value);
            int i = 0;
            foreach (string item in values)
            {
                row.Cells[++i].Range.Text = item;
            }
        }

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

        public void SaveFileList()
        {
            string fileName = Path.Combine(FormMain.Config.OutputDirectory, "1FileList.csv");
            try
            {
                using (StreamWriter file = new StreamWriter(fileName))
                {
                    file.WriteLine("CustomerName,FileName, Email");
                    foreach (var item in m_Files)
                        file.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\"", item.Name, item.FileName, item.Email));
                }
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
                if (m_word != null)
                    m_word.Quit();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }

        } // close

    }
}
