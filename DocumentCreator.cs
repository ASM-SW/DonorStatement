// Copyright © 2016-2025 ASM-SW
//asm-sw@outlook.com  https://github.com/asm-sw
using ASM_SW.PdfCreator;
using MessageBoxCenteredDll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;
using DataTable = System.Data.DataTable;

namespace DonorStatement
{
    /// <summary>
    /// Class to contain one donor.  This is put into a list and serialized out to a csv file in DocumentCreatorQPdf.SaveFilesList().
    /// </summary>
    internal class DonorRecordQPdf
    {
        public DonorRecordQPdf(string name, string fileName, string email, string nameLastFirst)
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
    /// Class to contain a table name and its index within the template.
    /// </summary>
    internal class TableInfo
    {
        internal TableInfo(string tableName, int index)
        {
            TableName = tableName;
            Index = index;

        }

        public string TableName { get; set; }
        public int Index { get; set; }

        public decimal Total { get; set; } = 0;
    }

    // this class handles creating, updating and saving the document
    internal class DocumentCreator
    {
        private readonly LogMessageDelegate m_logger;
        private readonly List<DonorRecordQPdf> m_Files = [];

        private LetterTemplate m_template;

        // These are the fields that are required to be in the template. 
        // They will be replaced with the values from the data table.  
        private static readonly List<string> m_requiredPlaceHolders =
        [
            "«Name»",
            "«StatementDate»",
            "«ToAddress»",
            "«YearDateRange»",
            "«Total»",
        ];

        private TableInfo m_DonationTable = new TableInfo("DonationTable", -1);
        private TableInfo m_OtherPaymentsTable = new TableInfo("OtherPaymentsTable", -1);
        int m_colDate = -1;
        int m_colItem = -1;
        int m_colDescription = -1;
        int m_colAmount = -1;

        public DocumentCreator(LogMessageDelegate logger)
        {
            m_logger = logger;

        }

        // Do all of the checks to be sure that individual documents can be created.  
        // If any of the checks fail, return false and do not continue.
        public bool DocumentCreatorInit()
        {
            if (!InitTemplate(out string templateText))
                return false;
            if (!CheckTemplate(templateText))
                return false;
            if (!GetTemplateInfo())
                return false;

            if (string.IsNullOrWhiteSpace(FormMain.Config.OutputDirectory))
            {
                string msg = "Output directory is empty, cannot continue";
                FormMain.MessageBoxError(msg);
                m_logger(msg);
                return false;
            }
            if (Directory.Exists(FormMain.Config.OutputDirectory))
            {
                MessageBoxCentered.ButtonTyp res = FormMain.MessageBoxEx("Information",
                    string.Concat("Output directory exists: ", FormMain.Config.OutputDirectory), MessageBoxCentered.BoxType.OkCancel);
                if (res == MessageBoxCentered.ButtonTyp.Cancel)
                {
                    m_logger("Output directory exists: " + FormMain.Config.OutputDirectory);
                    m_logger("User selected to cancel");
                    return false;
                }
                try
                {
                    Directory.Delete(FormMain.Config.OutputDirectory, true);
                    System.Threading.Thread.Sleep(1000);
                    m_Files.Clear();
                    Directory.CreateDirectory(FormMain.Config.OutputDirectory);
                }
                catch (Exception ex)
                {
                    m_logger("Error: " + ex);
                    return false;
                }
            }
            return true;
        }

