// Copyright © 2016  ASM-SW
//asmeyers@outlook.com  https://github.com/asm-sw
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DonorStatement
{
    public partial class FormConfiguration : Form, ISubForm
    {

        public FormConfiguration()
        {
            InitializeComponent();

            textInputFile.Text = FormMain.Config.InputFileName;
            textWordTemplate.Text = FormMain.Config.WordTemplateFileName;
            textOutputDirectory.Text = FormMain.Config.OutputDirectory;
            textConfigFile.Text = "Configuration file: " + FormMain.Config.ConfigFileName;
            textDateRange.Text = FormMain.Config.DateRange;
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

        private void butInputFile_Click(object sender, EventArgs e)
        {
            string fileName = textInputFile.Text;
            if (SelectFile(ref fileName, "Comma separated file|*.csv|All Files|*.*"))
            {
                FormMain.Config.InputFileName = fileName;
                textInputFile.Text = FormMain.Config.InputFileName;
            }
        }

        private void butWordTemplate_Click(object sender, EventArgs e)
        {
            string fileName = textWordTemplate.Text;
            if (SelectFile(ref fileName, "Word templated file|*.dotx|All Files|*.*"))
            {
                FormMain.Config.WordTemplateFileName = fileName;
                textWordTemplate.Text = FormMain.Config.WordTemplateFileName;
            }
        }

        private void butOutputFolder_Click(object sender, EventArgs e)
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

        private void textBox1_TextChanged(object sender, EventArgs e)
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
            bool res = true;
            StringBuilder msg = new StringBuilder("Errors in Configuration: \n");

            
            FormMain.Config.InputFileName = textInputFile.Text;
            if(!File.Exists(FormMain.Config.InputFileName))
            {
                msg.AppendFormat("- Input file does not exist: {0}\n", FormMain.Config.InputFileName);
                res = false;
            }

            FormMain.Config.WordTemplateFileName = textWordTemplate.Text;
            if(!File.Exists(FormMain.Config.WordTemplateFileName))
            {
                msg.AppendFormat("- Word template file does not exist: {0}\n", FormMain.Config.WordTemplateFileName);
                res = false;
            }

            FormMain.Config.OutputDirectory = textOutputDirectory.Text;
            if(!Directory.Exists(FormMain.Config.OutputDirectory))
            {
                msg.AppendFormat("- Output Directory does not exist: {0}\n", FormMain.Config.OutputDirectory);
                res = false;
            }

            if (res)
                errorMsg = string.Empty;
            else
                errorMsg = msg.ToString();

            return res;
        }
    }
}
