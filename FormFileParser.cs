// Copyright © 2016-2026 ASM-SW
// asm-sw@outlook.com  https://github.com/asm-sw
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DonorStatement
{
    internal sealed partial class FormFileParser : Form, ISubForm
    {
        readonly FileParser m_parser;
        readonly LogMessageDelegate m_logger;

        public FormFileParser(FileParser parser, LogMessageDelegate logger)
        {
            m_parser = parser;
            m_logger = logger;
            InitializeComponent();

            ReloadAllLists();
            SetTextFileHasBeenRead();
        }

        public bool CanExit(out string errorMsg)
        {
            errorMsg = string.Empty;
            if (!m_parser.FileHasBeenRead)
            {
                errorMsg = "Please parse the input file before proceeding to the next step.";
                return false;
            }
            return true;
        }

        private void SetTextFileHasBeenRead()
        {
            textFileHasBeenRead.Text = m_parser.FileHasBeenRead
                ? "Input File has been read in."
                : "Input File has not been read. Click on Parse to read it and update the list of items.";
        }

        /// <summary>
        /// Parses the input file.  If an item in one of the list boxes isn't in the 
        /// input file it will be removed from the list. New items will be added 
        /// to the list in the Donations list box.
        /// </summary>
        private void ButtonParse_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                if (!m_parser.ParseInputFile())
                    return;

                m_parser.GetItemList(out List<string> itemListFromFile);
                HashSet<string> fileItemSet = new(itemListFromFile, StringComparer.CurrentCultureIgnoreCase);

                // Prune items no longer present in input file
                PruneRemovedItems(FormMain.Config.ListDonations, fileItemSet, "Donations");
                PruneRemovedItems(FormMain.Config.ListOther, fileItemSet, "Other");
                PruneRemovedItems(FormMain.Config.ListIgnore, fileItemSet, "Ignore");

                // Add new items into ListDonations by default
                HashSet<string> otherSet = new(FormMain.Config.ListOther, StringComparer.CurrentCultureIgnoreCase);
                HashSet<string> ignoreSet = new(FormMain.Config.ListIgnore, StringComparer.CurrentCultureIgnoreCase);
                HashSet<string> donationSet = new(FormMain.Config.ListDonations, StringComparer.CurrentCultureIgnoreCase);

                foreach (string item in itemListFromFile)
                {
                    if (otherSet.Contains(item) || ignoreSet.Contains(item) || donationSet.Contains(item))
                        continue;

                    FormMain.Config.ListDonations.Add(item);
                    donationSet.Add(item);
                }

                // Crucial: Must re-sort for DocumentCreator's BinarySearch
                FormMain.Config.ListDonations.Sort();
                FormMain.Config.ListOther.Sort();
                FormMain.Config.ListIgnore.Sort();

                ReloadAllLists();

                if (listDonations.Items.Count > 0)
                    listDonations.TopIndex = 0;

                SetTextFileHasBeenRead();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Remove items from a list that are not present in the input file.
        /// </summary>
        /// <param name="list">The list to remove items from.</param>
        /// <param name="validItems">A hash set of items to keep.</param>
        /// <param name="listName">The name of the list to use in the log message.</param>
        private void PruneRemovedItems(List<string> list, HashSet<string> validItems, string listName)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                string item = list[i];
                if (!validItems.Contains(item))
                {
                    list.RemoveAt(i);
                    m_logger($"Item '{item}' was removed from {listName} because it is no longer in the input file.");
                }
            }
        }

        private void FileParserForm_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                SetTextFileHasBeenRead();
            }
        }

        private void butSelectToNotSelect_Click(object sender, EventArgs e) =>
            MoveSelectedItems(listDonations, FormMain.Config.ListDonations, FormMain.Config.ListOther, LoadDonations, LoadOther);

        private void buttonNotSelectedToSelected_Click(object sender, EventArgs e) =>
            MoveSelectedItems(listNotDonations, FormMain.Config.ListOther, FormMain.Config.ListDonations, LoadOther, LoadDonations);

        private void buttonNotSelectedToIgnore_Click(object sender, EventArgs e) =>
            MoveSelectedItems(listNotDonations, FormMain.Config.ListOther, FormMain.Config.ListIgnore, LoadOther, LoadIgnore);

        private void buttonIgnoreToNotSelected_Click(object sender, EventArgs e) =>
            MoveSelectedItems(listIgnore, FormMain.Config.ListIgnore, FormMain.Config.ListOther, LoadIgnore, LoadOther);

        private static void MoveSelectedItems(ListBox sourceBox, List<string> sourceList, List<string> targetList, Action reloadSource, Action reloadTarget)
        {
            if (sourceBox.SelectedItems.Count == 0)
                return;

            foreach (var item in sourceBox.SelectedItems)
            {
                string text = sourceBox.GetItemText(item);
                if (!targetList.Contains(text))
                    targetList.Add(text);
                sourceList.Remove(text);
            }

            sourceList.Sort();
            targetList.Sort();

            reloadSource();
            reloadTarget();
        }

        private void ReloadAllLists()
        {
            LoadDonations();
            LoadOther();
            LoadIgnore();
        }

        private void LoadDonations() => PopulateListBox(listDonations, FormMain.Config.ListDonations);
        private void LoadOther() => PopulateListBox(listNotDonations, FormMain.Config.ListOther);
        private void LoadIgnore() => PopulateListBox(listIgnore, FormMain.Config.ListIgnore);

        private static void PopulateListBox(ListBox listBox, List<string> items)
        {
            listBox.BeginUpdate();
            try
            {
                listBox.Items.Clear();
                listBox.Items.AddRange(items.ToArray());
            }
            finally
            {
                listBox.EndUpdate();
            }
        }
    }
}
