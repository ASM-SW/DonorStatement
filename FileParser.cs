// Copyright © 2016-20123  ASM-SW
//asmeyers@outlook.com  https://github.com/asm-sw
using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using System.Data;

namespace DonorStatement
{
    public class FileParser: IDisposable
    {
        readonly LogMessageDelegate m_logger;
        DataTable m_dataTable = new();
        public bool FileHasBeenRead { get; set; }
        private FileParser() { }
        bool m_disposed = false;

        public FileParser(LogMessageDelegate logger)
        {
            m_logger = logger;
            FileHasBeenRead = false;
        }

        public bool ParseInputFile()
        {
            if (string.IsNullOrWhiteSpace(FormMain.Config.InputFileName))
            {
                m_logger("Input file name has not been set");
                MessageBox.Show("Input file name has not been set");
                return false;
            }

            m_logger("Parsing input file: " + FormMain.Config.InputFileName);
            if (FileHasBeenRead)
                m_dataTable = new DataTable();

            FileHasBeenRead = true;
            try
            {
                using TextFieldParser csvReader = new(FormMain.Config.InputFileName);
                csvReader.SetDelimiters([","]);
                csvReader.HasFieldsEnclosedInQuotes = true;
                string[] colFields = csvReader.ReadFields();
                foreach (string item in colFields)
                {
                    DataColumn column = new(item);
                    m_dataTable.Columns.Add(column);
                }
                while (!csvReader.EndOfData)
                {
                    string[] fieldData = csvReader.ReadFields();
                    m_dataTable.Rows.Add(fieldData);
                }
            }
            catch (Exception ex)
            {
                m_logger(ex.ToString());
                m_logger("Error reading input file: " + FormMain.Config.InputFileName);
                return false;
            }

            return true;
        } //end ParseInputFile

        public void GetColumnNames(out List<string> columnNames)
        {
            columnNames = [];
            foreach (DataColumn col in m_dataTable.Columns)
                columnNames.Add(col.ColumnName);
        }

        private void GetColumnContentsUnique(string columnName, out List<string> items)
        {
            items = [];
            DataView view = new(m_dataTable)
            {
                Sort = columnName
            };

            DataTable distinctValues = view.ToTable(true, columnName);

            foreach (DataRow row in distinctValues.Rows)
            {
                string value = row[columnName].ToString();
                if (!string.IsNullOrWhiteSpace(value))
                    items.Add(value);
            }
        }

        public void GetItemList(out List<string> items)
        {
            GetColumnContentsUnique("Name", out items);  // was Item
        }

        public void GetNameList(out List<string> names)
        {
            GetColumnContentsUnique("Customer name", out names);  // was Name
        }

        public void GetDataForName(string name, out DataTable table)
        {
            // need to duplicate the single quote in a name  "O'Donald" -> "O''Donald"
            string filter = string.Format("[Customer name] = '{0}'", name.Replace("'", "''")); 
            DataRow[] rows = m_dataTable.Select(filter, "Date ASC");

            table = m_dataTable.Clone();
            foreach (DataRow row in rows)
            {
                table.Rows.Add(row.ItemArray);
            }
        }

        public void GetColunmNames(out List<string> columNames)
        {
            columNames = [];
            foreach (DataColumn col in m_dataTable.Columns)
                columNames.Add(col.ColumnName);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (m_disposed)
                return;

            if (disposing)
            {
                m_dataTable.Dispose();
            }
            
            m_disposed = true;
        }
    } // end class FileParser
}
