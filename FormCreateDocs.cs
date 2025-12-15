// Copyright © 2016-2024 ASM-SW
//asm-sw@outlook.com  https://github.com/asm-sw
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Threading;
using System.Windows.Forms;

namespace DonorStatement
{
    public partial class FormCreateDocs : Form
    {
        readonly FileParser m_parser;
        readonly DocumentCreator m_docCreator;
        readonly LogMessageDelegate m_logger;
        bool m_CancelPending = false;  // Set to true to cancel the background thread, bools are atomic per spec
        Thread m_backgroundThread = null;

        public FormCreateDocs(ref FileParser parser, ref DocumentCreator docCreator, ref LogMessageDelegate logger)
        {
            m_parser = parser;
            m_logger = logger;
            m_docCreator = docCreator;

            InitializeComponent();
            buttonStop.Enabled = false;
            UpdateUI(0, 0);
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            ConfigurationDYES Config = FormMain.Config;
            Config.Serialize(Config.ConfigFileName);
            buttonReloadFile.Enabled = false;
            buttonStart.Enabled = false;
            buttonStop.Enabled = true;

            m_backgroundThread = new(new ThreadStart(BackgroundWork));
            m_backgroundThread.SetApartmentState(ApartmentState.STA);
            m_backgroundThread.Start();
        }

        private void BackgroundWork()
        {
            m_logger(string.Format("Start: {0}", DateTimeOffset.Now.ToString("HH:mm:ss")));
            if(!m_docCreator.CreateDoc())
            {
                m_docCreator.CreateDocsDone();
                EnableButtons();
                return;
            }

            if(!m_parser.FileHasBeenRead)
                m_parser.ParseInputFile();

            m_parser.GetColumnNames(out List<string> columnNames);
            if (!ColumnMap.CheckForColumns(ref columnNames))
            {
                m_docCreator.CreateDocsDone();
                EnableButtons();
                return;
            }
            m_docCreator.CheckForBookmarksAndTables();

            m_parser.GetNameList(out List<string> names);

            for (int i = 0; i < names.Count; i++)
            {
                if (m_CancelPending)
                {
                    m_logger(string.Format("User canceled the program.  {0} of {1} documents completed", i, names.Count));
                    UpdateUI(0, 0);
                    break;
                }
                m_parser.GetDataForName(names[i], out DataTable table);
                m_docCreator.CreateDoc(table);
                UpdateUI(i+1, names.Count);
            }
            m_CancelPending = false;
            m_docCreator.CreateDocsDone();
            m_docCreator.SaveFileList();
            m_logger(string.Format("Finish: {0}", DateTimeOffset.Now.ToString("HH:mm:ss")));
            EnableButtons();
        }

        private void EnableButtons()
        {
            if (buttonStop.InvokeRequired)
            {
                buttonStop.Invoke(new Action(EnableButtons));
                return;
            }
            buttonStop.Enabled = false;
            buttonReloadFile.Enabled = true;
            buttonStart.Enabled = true;
        }

        private void UpdateUI(int step, int totalSteps)
        {
            if (labelProgress.InvokeRequired)
            {
                labelProgress.Invoke(new Action<int, int>(UpdateUI), step, totalSteps);
                return;
            }
            if (totalSteps == 0)
            {
                labelProgress.Text = string.Empty;
                progressBar1.Value = 0;
            }
            else
            {
                labelProgress.Text = string.Format("Step {0} of {1}", step, totalSteps);
                progressBar1.Value = (int)((float)step / (float)totalSteps * 100.0);
            }
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            buttonStart.Enabled = true;
            buttonStop.Enabled = false;
            buttonReloadFile.Enabled = true;
        }

        private void ButtonStop_Click(object sender, EventArgs e)
        {
           m_CancelPending = true;
        }

        private void ButtonReloadFile_Click(object sender, EventArgs e)
        {
            m_parser.ParseInputFile();
        }
    }
}
