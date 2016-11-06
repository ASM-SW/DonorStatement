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

            foreach (string item in FormMain.Config.ItemListSelected)
                checkedListItems.Items.Add(item, true);
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
            checkedListItems.Items.Clear();
            List<string> newItemList = new List<string>();
            foreach (string item in itemListFromFile)
            {
                // if item was in old list put in new list with same selection value.
                if (FormMain.Config.ItemListSelected.BinarySearch(item) >= 0)
                    checkedListItems.Items.Add(item, true);
                else
                    checkedListItems.Items.Add(item, false);
                newItemList.Add(item);
            }
            FillItemListSelected();

            // scroll to top
            if (checkedListItems.Items.Count > 0)
                checkedListItems.TopIndex = 0;
            checkedListItems.SelectedItems.Clear();
            SetTextFileHasBeenRead();
        }


        private void FileParserForm_VisibleChanged(object sender, EventArgs e)
        {
            if (((System.Windows.Forms.Control)sender).Visible)
                return;

            FillItemListSelected();
        }

        private void FillItemListSelected()
        {
            FormMain.Config.ItemListSelected.Clear();
            foreach (string item in checkedListItems.CheckedItems)
                FormMain.Config.ItemListSelected.Add(item);

            FormMain.Config.ItemListSelected.Sort();
        }

        private void CheckUnCheckAll(bool bChecked)
        {
            for (int i = 0; i < checkedListItems.Items.Count; ++i)
                checkedListItems.SetItemChecked(i, bChecked);
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
