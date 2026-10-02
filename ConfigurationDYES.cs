// Copyright © 2016-2026 ASM-SW
// asm-sw@outlook.com  https://github.com/asm-sw
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace DonorStatement
{
    /// <summary>
    /// ConfigurationDYES class contains all of the configuration information for generating the donor reports.  
    /// It may be saved and read in from file between runs.
    /// </summary>
    [Serializable]
    public class ConfigurationDYES
    {
        public string OutputDirectory { get; set; }             // directory where all of the output files will be saved
        public string PdfTemplateFile { get; set; }             // PDF template file for the donation statement
        public string InputFileName { get; set; }               // Name of input CSV file; Sale report from QuickBooks
        public string OutputFileListFileName { get; set; }      // CSV file that contains the list of statements
        public string DateRange { get; set; }                   // Date range in statement:  Jan 1, 2015 through December 31, 2015
        public string ReturnAddress { get; set; }               // Return address:  4 lines separate by \n
        public List<string> ListDonations { get; set; }         // List of Donations
        public List<string> ListOther { get; set; }             // List of Items not donations
        public List<string> ListIgnore { get; set; }            // List of items that are ignored, not put in donations or other
        public string ConfigFileName { get; set; }              // name of file containing configuration information
        public bool ReportOtherPayments { get; set; }           // Report items that are not included in the donation in the Other Payments table

        public ConfigurationDYES()
        {
            ListDonations = [];
            ListOther = [];
            ListIgnore = [];
            OutputFileListFileName = "1FileList.csv";
            ReportOtherPayments = false;

            string appData = Environment.GetEnvironmentVariable("APPDATA");
            string dataDir = Path.Combine(appData, "DonorStatement");
            if (!Directory.Exists(dataDir))
                Directory.CreateDirectory(dataDir);

            ConfigFileName = Path.Combine(dataDir, "Configuration.xml");
        }

        public bool Serialize(string fileName)
        {
            try
            {
                using TextWriter writer = new StreamWriter(fileName);
                XmlSerializer ser = new(typeof(ConfigurationDYES));
                ser.Serialize(writer, this);
            }
            catch (Exception ex)
            {
                FormMain.MessageBoxError(ex.ToString());
            }
            return true;
        }

        public static bool DeSerialize(string fileName, ref ConfigurationDYES cfg)
        {
            if (!File.Exists(fileName))
                return true;
            try
            {
                XmlSerializer ser = new(typeof(ConfigurationDYES));
                using FileStream fs = new(fileName, FileMode.Open);
                using XmlReader reader = XmlReader.Create(fs);
                cfg = (ConfigurationDYES)ser.Deserialize(reader);
                cfg.ListDonations.Sort();
                cfg.ListDonations.Sort();
                cfg.ListIgnore.Sort();
            }
            catch (Exception ex)
            {
                string msg = string.Format(CultureInfo.CurrentCulture, "Unable to read config file:  {0}\n\n{1}", fileName, ex.ToString());
                MessageBox.Show(msg); // too early to use FormMain.MessageBoxError() since FormMain may not be initialized yet
                return false;
            }
            return true;
        }
    }
}
