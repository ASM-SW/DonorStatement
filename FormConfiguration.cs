// Copyright © 2016-2024 ASM-SW
//asm-sw@outlook.com  https://github.com/asm-sw
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace DonorStatement
{
    public partial class FormConfiguration : Form, ISubForm
    {

        public FormConfiguration()
        {
            InitializeComponent();

            textInputFile.Text = FormMain.Config.InputFileName;
            textWordTemplate.Text = FormMain.Config.PdfTemplateFile;
            textOutputDirectory.Text = FormMain.Config.OutputDirectory;
            textConfigFile.Text = "Configuration file: " + FormMain.Config.ConfigFileName;
            textDateRange.Text = FormMain.Config.DateRange;
            cbReportOtherPayments.Checked = FormMain.Config.ReportOtherPayments;
        }

        private bool SelectFile(ref string fileName, string fileFilter)
        {
            fileDlgInputFile.FileName = fileName;
            fileDlgInputFile.Filter = fileFilter;
            if (!string.IsNullOrWhiteSpace(fileName))
                fileDlgInputFile.InitialDirectory = Path.GetDirectoryName(fileName);
            DialogResult res = fileDlgInputFile.ShowDialog();
            if (res == DialogResult.OK)
            {
                fileName = fileDlgInputFile.FileName;
                return true;
            }
            return false;
        }

        private void ButInputFile_Click(object sender, EventArgs e)
        {
            string fileName = textInputFile.Text;
            if (SelectFile(ref fileName, "Comma separated file|*.csv|All Files|*.*"))
            {
                FormMain.Config.InputFileName = fileName;
                textInputFile.Text = FormMain.Config.InputFileName;
            }
        }

        private void ButLetterTemplate_Click(object sender, EventArgs e)
        {
            string fileName = textWordTemplate.Text;
            if (SelectFile(ref fileName, "Json templated file|*.json|All Files|*.*"))
            {
                FormMain.Config.PdfTemplateFile = fileName;
                textWordTemplate.Text = FormMain.Config.PdfTemplateFile;
            }
        }

        private void ButOutputFolder_Click(object sender, EventArgs e)
        {
            string folderName = textOutputDirectory.Text;
            folderBrowserDialog1.SelectedPath = folderName;
            DialogResult res = folderBrowserDialog1.ShowDialog();
            if(res == DialogResult.OK)
            {
                FormMain.Config.OutputDirectory = folderBrowserDialog1.SelectedPath;
                textOutputDirectory.Text = FormMain.Config.OutputDirectory;
            }
        }

        private void TextBoxDateRange_TextChanged(object sender, EventArgs e)
        {
            FormMain.Config.DateRange = textDateRange.Text;
        }

        private void FormConfiguration_VisibleChanged(object sender, EventArgs e)
        {
            if (((System.Windows.Forms.Control)sender).Visible)
                return;
            ConfigurationDYES cfg = FormMain.Config;
            ConfigurationDYES.DeSerialize(FormMain.Config.ConfigFileName, ref cfg);
        }

        /// <summary>
        /// Validation checks before closing the form.
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool CanExit(out string errorMsg)
        {
            // update fields
            FormMain.Config.OutputDirectory = textOutputDirectory.Text.Trim();
            FormMain.Config.DateRange = textDateRange.Text.Trim();
            FormMain.Config.InputFileName = textInputFile.Text.Trim();
            FormMain.Config.PdfTemplateFile = textWordTemplate.Text.Trim();

            bool res = true;
            StringBuilder msg = new("Errors in Configuration: \n");

            if (FormMain.Config.InputFileName.StartsWith(FormMain.Config.OutputDirectory, StringComparison.CurrentCulture) || 
                FormMain.Config.PdfTemplateFile.StartsWith(FormMain.Config.OutputDirectory, StringComparison.CurrentCulture))
            {
                msg.AppendFormat(CultureInfo.CurrentCulture, "- Input file and Word template file, cannot be in the output directory\n");
                res = false;
            }
                        
            FormMain.Config.InputFileName = textInputFile.Text;
            if(!File.Exists(FormMain.Config.InputFileName))
            {
                msg.AppendFormat(CultureInfo.CurrentCulture, "- Input file does not exist: {0}\n", FormMain.Config.InputFileName);
                res = false;
            }
           
            if(!File.Exists(FormMain.Config.PdfTemplateFile))
            {
                msg.AppendFormat(CultureInfo.CurrentCulture, "- Word template file does not exist: {0}\n", FormMain.Config.PdfTemplateFile);
                res = false;
            }

            if(!Directory.Exists(FormMain.Config.OutputDirectory))
            {
                msg.AppendFormat(CultureInfo.CurrentCulture, "- Output Directory does not exist: {0}\n", FormMain.Config.OutputDirectory);
                res = false;
            }

            if (res)
                errorMsg = string.Empty;
            else
                errorMsg = msg.ToString();

            return res;
        }

        private void CbReportOtherPayments_CheckedChanged(object sender, EventArgs e)
        {
            FormMain.Config.ReportOtherPayments = cbReportOtherPayments.Checked;
        }
    }
}
