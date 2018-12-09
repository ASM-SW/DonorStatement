// Copyright © 2018  ASM-SW
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
    public partial class FormItemIgnore : Form
    {
        public FormItemIgnore()
        {
            InitializeComponent();
        }

        private void FormIgnoreItem_VisibleChanged(object sender, EventArgs e)
        {
            if (((System.Windows.Forms.Control)sender).Visible)
                LoadListBox();
            else
                SaveSelected();
        }

        // set ItemListIgnore to contain items selected
        private void SaveSelected()
        {
            FormMain.Config.ItemListIgnore.Clear();
            foreach (string item in listItems.SelectedItems)
                FormMain.Config.ItemListIgnore.Add(item);
        }

        // update listbox so that it contains items not selected in previous form and select items from the ItemListIgnore collection
        private void LoadListBox()
        {
            listItems.Items.Clear();

            FormMain.Config.ItemListIgnore.Sort();
            foreach (string item in FormMain.Config.ItemListNotSelected)
            {
                listItems.Items.Add(item);
                if (FormMain.Config.ItemListIgnore.BinarySearch(item) >= 0)
                    listItems.SelectedItems.Add(item);
            }
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
