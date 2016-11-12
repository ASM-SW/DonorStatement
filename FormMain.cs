// Copyright © 2016  ASM-SW
//asmeyers@outlook.com  https://github.com/asm-sw
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace DonorStatement
{
    public delegate void LogMessageDelegate(string msg);

    public partial class FormMain : Form
    {
        public enum PanelNavDirection
        {
            forward,
            backward
        }

        private List<Form> m_forms = new List<Form>();
        LogMessageDelegate m_loggerDelegate;

        private UInt32 LogMaxLines = 250;
        DocumentCreator m_docCreator;
        FileParser m_parser;
        int m_activeForm = -1;

        public static ConfigurationDYES Config { get; private set; }

        public FormMain()
        {
            m_loggerDelegate = LogMessage;
            ConfigurationDYES cfg = new ConfigurationDYES();
            ConfigurationDYES.DeSerialize(cfg.ConfigFileName, ref cfg);
            Config = cfg;
            m_docCreator = new DocumentCreator(m_loggerDelegate);
            m_parser = new FileParser(m_loggerDelegate);

            InitializeComponent();
            InitializeControls();
        }

        private void InitializeControls()
        {
            // Add forms in the order to be displayed
            m_forms.Add(new FormConfiguration());
            m_forms.Add(new FormFileParser(ref m_parser));
            m_forms.Add(new FormCreateDocs(ref m_parser, ref m_docCreator, ref m_loggerDelegate));

            // initial configuration for each form
            foreach (Form item in m_forms)
            {
                item.TopLevel = false;
                item.AutoScroll = true;
            }
            panel1.Controls.Add(m_forms[0]);
            m_forms[0].Show();
            butBack.Enabled = false;

            // load first form
            SwitchPanelForm(PanelNavDirection.forward);
        }

        private void SetProgressText()
        {
            labelProgress.Text = string.Empty;
            if (m_activeForm < 0)
                return;
            labelProgress.Text = string.Format("Step {0} of {1}", m_activeForm + 1, m_forms.Count);
        }

        private bool SubFormOkToExit()
        {
            if (m_forms[m_activeForm] is ISubForm)
            {
                string errorMsg;
                if (((ISubForm)m_forms[m_activeForm]).CanExit(out errorMsg))
                    return true;

                MessageBox.Show(errorMsg);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Switch between the forms in panel1
        /// </summary>
        /// <param name="direction"> if true select next form if false select previous form</param>
        private void SwitchPanelForm(PanelNavDirection direction)
        {
            SetProgressText();
            if (m_forms.Count == 0)
            {
                labelProgress.Text = string.Empty;
                m_activeForm = -1; // no active form
                if (direction == PanelNavDirection.backward)
                    return;  // no form selected, can't go backward
            }

            if (direction == PanelNavDirection.forward)
            {
                if (m_activeForm == m_forms.Count - 1)
                    return; // already selected last form, can't go forward
                if (m_activeForm >= 0 && !SubFormOkToExit())
                    return;
                ++m_activeForm;
            }
            else
            {
                if (m_activeForm != 0)
                    if (!SubFormOkToExit())
                        return;
                --m_activeForm;
            }

            if (m_forms.Count > 0)
            {
                // remove active form
                panel1.Controls[0].Hide();
                panel1.Controls.RemoveAt(0);
            }
      
            panel1.Controls.Add(m_forms[m_activeForm]);
            panel1.Controls[0].Show();
            labelStep.Text = panel1.Controls[0].AccessibleDescription;

            // figure out button state
            butNext.Enabled = true;
            butBack.Enabled = true;
            if (m_activeForm == 0)
                butBack.Enabled = false;
            if (m_activeForm == m_forms.Count - 1)
                butNext.Enabled = false;

            SetProgressText();
        } // SwitchPanelForm


        /// <summary>
        /// Log a message to the log window
        /// </summary>
        /// <param name="msg">the message</param>
        public void LogMessage(string msg)
        {
            if (lbLogging.InvokeRequired)
            {
                LogMessageDelegate update = new LogMessageDelegate(LogMessage);
                lbLogging.Invoke(update, msg);
            }
            else
            {
                lbLogging.ClearSelected();
                // add item at top, remove lines at bottom
                lbLogging.Items.Insert(0, msg);
                while (lbLogging.Items.Count > LogMaxLines - 1)
                    lbLogging.Items.RemoveAt(lbLogging.Items.Count - 1);
            }
        } // LogMessage

        /// <summary>
        /// Contect menu for the logging control.  Provides way to select all, copy and clear.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "&Copy":
                    {
                        StringBuilder str = new StringBuilder();
                        if (lbLogging.SelectedItems.Count == 0)
                            break;
                        foreach (var item in lbLogging.SelectedItems)
                            str.AppendLine(item.ToString());
                        Clipboard.SetText(str.ToString());
                    }
                    break;
                case "C&lear":
                    lbLogging.Items.Clear();
                    break;
                case "Select &All":
                    for (int i = 0; i < lbLogging.Items.Count; ++i)
                        lbLogging.SetSelected(i, true);
                    break;
                default:
                    break;
            }

        }// contextMenuStrip1_ItemClicked

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_docCreator.Close();
            Config.Serialize(Config.ConfigFileName);
        }

        private void butNext_Click(object sender, EventArgs e)
        {
            SwitchPanelForm(PanelNavDirection.forward);
        }

        private void butBack_Click(object sender, EventArgs e)
        {
            SwitchPanelForm(PanelNavDirection.backward);

        }

        private void buttonAbout_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutBox = new AboutBox1(this);
            aboutBox.ShowDialog();
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DonorStatement.pdf");
            try
            {
                System.Diagnostics.Process.Start(fileName);
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to open help file: " + fileName);
            }
        }
    } // Form1
}