        private bool InitTemplate(out string templateText)
        {
            templateText = string.Empty;
            if (string.IsNullOrWhiteSpace(FormMain.Config.PdfTemplateFile))
            {
                string msg = "PDF template file is empty, cannot continue";
                FormMain.MessageBoxError(msg);
                m_logger(msg);
                return false;
            }
            if (!File.Exists(FormMain.Config.PdfTemplateFile))
            {
                string msg = "PDF template file does not exist: " + FormMain.Config.PdfTemplateFile;
                FormMain.MessageBoxError(msg);
                m_logger(msg);
                return false;
            }
            try
            {
                templateText = File.ReadAllText(FormMain.Config.PdfTemplateFile);
                m_template = JsonSerializer.Deserialize<LetterTemplate>(templateText);
                if (m_template == null)
                {
                    string msg = "Error reading PDF template file: " + FormMain.Config.PdfTemplateFile + "\nTemplate is null";
                    FormMain.MessageBoxError(msg);
                    m_logger(msg);
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                string msg = "Error reading PDF template file: " + FormMain.Config.PdfTemplateFile + "\n" + ex.Message;
                FormMain.MessageBoxError(msg);
                m_logger(msg);
                return false;
            }
            return true;
        }

        bool GetTemplateInfo()
        {
            // check for the tables that are required to be in the template.
            for (int i = 0; i < m_template.Blocks.Count; i++)
            {
                if (m_template.Blocks[i].Id == m_DonationTable.TableName)
                    m_DonationTable.Index = i;
                else if (m_template.Blocks[i].Id == m_OtherPaymentsTable.TableName)
                    m_OtherPaymentsTable.Index = i;
            }
            if(m_DonationTable.Index < 0)
                m_logger("DonationTable not found in template");
            if(m_OtherPaymentsTable.Index < 0)
                m_logger("OtherPaymentsTable not found in template");
            bool isOk = m_DonationTable.Index >= 0 && m_OtherPaymentsTable.Index >= 0;
            StringBuilder msg = new();
            if (m_DonationTable.Index < 0)
            {
                isOk = false;
                msg.Append(CultureInfo.CurrentCulture, $"Table Missing: {m_DonationTable.TableName} ");
            }
            if (m_OtherPaymentsTable.Index < 0)
            {
                isOk = false;
                msg.Append(CultureInfo.CurrentCulture, $"Table Missing: {m_OtherPaymentsTable.TableName} ");
            }
            if (isOk)
            {
                // find column numbers
                m_colDate = m_template.Blocks[m_DonationTable.Index].Columns.FindIndex(c => c.Header == ColumnMap.Lookup("Date"));
                m_colItem = m_template.Blocks[m_DonationTable.Index].Columns.FindIndex(c => c.Header == ColumnMap.Lookup("Product/Service"));
                m_colDescription = m_template.Blocks[m_DonationTable.Index].Columns.FindIndex(c => c.Header == "Memo/Description");
                m_colAmount = m_template.Blocks[m_DonationTable.Index].Columns.FindIndex(c => c.Header == ColumnMap.Lookup("Amount"));
                if (m_colDate < 0 || m_colItem < 0 || m_colDescription < 0 || m_colAmount < 0)
                {
                    isOk = false;
                    msg.Append("DonationTable missing columns, either in lookup or: ");
                    if (m_colDate < 0)
                        msg.Append(ColumnMap.Lookup("Date") + " ");
                    if (m_colItem < 0)
                        msg.Append(ColumnMap.Lookup("Product/Service") + " ");
                    if (m_colDescription < 0)
                        msg.Append("Memo/Description" + " ");
                    if (m_colAmount < 0)
                        msg.Append(ColumnMap.Lookup("Amount") + " ");
                }
            }
            if (!isOk)
            {
                m_logger(msg.ToString());
                FormMain.MessageBoxError(msg.ToString());
            }
            return isOk;
        }


        // check for all of the place holders and tables that are required to be in the template. 
        bool CheckTemplate(string templateText)
        {
            StringBuilder msg = new("Template file is missing the following fields: ");
            bool isOk = true;
            foreach (string field in m_requiredPlaceHolders)
            {
                if (!templateText.Contains($"{field}", StringComparison.CurrentCulture))
                {
                    msg.Append(field + " ");
                    isOk = false;
                }
            }
            if (!isOk)
                FormMain.MessageBox(msg.ToString());
            isOk &= GetTemplateInfo();
            return isOk;
        }

        public void CreateDocsDone()
        {
            // No action needed for QuestPDF
        }

        public void CreateDoc(DataTable table)
        {
            if (table.Rows.Count == 0)
                return;  // should never happen

            // Get name, in case one is empty, make them both equal
            // if both empty give up
            string nameLastFirst = table.Rows[0][ColumnMap.Lookup("Customer")].ToString();
            nameLastFirst = nameLastFirst.TrimEnd('_');
            if (string.IsNullOrWhiteSpace(nameLastFirst))
                return;  // should never happen
            RemoveDeletedFromString(ref nameLastFirst);

            string customerName = nameLastFirst;

            //split "last, first" into customer name
            string first = "";
            string last = "";
            int commaPos = nameLastFirst.IndexOf(',', StringComparison.CurrentCulture);
            if (commaPos > 0)
            {
                first = nameLastFirst[(commaPos + 1)..];
                first = first.Trim();
                last = nameLastFirst[..commaPos];
                last = last.Trim();
                last = last.TrimEnd('_');
                customerName = string.Format(CultureInfo.CurrentCulture, "{0} {1}", first, last);
            }

            //create a new document.
            // create replacement values for the bookmarks
            Dictionary<string, string> fields = [];
            fields.Add("Name", customerName);
            string date = DateTime.Now.ToString("M", CultureInfo.CurrentCulture) + ", " + DateTime.Now.ToString("yyyy", CultureInfo.CurrentCulture);
            fields.Add("StatementDate", date);

            // use the last row for the address, in case there was a change
            int ndxLast = table.Rows.Count - 1;
            StringBuilder builderToAddress = new();
            builderToAddress.AppendLine(customerName);
            builderToAddress.AppendLine(table.Rows[ndxLast][ColumnMap.Lookup("Billing street")].ToString());
            StringBuilder stringBuilder = builderToAddress.AppendFormat(CultureInfo.CurrentCulture, "{0}, {1}  {2}", table.Rows[ndxLast][ColumnMap.Lookup("Billing city")].ToString(), table.Rows[ndxLast][ColumnMap.Lookup("Billing state")].ToString(), table.Rows[ndxLast][ColumnMap.Lookup("Billing zip code")].ToString());

            fields.Add("ToAddress", builderToAddress.ToString());
            fields.Add("YearDateRange", FormMain.Config.DateRange);

            // used to convert numbers to strings
            const string formatNumberSmall = ",0.00";  // 1.12
            const string formatNumberLarge = "0,0.00"; // 123,456.78

            // handle the table of donations and other payments
            foreach (DataRow row in table.Rows)
            {
                string item = row[m_colItem].ToString();
                if (item == "--")
                    continue;
                if (string.IsNullOrWhiteSpace(item))
                    continue;

                // Binary search returns  0 based index of find, negative number if not found
                //check to see if the item should be ignored, if so drop it
                if (FormMain.Config.ItemListIgnore.BinarySearch(item) >= 0)
                    continue;
                bool isDonation = false;
                if (FormMain.Config.ItemListSelected.BinarySearch(item) >= 0)
                    isDonation = true;
                RemoveDeletedFromString(ref item);

                string itemDate = row[m_colDate].ToString();
                string description = row[m_colDescription].ToString();
                if (description == "--")
                    description = string.Empty;
                string paid = row[m_colAmount].ToString();
                if (decimal.TryParse(paid, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal thisAmount))
                {
                    if (isDonation)
                        m_DonationTable.Total += thisAmount;
                    else
                        m_OtherPaymentsTable.Total += thisAmount;
                }
                string amountString = thisAmount.ToString(thisAmount < 10 ? formatNumberSmall : formatNumberLarge, CultureInfo.InvariantCulture);

                TableRow payment = new();
                payment.Cells = [itemDate, item, description, amountString];
                if (isDonation)
                    m_template.Blocks[m_DonationTable.Index].Rows.Add(payment);
                else
                    m_template.Blocks[m_OtherPaymentsTable.Index].Rows.Add(payment);
            }
            if (m_DonationTable.Total == 0 && m_OtherPaymentsTable.Total == 0)
            {
                m_logger("Skipping: " + customerName + ", No items were found, or item amount was zero.");
                return;
            }

            // add summary row to the end of each table
            foreach (TableInfo tableInfo in new TableInfo[] { m_DonationTable, m_OtherPaymentsTable })
            {
                string amountString = tableInfo.Total.ToString(tableInfo.Total < 10 ? formatNumberSmall : formatNumberLarge, CultureInfo.InvariantCulture);
                TableRow summary = new();
                if (tableInfo.Total == 0)
                    summary.Cells = [string.Empty, "None", string.Empty, string.Empty];
                else
                    summary.Cells = [string.Empty, string.Empty, "Total Donations", amountString];
                m_template.Blocks[tableInfo.Index].Rows.Add(summary);
            }


            string email = table.Rows[0][ColumnMap.Lookup("Email")].ToString();
            if (email == "--")
                email = string.Empty;

            // check for second Email and include it in semicolon separated list
            int idxEmail2 = table.Columns.IndexOf("Email2");
            string email2 = string.Empty;
            if (idxEmail2 >= 0)
                email2 = table.Rows[0][idxEmail2].ToString();
            if (!string.IsNullOrEmpty(email2) && email2 != "--")
                email += "; " + email2;
            email = email.Replace(",", ";", StringComparison.CurrentCulture);  // newer way is to put multiple emails into the email field with a comma separate. Convert to ; which email programs like

            // create filename replacing certain characters
            string fileName = nameLastFirst;
            fileName = fileName.Replace(',', '.');
            fileName = fileName.Replace('&', '-');
            fileName = fileName.Replace('/', '-');
            fileName = fileName.Replace('\'', '_');
            fileName = fileName.Replace(" ", string.Empty, StringComparison.CurrentCulture);
            fileName += ".pdf";
            fileName = Path.Combine(FormMain.Config.OutputDirectory, fileName);

            // Generate letter modifies filename if there is a conflict
            string fullPath = LetterProcessor.GenerateLetter(m_template, fields, FormMain.Config.OutputDirectory, fileName);
            fileName = Path.GetFileName(fullPath);
            DonorRecordQPdf donorRecord = new(customerName, fileName, email, nameLastFirst);
            m_Files.Add(donorRecord);
        } // end CreateDoc

        private static void RemoveDeletedFromString(ref string item)
        {
            const string strDeleted = " (deleted)";
            if (item.EndsWith(strDeleted, StringComparison.CurrentCulture))
                item = item[..item.LastIndexOf(strDeleted, StringComparison.CurrentCulture)];
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
                    file.WriteLine(string.Format(CultureInfo.CurrentCulture, "\"{0}\",\"{1}\",\"{2}\",\"{3}\"", item.NameLastFirst, item.Name, item.FileName, item.Email));
            }
            catch (Exception ex)
            {
                FormMain.MessageBoxError("Error saving file: " + fileName + "\n" + ex.Message);
            }
        }

        public void Close()
        {
            // No action needed for QuestPDF
        } // close

    }
}