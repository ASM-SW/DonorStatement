// Copyright © 2016-2019 ASM-SW
//asmeyers@outlook.com  https://github.com/asm-sw
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
            this.butParse = new System.Windows.Forms.Button();
            this.listItems = new System.Windows.Forms.ListBox();
            this.textFileHasBeenRead = new System.Windows.Forms.TextBox();
            this.buttonSelectAll = new System.Windows.Forms.Button();
            this.buttonClearSelections = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // butParse
            // 
            this.butParse.Location = new System.Drawing.Point(9, 8);
            this.butParse.Margin = new System.Windows.Forms.Padding(2);
            this.butParse.Name = "butParse";
            this.butParse.Size = new System.Drawing.Size(63, 21);
            this.butParse.TabIndex = 0;
            this.butParse.Text = "Parse Input File";
            this.butParse.UseVisualStyleBackColor = true;
            this.butParse.Click += new System.EventHandler(this.ButtonParse_Click);
            // 
            // listItems
            // 
            this.listItems.FormattingEnabled = true;
            this.listItems.Location = new System.Drawing.Point(35, 41);
            this.listItems.Margin = new System.Windows.Forms.Padding(2);
            this.listItems.Name = "listItems";
            this.listItems.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listItems.Size = new System.Drawing.Size(601, 199);
            this.listItems.TabIndex = 1;
            // 
            // textFileHasBeenRead
            // 
            this.textFileHasBeenRead.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textFileHasBeenRead.Enabled = false;
            this.textFileHasBeenRead.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textFileHasBeenRead.Location = new System.Drawing.Point(77, 8);
            this.textFileHasBeenRead.Multiline = true;
            this.textFileHasBeenRead.Name = "textFileHasBeenRead";
            this.textFileHasBeenRead.ReadOnly = true;
            this.textFileHasBeenRead.Size = new System.Drawing.Size(383, 34);
            this.textFileHasBeenRead.TabIndex = 2;
            // 
            // buttonSelectAll
            // 
            this.buttonSelectAll.Location = new System.Drawing.Point(35, 245);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectAll.TabIndex = 3;
            this.buttonSelectAll.Text = "Select All";
            this.buttonSelectAll.UseVisualStyleBackColor = true;
            this.buttonSelectAll.Click += new System.EventHandler(this.ButtonSelectAll_Click);
            // 
            // buttonClearSelections
            // 
            this.buttonClearSelections.AutoSize = true;
            this.buttonClearSelections.Location = new System.Drawing.Point(131, 245);
            this.buttonClearSelections.Name = "buttonClearSelections";
            this.buttonClearSelections.Size = new System.Drawing.Size(93, 23);
            this.buttonClearSelections.TabIndex = 4;
            this.buttonClearSelections.Text = "Clear Selections";
            this.buttonClearSelections.UseVisualStyleBackColor = true;
            this.buttonClearSelections.Click += new System.EventHandler(this.ButtonClearSelections_Click);
            // 
            // FormFileParser
            // 
            this.AccessibleDescription = "Select Items to include in letters.";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(658, 270);
            this.Controls.Add(this.buttonClearSelections);
            this.Controls.Add(this.buttonSelectAll);
            this.Controls.Add(this.textFileHasBeenRead);
            this.Controls.Add(this.listItems);
            this.Controls.Add(this.butParse);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormFileParser";
            this.Text = "FileParserForm";
            this.VisibleChanged += new System.EventHandler(this.FileParserForm_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butParse;
        private System.Windows.Forms.ListBox listItems;
        private System.Windows.Forms.TextBox textFileHasBeenRead;
        private System.Windows.Forms.Button buttonSelectAll;
        private System.Windows.Forms.Button buttonClearSelections;
    }
}