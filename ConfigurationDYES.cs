// Copyright © 2016-2018  ASM-SW
// asmeyers@outlook.com  https://github.com/asm-sw
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        public string WordTemplateFileName { get; set; }            // word template file for the donation statement
        public string InputFileName { get; set; }               // Name of input CSV file; Sale report from QuickBooks
        public string OutputFileListFileName { get; set; }      // CSV file that contains the list of statements
        public string DateRange { get; set; }                   // Date range in statement:  Jan 1, 2015 through December 31, 2015
        public string ReturnAddress { get; set; }               // Return address:  4 lines separate by \n
        public List<string> ItemListSelected { get; set; }      // List of all Items selected to be included in the report
        public List<string> ItemListNotSelected { get; set; }   // List of all Items not selected to be included in the report
        public List<string> ItemListIgnore { get; set; }        // List of itmes to not be included in the Other payments report
        public string ConfigFileName { get; set; }              // name of file containing configuration information
        public bool ReportOtherPayments { get; set; }           // Report items that are not included in the donation in the Other Payments table

        public ConfigurationDYES()
        {
            ItemListSelected = [];
            ItemListNotSelected = [];
            ItemListIgnore = [];
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
                MessageBox.Show("Error: " + ex.ToString());
            }
            return true;
        }

        public static bool DeSerialize(string fileName, ref ConfigurationDYES cfg)
        {
            if (!File.Exists(fileName))
                return true;
            try
            {
                using (FileStream fileStream = new(fileName, FileMode.Open))
                {
                    XmlSerializer ser = new(typeof(ConfigurationDYES));
                    cfg = (ConfigurationDYES)ser.Deserialize(fileStream);
                }
                cfg.ItemListSelected.Sort();
                cfg.ItemListIgnore.Sort();
            }
            catch (Exception ex)
            {
                string msg = string.Format("Unable to read config file:  {0}\n\n{1}", fileName, ex.ToString());
                MessageBox.Show(msg, "ERROR", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }
    }
}
