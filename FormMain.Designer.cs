// Copyright © 2016-2024 ASM-SW
//asm-sw@outlook.com  https://github.com/asm-sw
using System;

namespace DonorStatement
{
    partial class FormMain
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
            if (disposing)
                m_parser.Dispose();

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            lbLogging = new System.Windows.Forms.ListBox();
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            copyToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            selectAllToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            panel1 = new System.Windows.Forms.Panel();
            butNext = new System.Windows.Forms.Button();
            butBack = new System.Windows.Forms.Button();
            labelProgress = new System.Windows.Forms.Label();
            labelStep = new System.Windows.Forms.Label();
            buttonAbout = new System.Windows.Forms.Button();
            buttonHelp = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            Exit = new System.Windows.Forms.Button();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // lbLogging
            // 
            lbLogging.BackColor = System.Drawing.Color.GhostWhite;
            lbLogging.ContextMenuStrip = contextMenuStrip1;
            lbLogging.Dock = System.Windows.Forms.DockStyle.Bottom;
            lbLogging.FormattingEnabled = true;
            lbLogging.HorizontalScrollbar = true;
            lbLogging.ItemHeight = 15;
            lbLogging.Location = new System.Drawing.Point(0, 468);
            lbLogging.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            lbLogging.Name = "lbLogging";
            lbLogging.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            lbLogging.Size = new System.Drawing.Size(957, 154);
            lbLogging.TabIndex = 1;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { clearToolStripMenuItem, copyToolStripMenuItem1, selectAllToolStripMenuItem1 });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(123, 70);
            contextMenuStrip1.ItemClicked += ContextMenuStrip1_ItemClicked;
            // 
            // clearToolStripMenuItem
            // 
            clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            clearToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            clearToolStripMenuItem.Text = "C&lear";
            // 
            // copyToolStripMenuItem1
            // 
            copyToolStripMenuItem1.Name = "copyToolStripMenuItem1";
            copyToolStripMenuItem1.Size = new System.Drawing.Size(122, 22);
            copyToolStripMenuItem1.Text = "&Copy";
            // 
            // selectAllToolStripMenuItem1
            // 
            selectAllToolStripMenuItem1.Name = "selectAllToolStripMenuItem1";
            selectAllToolStripMenuItem1.Size = new System.Drawing.Size(122, 22);
            selectAllToolStripMenuItem1.Text = "Select &All";
            // 
            // panel1
            // 
            panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel1.Location = new System.Drawing.Point(6, 135);
            panel1.Margin = new System.Windows.Forms.Padding(2);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(943, 318);
            panel1.TabIndex = 4;
            // 
            // butNext
            // 
            butNext.Location = new System.Drawing.Point(126, 20);
            butNext.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            butNext.Name = "butNext";
            butNext.Size = new System.Drawing.Size(88, 27);
            butNext.TabIndex = 5;
            butNext.Text = "Next";
            butNext.UseVisualStyleBackColor = true;
            butNext.Click += ButNext_Click;
            // 
            // butBack
            // 
            butBack.Location = new System.Drawing.Point(14, 20);
            butBack.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            butBack.Name = "butBack";
            butBack.Size = new System.Drawing.Size(88, 27);
            butBack.TabIndex = 6;
            butBack.Text = "Back";
            butBack.UseVisualStyleBackColor = true;
            butBack.Click += ButBack_Click;
            // 
            // labelProgress
            // 
            labelProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            labelProgress.Location = new System.Drawing.Point(10, 63);
            labelProgress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelProgress.Name = "labelProgress";
            labelProgress.Size = new System.Drawing.Size(155, 25);
            labelProgress.TabIndex = 8;
            labelProgress.Text = "Step 0 of 0";
            // 
            // labelStep
            // 
            labelStep.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            labelStep.Location = new System.Drawing.Point(34, 112);
            labelStep.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelStep.Name = "labelStep";
            labelStep.Size = new System.Drawing.Size(632, 21);
            labelStep.TabIndex = 9;
            labelStep.Text = "-";
            labelStep.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonAbout
            // 
            buttonAbout.Location = new System.Drawing.Point(584, 20);
            buttonAbout.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonAbout.Name = "buttonAbout";
            buttonAbout.Size = new System.Drawing.Size(88, 27);
            buttonAbout.TabIndex = 10;
            buttonAbout.Text = "About";
            buttonAbout.UseVisualStyleBackColor = true;
            buttonAbout.Click += ButtonAbout_Click;
            // 
            // buttonHelp
            // 
            buttonHelp.Location = new System.Drawing.Point(584, 51);
            buttonHelp.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonHelp.Name = "buttonHelp";
            buttonHelp.Size = new System.Drawing.Size(88, 27);
            buttonHelp.TabIndex = 11;
            buttonHelp.Text = "Help";
            buttonHelp.UseVisualStyleBackColor = true;
            buttonHelp.Click += ButtonHelp_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            label1.Location = new System.Drawing.Point(10, 97);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(64, 13);
            label1.TabIndex = 12;
            label1.Text = "labelVersion";
            // 
            // Exit
            // 
            Exit.Location = new System.Drawing.Point(584, 82);
            Exit.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Exit.Name = "Exit";
            Exit.Size = new System.Drawing.Size(88, 27);
            Exit.TabIndex = 13;
            Exit.Text = "Exit";
            Exit.UseMnemonic = false;
            Exit.UseVisualStyleBackColor = true;
            Exit.Click += Exit_Click;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(957, 622);
            Controls.Add(Exit);
            Controls.Add(label1);
            Controls.Add(buttonHelp);
            Controls.Add(buttonAbout);
            Controls.Add(labelStep);
            Controls.Add(labelProgress);
            Controls.Add(butBack);
            Controls.Add(butNext);
            Controls.Add(panel1);
            Controls.Add(lbLogging);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormMain";
            Text = "Donor Year End Statement";
            FormClosed += FormMain_FormClosed;
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }


        #endregion
        private System.Windows.Forms.ListBox lbLogging;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button butNext;
        private System.Windows.Forms.Button butBack;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.Label labelStep;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.Button buttonAbout;
        private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Exit;
    }
}

