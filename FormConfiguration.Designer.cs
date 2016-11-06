// Copyright © 2016  ASM-SW
//asmeyers@outlook.com  https://github.com/asm-sw
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
            this.fileDlgInputFile = new System.Windows.Forms.OpenFileDialog();
            this.butInputFile = new System.Windows.Forms.Button();
            this.textInputFile = new System.Windows.Forms.TextBox();
            this.textWordTemplate = new System.Windows.Forms.TextBox();
            this.butWordTemplate = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.butOutputFolder = new System.Windows.Forms.Button();
            this.textOutputDirectory = new System.Windows.Forms.TextBox();
            this.textConfigFile = new System.Windows.Forms.TextBox();
            this.textDateRange = new System.Windows.Forms.TextBox();
            this.labelDateRange = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // fileDlgInputFile
            // 
            this.fileDlgInputFile.Filter = "comma seperated files|*.csv";
            this.fileDlgInputFile.Title = "Select Input File";
            // 
            // butInputFile
            // 
            this.butInputFile.AutoSize = true;
            this.butInputFile.Location = new System.Drawing.Point(30, 8);
            this.butInputFile.Margin = new System.Windows.Forms.Padding(2);
            this.butInputFile.Name = "butInputFile";
            this.butInputFile.Size = new System.Drawing.Size(60, 23);
            this.butInputFile.TabIndex = 0;
            this.butInputFile.Text = "Input File";
            this.butInputFile.UseVisualStyleBackColor = true;
            this.butInputFile.Click += new System.EventHandler(this.butInputFile_Click);
            // 
            // textInputFile
            // 
            this.textInputFile.Location = new System.Drawing.Point(101, 8);
            this.textInputFile.Margin = new System.Windows.Forms.Padding(2);
            this.textInputFile.Name = "textInputFile";
            this.textInputFile.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textInputFile.Size = new System.Drawing.Size(249, 20);
            this.textInputFile.TabIndex = 1;
            // 
            // textWordTemplate
            // 
            this.textWordTemplate.Location = new System.Drawing.Point(101, 41);
            this.textWordTemplate.Margin = new System.Windows.Forms.Padding(2);
            this.textWordTemplate.Name = "textWordTemplate";
            this.textWordTemplate.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textWordTemplate.Size = new System.Drawing.Size(249, 20);
            this.textWordTemplate.TabIndex = 3;
            // 
            // butWordTemplate
            // 
            this.butWordTemplate.AutoSize = true;
            this.butWordTemplate.Location = new System.Drawing.Point(2, 39);
            this.butWordTemplate.Margin = new System.Windows.Forms.Padding(2);
            this.butWordTemplate.Name = "butWordTemplate";
            this.butWordTemplate.Size = new System.Drawing.Size(90, 23);
            this.butWordTemplate.TabIndex = 2;
            this.butWordTemplate.Text = "Word Template";
            this.butWordTemplate.UseVisualStyleBackColor = true;
            this.butWordTemplate.Click += new System.EventHandler(this.butWordTemplate_Click);
            // 
            // butOutputFolder
            // 
            this.butOutputFolder.Location = new System.Drawing.Point(9, 74);
            this.butOutputFolder.Margin = new System.Windows.Forms.Padding(2);
            this.butOutputFolder.Name = "butOutputFolder";
            this.butOutputFolder.Size = new System.Drawing.Size(78, 21);
            this.butOutputFolder.TabIndex = 4;
            this.butOutputFolder.Text = "Output Folder";
            this.butOutputFolder.UseVisualStyleBackColor = true;
            this.butOutputFolder.Click += new System.EventHandler(this.butOutputFolder_Click);
            // 
            // textOutputDirectory
            // 
            this.textOutputDirectory.Location = new System.Drawing.Point(101, 74);
            this.textOutputDirectory.Margin = new System.Windows.Forms.Padding(2);
            this.textOutputDirectory.Name = "textOutputDirectory";
            this.textOutputDirectory.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textOutputDirectory.Size = new System.Drawing.Size(249, 20);
            this.textOutputDirectory.TabIndex = 5;
            // 
            // textConfigFile
            // 
            this.textConfigFile.Enabled = false;
            this.textConfigFile.Location = new System.Drawing.Point(9, 248);
            this.textConfigFile.Margin = new System.Windows.Forms.Padding(2);
            this.textConfigFile.Name = "textConfigFile";
            this.textConfigFile.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textConfigFile.Size = new System.Drawing.Size(529, 20);
            this.textConfigFile.TabIndex = 6;
            // 
            // textDateRange
            // 
            this.textDateRange.Location = new System.Drawing.Point(101, 110);
            this.textDateRange.Margin = new System.Windows.Forms.Padding(2);
            this.textDateRange.Name = "textDateRange";
            this.textDateRange.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textDateRange.Size = new System.Drawing.Size(249, 20);
            this.textDateRange.TabIndex = 8;
            this.textDateRange.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // labelDateRange
            // 
            this.labelDateRange.AutoSize = true;
            this.labelDateRange.Location = new System.Drawing.Point(6, 113);
            this.labelDateRange.Name = "labelDateRange";
            this.labelDateRange.Size = new System.Drawing.Size(65, 13);
            this.labelDateRange.TabIndex = 10;
            this.labelDateRange.Text = "Date Range";
            // 
            // FormConfiguration
            // 
            this.AccessibleDescription = "Fill in the following items:";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 273);
            this.ControlBox = false;
            this.Controls.Add(this.labelDateRange);
            this.Controls.Add(this.textDateRange);
            this.Controls.Add(this.textConfigFile);
            this.Controls.Add(this.textOutputDirectory);
            this.Controls.Add(this.butOutputFolder);
            this.Controls.Add(this.textWordTemplate);
            this.Controls.Add(this.butWordTemplate);
            this.Controls.Add(this.textInputFile);
            this.Controls.Add(this.butInputFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormConfiguration";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "ConfigurationForm";
            this.VisibleChanged += new System.EventHandler(this.FormConfiguration_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog fileDlgInputFile;
        private System.Windows.Forms.Button butInputFile;
        private System.Windows.Forms.TextBox textInputFile;
        private System.Windows.Forms.TextBox textWordTemplate;
        private System.Windows.Forms.Button butWordTemplate;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button butOutputFolder;
        private System.Windows.Forms.TextBox textOutputDirectory;
        private System.Windows.Forms.TextBox textConfigFile;
        private System.Windows.Forms.TextBox textDateRange;
        private System.Windows.Forms.Label labelDateRange;
    }
}