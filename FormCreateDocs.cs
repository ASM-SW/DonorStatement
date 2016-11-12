// Copyright © 2016  ASM-SW
//asmeyers@outlook.com  https://github.com/asm-sw
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace DonorStatement
{
    public partial class FormCreateDocs : Form
    {
        private FormCreateDocs() { }

        FileParser m_parser;
        DocumentCreator m_docCreator;
        LogMessageDelegate m_logger;
        string m_strProgress = string.Empty;
        object m_strProgressLock = new object();

        public FormCreateDocs(ref FileParser parser, ref DocumentCreator docCreator, ref LogMessageDelegate logger)
        {
            m_parser = parser;
            m_logger = logger;
            m_docCreator = docCreator;

            InitializeComponent();

            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.WorkerReportsProgress = true;
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            buttonReloadFile.Enabled = false;
            buttonStart.Enabled = false;
            buttonStop.Enabled = true;

            backgroundWorker1.RunWorkerAsync();
        }

        private void BackgroundWork(BackgroundWorker worker, DoWorkEventArgs e)
        {
            m_strProgress = string.Empty;
            m_docCreator.CreateDoc();

            if(!m_parser.FileHasBeenRead)
                m_parser.ParseInputFile();

            List<string> columnNames;
            m_parser.GetColumnNames(out columnNames);
            if(!m_docCreator.CheckForColumns(ref columnNames))
            {
                m_docCreator.CreateDocsDone();
                return;
            }
            m_docCreator.CheckForBookmarks();

            List<string> names;
            m_parser.GetNameList(out names);

            for (int i = 0; i < names.Count; i++)
            {
                if (worker.CancellationPending)
                {
                    m_logger(string.Format("User canceled the program.  {0} of {1} documents completed", i, names.Count));
                    worker.ReportProgress(0);
                    e.Cancel = true;
                    break;
                }
                DataTable table;
                m_parser.GetDataForName(names[i], out table);
                m_docCreator.CreateDoc(table);

                int percentComplete = (int)((float)(i + 1) / (float)names.Count * 100.0);
                lock (m_strProgressLock)
                {
                    m_strProgress = string.Format("{0} of {1} completed", i + 1, names.Count);
                }
                worker.ReportProgress(percentComplete);
            }
            m_docCreator.CreateDocsDone();
            m_docCreator.SaveFileList();
        }


        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            lock (m_strProgressLock)
            {
                labelProgress.Text = m_strProgress; 
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            buttonStart.Enabled = true;
            buttonStop.Enabled = false;
            buttonReloadFile.Enabled = true;
        }
        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            BackgroundWork(worker, e);
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
                backgroundWorker1.CancelAsync();
        }

        private void buttonReloadFile_Click(object sender, EventArgs e)
        {
            m_parser.ParseInputFile();
        }
    }
}
