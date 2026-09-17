// Copyright © 2016-2024 ASM-SW
//asm-sw@outlook.com  https://github.com/asm-sw
namespace DonorStatement
{
    partial class FormConfiguration
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            fileDlgInputFile = new System.Windows.Forms.OpenFileDialog();
            butInputFile = new System.Windows.Forms.Button();
            textInputFile = new System.Windows.Forms.TextBox();
            textWordTemplate = new System.Windows.Forms.TextBox();
            ButLetterTemplate = new System.Windows.Forms.Button();
            folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            butOutputFolder = new System.Windows.Forms.Button();
            textOutputDirectory = new System.Windows.Forms.TextBox();
            textConfigFile = new System.Windows.Forms.TextBox();
            textDateRange = new System.Windows.Forms.TextBox();
            labelDateRange = new System.Windows.Forms.Label();
            cbReportOtherPayments = new System.Windows.Forms.CheckBox();
            SuspendLayout();
            // 
            // fileDlgInputFile
            // 
            fileDlgInputFile.Filter = "comma seperated files|*.csv";
            fileDlgInputFile.Title = "Select Input File";
            // 
            // butInputFile
            // 
            butInputFile.AutoSize = true;
            butInputFile.Location = new System.Drawing.Point(37, 9);
            butInputFile.Margin = new System.Windows.Forms.Padding(2);
            butInputFile.Name = "butInputFile";
            butInputFile.Size = new System.Drawing.Size(70, 27);
            butInputFile.TabIndex = 0;
            butInputFile.Text = "Input File";
            butInputFile.UseVisualStyleBackColor = true;
            butInputFile.Click += ButInputFile_Click;
            // 
            // textInputFile
            // 
            textInputFile.Location = new System.Drawing.Point(118, 9);
            textInputFile.Margin = new System.Windows.Forms.Padding(2);
            textInputFile.Name = "textInputFile";
            textInputFile.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            textInputFile.Size = new System.Drawing.Size(811, 23);
            textInputFile.TabIndex = 1;
            // 
            // textWordTemplate
            // 
            textWordTemplate.Location = new System.Drawing.Point(118, 47);
            textWordTemplate.Margin = new System.Windows.Forms.Padding(2);
            textWordTemplate.Name = "textWordTemplate";
            textWordTemplate.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            textWordTemplate.Size = new System.Drawing.Size(811, 23);
            textWordTemplate.TabIndex = 3;
            // 
            // ButLetterTemplate
            // 
            ButLetterTemplate.AutoSize = true;
            ButLetterTemplate.Location = new System.Drawing.Point(2, 45);
            ButLetterTemplate.Margin = new System.Windows.Forms.Padding(2);
            ButLetterTemplate.Name = "ButLetterTemplate";
            ButLetterTemplate.Size = new System.Drawing.Size(105, 27);
            ButLetterTemplate.TabIndex = 2;
            ButLetterTemplate.Text = "Letter Template";
            ButLetterTemplate.UseVisualStyleBackColor = true;
            ButLetterTemplate.Click += ButLetterTemplate_Click;
            // 
            // butOutputFolder
            // 
            butOutputFolder.Location = new System.Drawing.Point(16, 85);
            butOutputFolder.Margin = new System.Windows.Forms.Padding(2);
            butOutputFolder.Name = "butOutputFolder";
            butOutputFolder.Size = new System.Drawing.Size(91, 24);
            butOutputFolder.TabIndex = 4;
            butOutputFolder.Text = "Output Folder";
            butOutputFolder.UseVisualStyleBackColor = true;
            butOutputFolder.Click += ButOutputFolder_Click;
            // 
            // textOutputDirectory
            // 
            textOutputDirectory.Location = new System.Drawing.Point(118, 85);
            textOutputDirectory.Margin = new System.Windows.Forms.Padding(2);
            textOutputDirectory.Name = "textOutputDirectory";
            textOutputDirectory.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            textOutputDirectory.Size = new System.Drawing.Size(811, 23);
            textOutputDirectory.TabIndex = 5;
            // 
            // textConfigFile
            // 
            textConfigFile.Enabled = false;
            textConfigFile.Location = new System.Drawing.Point(-2, 279);
            textConfigFile.Margin = new System.Windows.Forms.Padding(2);
            textConfigFile.Name = "textConfigFile";
            textConfigFile.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            textConfigFile.Size = new System.Drawing.Size(910, 23);
            textConfigFile.TabIndex = 6;
            // 
            // textDateRange
            // 
            textDateRange.Location = new System.Drawing.Point(118, 127);
            textDateRange.Margin = new System.Windows.Forms.Padding(2);
            textDateRange.Name = "textDateRange";
            textDateRange.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            textDateRange.Size = new System.Drawing.Size(811, 23);
            textDateRange.TabIndex = 8;
            textDateRange.TextChanged += TextBoxDateRange_TextChanged;
            // 
            // labelDateRange
            // 
            labelDateRange.AutoSize = true;
            labelDateRange.Location = new System.Drawing.Point(31, 130);
            labelDateRange.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelDateRange.Name = "labelDateRange";
            labelDateRange.Size = new System.Drawing.Size(67, 15);
            labelDateRange.TabIndex = 10;
            labelDateRange.Text = "Date Range";
            // 
            // cbReportOtherPayments
            // 
            cbReportOtherPayments.AutoSize = true;
            cbReportOtherPayments.Location = new System.Drawing.Point(118, 186);
            cbReportOtherPayments.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cbReportOtherPayments.Name = "cbReportOtherPayments";
            cbReportOtherPayments.Size = new System.Drawing.Size(268, 19);
            cbReportOtherPayments.TabIndex = 11;
            cbReportOtherPayments.Text = "Report non-donation in Other Payments table";
            cbReportOtherPayments.UseVisualStyleBackColor = true;
            cbReportOtherPayments.CheckedChanged += CbReportOtherPayments_CheckedChanged;
            // 
            // FormConfiguration
            // 
            AccessibleDescription = "Fill in the following items:";
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(943, 315);
            ControlBox = false;
            Controls.Add(cbReportOtherPayments);
            Controls.Add(labelDateRange);
            Controls.Add(textDateRange);
            Controls.Add(textConfigFile);
            Controls.Add(textOutputDirectory);
            Controls.Add(butOutputFolder);
            Controls.Add(textWordTemplate);
            Controls.Add(ButLetterTemplate);
            Controls.Add(textInputFile);
            Controls.Add(butInputFile);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Margin = new System.Windows.Forms.Padding(2);
            Name = "FormConfiguration";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "ConfigurationForm";
            VisibleChanged += FormConfiguration_VisibleChanged;
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog fileDlgInputFile;
        private System.Windows.Forms.Button butInputFile;
        private System.Windows.Forms.TextBox textInputFile;
        private System.Windows.Forms.TextBox textWordTemplate;
        private System.Windows.Forms.Button ButLetterTemplate;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button butOutputFolder;
        private System.Windows.Forms.TextBox textOutputDirectory;
        private System.Windows.Forms.TextBox textConfigFile;
        private System.Windows.Forms.TextBox textDateRange;
        private System.Windows.Forms.Label labelDateRange;
        private System.Windows.Forms.CheckBox cbReportOtherPayments;
    }
}