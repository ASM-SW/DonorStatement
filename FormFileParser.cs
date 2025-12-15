// Copyright © 2016-2024 ASM-SW
//asm-sw@outlook.com  https://github.com/asm-sw
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DonorStatement
{
    public partial class FormFileParser : Form
    {
        readonly FileParser m_parser;
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

        private void ButtonParse_Click(object sender, EventArgs e)
        {
            m_parser.ParseInputFile();
            m_parser.GetItemList(out List<string> itemListFromFile);
            itemListFromFile.Sort();

            // replace item list in configuration with new one.
            // also populate control
            listItems.Items.Clear();
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

            }

            // remove any items from ItemListNotSelected that is not in itemListFromFile
            List<int> indexItemsToRemove = [];
            for (int i = 0; i < FormMain.Config.ItemListNotSelected.Count; i++)
            {
                if (itemListFromFile.BinarySearch(FormMain.Config.ItemListNotSelected[i]) < 0)
                    indexItemsToRemove.Insert(0, i);
            }
            // The iteration and remove is working because theindexItemsToRemove is automatically reverse sorted by inserting at the beginning, see above
            foreach (int item in indexItemsToRemove)
                FormMain.Config.ItemListNotSelected.RemoveAt(item);

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
            for (int i = 0; i < listItems.Items.Count; i++)
            {
                if (listItems.GetSelected(i))
                    FormMain.Config.ItemListSelected.Add(listItems.GetItemText(listItems.Items[i]));
                else
                {
                    if (!FormMain.Config.ItemListNotSelected.Contains(listItems.Items[i].ToString()))
                        FormMain.Config.ItemListNotSelected.Add(listItems.GetItemText(listItems.Items[i]));
                }
            }
            FormMain.Config.ItemListSelected.Sort();
            FormMain.Config.ItemListNotSelected.Sort();
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

        private void ButtonSelectAll_Click(object sender, EventArgs e)
        {
            CheckUnCheckAll(true);
        }

        private void ButtonClearSelections_Click(object sender, EventArgs e)
        {
            CheckUnCheckAll(false);
        }


    }
}
