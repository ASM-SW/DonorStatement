// Copyright © 2016-2026 ASM-SW
//asm-sw@outlook.com  https://github.com/asm-sw
namespace DonorStatement
{
    partial class FormFileParser
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
            butParse = new System.Windows.Forms.Button();
            listDonations = new System.Windows.Forms.ListBox();
            textFileHasBeenRead = new System.Windows.Forms.TextBox();
            listNotDonations = new System.Windows.Forms.ListBox();
            listIgnore = new System.Windows.Forms.ListBox();
            butSelectToNotSelect = new System.Windows.Forms.Button();
            buttonNotSelectedToExcluded = new System.Windows.Forms.Button();
            buttonNotSelectedToSelected = new System.Windows.Forms.Button();
            buttonExcludedToNotSelected = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // butParse
            // 
            butParse.Location = new System.Drawing.Point(10, 9);
            butParse.Margin = new System.Windows.Forms.Padding(2);
            butParse.Name = "butParse";
            butParse.Size = new System.Drawing.Size(74, 24);
            butParse.TabIndex = 0;
            butParse.Text = "Parse Input File";
            butParse.UseVisualStyleBackColor = true;
            butParse.Click += ButtonParse_Click;
            // 
            // listDonations
            // 
            listDonations.FormattingEnabled = true;
            listDonations.HorizontalScrollbar = true;
            listDonations.Location = new System.Drawing.Point(22, 72);
            listDonations.Margin = new System.Windows.Forms.Padding(2);
            listDonations.Name = "listDonations";
            listDonations.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            listDonations.Size = new System.Drawing.Size(204, 229);
            listDonations.Sorted = true;
            listDonations.TabIndex = 1;
            // 
            // textFileHasBeenRead
            // 
            textFileHasBeenRead.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textFileHasBeenRead.Enabled = false;
            textFileHasBeenRead.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            textFileHasBeenRead.Location = new System.Drawing.Point(90, 9);
            textFileHasBeenRead.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textFileHasBeenRead.Multiline = true;
            textFileHasBeenRead.Name = "textFileHasBeenRead";
            textFileHasBeenRead.ReadOnly = true;
            textFileHasBeenRead.Size = new System.Drawing.Size(447, 24);
            textFileHasBeenRead.TabIndex = 2;
            // 
            // listNotDonations
            // 
            listNotDonations.FormattingEnabled = true;
            listNotDonations.HorizontalScrollbar = true;
            listNotDonations.Location = new System.Drawing.Point(311, 72);
            listNotDonations.Margin = new System.Windows.Forms.Padding(2);
            listNotDonations.Name = "listNotDonations";
            listNotDonations.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            listNotDonations.Size = new System.Drawing.Size(207, 229);
            listNotDonations.Sorted = true;
            listNotDonations.TabIndex = 5;
            // 
            // listIgnore
            // 
            listIgnore.FormattingEnabled = true;
            listIgnore.Location = new System.Drawing.Point(603, 72);
            listIgnore.Margin = new System.Windows.Forms.Padding(2);
            listIgnore.Name = "listIgnore";
            listIgnore.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            listIgnore.Size = new System.Drawing.Size(154, 229);
            listIgnore.Sorted = true;
            listIgnore.TabIndex = 6;
            // 
            // butSelectToNotSelect
            // 
            butSelectToNotSelect.Location = new System.Drawing.Point(231, 124);
            butSelectToNotSelect.Name = "butSelectToNotSelect";
            butSelectToNotSelect.Size = new System.Drawing.Size(75, 23);
            butSelectToNotSelect.TabIndex = 7;
            butSelectToNotSelect.Text = ">>>>";
            butSelectToNotSelect.UseVisualStyleBackColor = true;
            butSelectToNotSelect.Click += butSelectToNotSelect_Click;
            // 
            // buttonNotSelectedToExcluded
            // 
            buttonNotSelectedToExcluded.Location = new System.Drawing.Point(523, 124);
            buttonNotSelectedToExcluded.Name = "buttonNotSelectedToExcluded";
            buttonNotSelectedToExcluded.Size = new System.Drawing.Size(75, 23);
            buttonNotSelectedToExcluded.TabIndex = 8;
            buttonNotSelectedToExcluded.Text = ">>>>";
            buttonNotSelectedToExcluded.UseVisualStyleBackColor = true;
            buttonNotSelectedToExcluded.Click += buttonNotSelectedToIgnore_Click;
            // 
            // buttonNotSelectedToSelected
            // 
            buttonNotSelectedToSelected.Location = new System.Drawing.Point(231, 172);
            buttonNotSelectedToSelected.Name = "buttonNotSelectedToSelected";
            buttonNotSelectedToSelected.Size = new System.Drawing.Size(75, 23);
            buttonNotSelectedToSelected.TabIndex = 9;
            buttonNotSelectedToSelected.Text = "<<<<";
            buttonNotSelectedToSelected.UseVisualStyleBackColor = true;
            buttonNotSelectedToSelected.Click += buttonNotSelectedToSelected_Click;
            // 
            // buttonExcludedToNotSelected
            // 
            buttonExcludedToNotSelected.Location = new System.Drawing.Point(523, 172);
            buttonExcludedToNotSelected.Name = "buttonExcludedToNotSelected";
            buttonExcludedToNotSelected.Size = new System.Drawing.Size(75, 23);
            buttonExcludedToNotSelected.TabIndex = 10;
            buttonExcludedToNotSelected.Text = "<<<<";
            buttonExcludedToNotSelected.UseVisualStyleBackColor = true;
            buttonExcludedToNotSelected.Click += buttonIgnoreToNotSelected_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(90, 55);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(61, 15);
            label1.TabIndex = 11;
            label1.Text = "Donations";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(347, 55);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(92, 15);
            label2.TabIndex = 12;
            label2.Text = "Other Payments";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(593, 55);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(41, 15);
            label3.TabIndex = 13;
            label3.Text = "Ignore";
            // 
            // FormFileParser
            // 
            AccessibleDescription = "Select Items to include as donations.";
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(768, 312);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(buttonExcludedToNotSelected);
            Controls.Add(buttonNotSelectedToSelected);
            Controls.Add(buttonNotSelectedToExcluded);
            Controls.Add(butSelectToNotSelect);
            Controls.Add(listIgnore);
            Controls.Add(listNotDonations);
            Controls.Add(textFileHasBeenRead);
            Controls.Add(listDonations);
            Controls.Add(butParse);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Margin = new System.Windows.Forms.Padding(2);
            Name = "FormFileParser";
            Text = "FileParserForm";
            VisibleChanged += FileParserForm_VisibleChanged;
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butParse;
        private System.Windows.Forms.ListBox listDonations;
        private System.Windows.Forms.TextBox textFileHasBeenRead;
        private System.Windows.Forms.ListBox listNotDonations;
        private System.Windows.Forms.ListBox listIgnore;
        private System.Windows.Forms.Button butSelectToNotSelect;
        private System.Windows.Forms.Button buttonNotSelectedToExcluded;
        private System.Windows.Forms.Button buttonNotSelectedToSelected;
        private System.Windows.Forms.Button buttonExcludedToNotSelected;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}