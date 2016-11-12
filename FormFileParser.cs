// Copyright © 2016  ASM-SW
//asmeyers@outlook.com  https://github.com/asm-sw
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DonorStatement
{
    public partial class FormFileParser : Form
    {
        FileParser m_parser;
        public FormFileParser(ref FileParser parser)
        {
            m_parser = parser;
            InitializeComponent();

            // for each item coming in from file parser, load it into the control and select it.
            foreach (string item in FormMain.Config.ItemListSelected)
                listItems.Items.Add(item);
            for (int i = 0; i < listItems.Items.Count; i++)
                listItems.SetSelected(i, true);

            SetTextFileHasBeenRead();
        }

        private void SetTextFileHasBeenRead()
        {
            if (m_parser.FileHasBeenRead)
                textFileHasBeenRead.Text = "Input File has been read in";
            else
                textFileHasBeenRead.Text = "Input File has not been read.  Click on Parse to read it and update the list of items.";
        }

        private void butParse_Click(object sender, EventArgs e)
        {
            m_parser.ParseInputFile();
            List<string> itemListFromFile;
            m_parser.GetItemList(out itemListFromFile);

            // replace item list in configuration with new one.
            // also populate control
            listItems.Items.Clear();
            List<string> newItemList = new List<string>();
            foreach (string item in itemListFromFile)
            {
                // if item was in old list put in new list with same selection value.
                if (FormMain.Config.ItemListSelected.BinarySearch(item) >= 0)
                {
                    listItems.Items.Add(item);
                    listItems.SelectedItems.Add(item);
                }
                else
                    listItems.Items.Add(item);
                newItemList.Add(item);
            }
            UpdateConfigurationWithSelectedItems();

            // scroll to top
            if (listItems.Items.Count > 0)
                listItems.TopIndex = 0;
            SetTextFileHasBeenRead();
        }


        private void FileParserForm_VisibleChanged(object sender, EventArgs e)
        {
            if (((System.Windows.Forms.Control)sender).Visible)
                return;  // became visible, do nothing

            UpdateConfigurationWithSelectedItems();
        }

        private void UpdateConfigurationWithSelectedItems()
        {
            FormMain.Config.ItemListSelected.Clear();
            foreach (string item in listItems.SelectedItems)
                FormMain.Config.ItemListSelected.Add(item);

            FormMain.Config.ItemListSelected.Sort();
        }

        private void CheckUnCheckAll(bool bSelect)
        {
            listItems.Visible = false;
            if (bSelect)
            {
                for (int i = 0; i < listItems.Items.Count; i++)
                    listItems.SetSelected(i, true);
            }
            else
                listItems.SelectedItems.Clear();
            listItems.Visible = true;
        }

        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            CheckUnCheckAll(true);
        }

        private void buttonClearSelections_Click(object sender, EventArgs e)
        {
            CheckUnCheckAll(false);
        }

    
    }
}
