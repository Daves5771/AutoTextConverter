using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.IO;
using System.Windows.Forms;

namespace SaelSoft.AutoTextConverter
{
    public delegate void DelegateOpenDocEvent(string docName);
    // supported export formats
    public enum ExportFormats { efText, efExcel }
    public enum Mode {AutoProcess, StepProcess, FileLoaded, NoFile }

    public partial class MainForm : Form
    {
        private MainViewModel viewModel;
        ColorCheckedListBox currentListBox;

        const string AppTitle = "Auto Text Converter";

        // search mode
        //  SearchMode theSearchMode = SearchMode.FindOnly;

        // GUI update delegates
        public delegate void DelegateUpdateGUI(int paraNum, HitInfo foundString);
        public DelegateUpdateGUI UpdateStatusListView;

        public delegate void DelegateUpdateCurrParaNum(string paraNum);
        public DelegateUpdateCurrParaNum UpdateCurrParaNum;

        // for future use
        public DelegateOpenDocEvent OpenDocumentEvent;

        // Primary Interop Assemblies Installed variable
        private bool piasInstalled = true;

        // sets default path for loading test documents (sample docs dir)
        private string documentPath = "";

        // processing
        private bool isProcessing = false;

        RegExListForm relf = new RegExListForm();

        int[] checkedIndecieArray;

        void EnableControls(bool enable)
        {
            btnFind.Enabled = enable;
            btnStep.Enabled = enable;
            btnPause.Enabled = enable;
        }

        public MainForm()
        {
            InitializeComponent();
            try
            {
                // initialize the view model
                viewModel = new MainViewModel(this);
            }
            catch
            {
                // changes are the pia for word is not installed
                piasInstalled = false;
            }

            // load choices into list box
            List<string> choiceNames = new List<string>();
            viewModel.GetListofChoices(ref choiceNames);
            foreach (string c in choiceNames)
                conversionsComboBox.Items.Add(c);

            LoadRegExListBox();
            LoadRegExReplaceListBox();

            this.Text = AppTitle;
            documentPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\Sample Docs\";

            // initialize delegates
            UpdateStatusListView = new DelegateUpdateGUI(this.UpdateGUI);
            UpdateCurrParaNum = new DelegateUpdateCurrParaNum(this.UpdateCurrParaTextBox);
            OpenDocumentEvent = new DelegateOpenDocEvent(this.UpdateTitle);
        }

        // update list view
        private void UpdateGUI(int currentParaNum, HitInfo hit)
        {
            SetStatusMessage(currentParaNum, hit, hit.ReplaceText);
        }

        private void UpdateCurrParaTextBox(string currParaNum)
        {
            toolStripStatusLabel1.Text = "Processing paragraph: " + currParaNum;
        }

        public void UpdateTitle(string docName)
        {
            this.Text = AppTitle + " - " + docName;
        }

        // RegEx list box with all search optons from XML
        public void LoadRegExListBox()
        {
            foreach (SearchStruct searchItem in viewModel.SearchCollection)
            {
                relf.Add(searchItem, true,
                    viewModel.ConvertSearchToSystemColor(searchItem.TextColor));
            }
        }

        public void LoadRegExReplaceListBox()
        {
            /// daves
 //           MessageBox.Show("should not get here");
            /*
            foreach (SearchStruct searchItem in SearchandReplaceCollection)
            {
                regExCheckedListBoxReplace.Add(searchItem.Description,
                    docEngine.ConvertSearchToSystemColor(searchItem.TextColor));
            } */
        }

        // open document directly from program's open 
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Word Document (*.docx *.doc)|*.docx;*doc|All Web Pages (*.htm *.html *.mht *mhtml)|*.htm;*.html; *.mht; *mhtml|Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = false;
            openFileDialog1.InitialDirectory = documentPath;
            openFileDialog1.FileName = "";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((openFileDialog1.FileName) != null)
                {
                    OpenFileCommon(openFileDialog1.FileName);
                }
            }
        }

        // closes the document
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                viewModel.CloseDocument();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            this.Text = AppTitle;
            NoFileLoaded();
        }

        // saves the document
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            viewModel.SaveDocument();
        }

        // presents a save document dialog before saving document
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Word Document (*.docx)|*.docx|Word 97-2003 Document (*.doc)|*.doc|Rich Text Format (*.rtf*)|*.rtf";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                viewModel.SaveAsDocument(saveFileDialog1.FileName);
            }
        }

        // updates the status list view 
        public void SetStatusMessage(int location, HitInfo hit, String replacedTxt)
        {
            int nImageIndex = 0;
            ListViewItem item = statusListView.Items.Add("", nImageIndex);
            item.ToolTipText = "This is a test of " + replacedTxt + " ttt";
            statusListView.ShowItemToolTips = true;

            item.SubItems.Add("");
            item.SubItems.Add("");
            item.SubItems.Add("");
            statusListView.Items[statusListView.Items.Count - 1].SubItems[0].Text = location.ToString();
            statusListView.Items[statusListView.Items.Count - 1].SubItems[0].ForeColor = hit.TextColor;
            statusListView.Items[statusListView.Items.Count - 1].SubItems[1].Text = hit.Text;
            if (hit.searchMode == SearchMode.FindOnly)
                replacedTxt = "N/A";
            statusListView.Items[statusListView.Items.Count - 1].SubItems[2].Text = replacedTxt;
            statusListView.Items[statusListView.Items.Count - 1].Tag = hit;
            if (hit.searchMode == SearchMode.FindOnly)
                statusListView.Items[statusListView.Items.Count - 1].SubItems[3].Text = "Searched";
            else
                statusListView.Items[statusListView.Items.Count - 1].SubItems[3].Text = "Replaced";

            statusListView.EnsureVisible(statusListView.Items.Count - 1);

            if (viewModel.StepMode)
                btnStep.Enabled = true;
        }

        // perform a search
        private void btnFind_Click(object sender, EventArgs e)
        {
            // cant do step mode while auto processing
            btnFind.Enabled = false;
            btnStep.Enabled = false;
            btnPause.Enabled = true;

            viewModel.CommandExecute(Commands.Process);
 
            if (!isProcessing)
                StartProcessing(); 
        }

        private void StartProcessing()
        {
            isProcessing = true;

            if (relf.CheckedIndicesCount == 0)
            {
                MessageBox.Show("You must select a Regular Expression to search for", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnStep.Enabled = false;
            btnFind.Enabled = false;
            btnStop.Enabled = true;
            btnPause.Enabled = true;

 ///           stepevent.Set();
 ///           pauseEvent.Set();
            toolStripStatusLabel1.Text = "Search started";
            toolStripNumParLabel.Text = "Num of Paragraphs: " + viewModel.ParagraphCount.ToString();

            checkedIndecieArray = new int[currentListBox.CheckedIndices.Count];
            foreach (int index in currentListBox.CheckedIndices)
                checkedIndecieArray[index] = currentListBox.CheckedIndices[index];

            viewModel.CheckedIndices = checkedIndecieArray;

            viewModel.StartProcessing();
        }

        // clear the list view
        private void btnClear_Click(object sender, EventArgs e)
        {
            statusListView.Items.Clear();
        }

        // goodbye
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 about = new AboutBox1();
            about.ShowDialog();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            viewModel.CommandExecute(Commands.Stop);
        }

        private void btnStep_Click(object sender, EventArgs e)
        {
            // signal that we can continue
            btnStep.Enabled = false;
            btnFind.Enabled = false;
            viewModel.CommandExecute(Commands.Step);

            if (!isProcessing)
                StartProcessing(); 
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (btnPause.Text == "Pause")
            {
                btnPause.Text = "Resume";
                // pause processing
                btnFind.Enabled = true;
                btnStep.Enabled = true;
                viewModel.CommandExecute(Commands.PausePause);
            }

            else if (btnPause.Text == "Resume")
            {
                btnPause.Text = "Pause";
                // continue processing
                viewModel.CommandExecute(Commands.PausePause);
            }
        }

        public void ProcessingCompleted(bool stopSearch)
        {
            btnFind.Enabled = true;
            btnStop.Enabled = false;
            btnPause.Enabled = false;
            btnStep.Enabled = true;

            if (stopSearch)
                toolStripStatusLabel1.Text = "Search terminated by user";
            else
                toolStripStatusLabel1.Text = "Search completed";
            isProcessing = false;
        }

        private void statusListView_DoubleClick(object sender, EventArgs e)
        {
            if (statusListView.SelectedItems.Count < 1)
                return;
            ListViewItem item = statusListView.SelectedItems[0];
            HitInfo hitInfo = (HitInfo)item.Tag;
            viewModel.FindTextInRange(hitInfo);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (piasInstalled)
                viewModel.CloseDocEngine();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            // display message of how to install Primary Interop Assemblies
            // if they are not installed on this computer
            if (!piasInstalled)
            {
                MissingPIAForm form = new MissingPIAForm(false);
                form.ShowDialog();
            }

            if (conversionsComboBox.Items.Count > 0)
                conversionsComboBox.SelectedIndex = 0;

            Preferences.ReadPreferences();

        }

        private void installMSOfficePIAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MissingPIAForm form = new MissingPIAForm(true);
            form.ShowDialog();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // default is to show search list box
            /// daves           regExCheckedListBoxReplace.Visible = false;
            NoFileLoaded();
            currentListBox = relf.XmlCheckedListBox;
        }

        private void NoFileLoaded()
        {
            btnFind.Enabled = false;
            btnPause.Enabled = false;
            btnStop.Enabled = false;
            btnStep.Enabled = false;
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] dropfiles = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (dropfiles.Length > 1)
            {
                MessageBox.Show("Only one file can be dragged to this program",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (dropfiles.Length == 0)
                return;

            string fileExt = Path.GetExtension(dropfiles[0]).ToLower();
            if (fileExt != ".doc" && fileExt != ".docx" && fileExt != ".rtf" && fileExt != ".mht")
            {
                MessageBox.Show("This version of the software does not support "
                    + fileExt + " files.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            OpenFileCommon(dropfiles[0]);
        }

        private void OpenFileCommon(string fileName)
        {
            try
            {
                viewModel.OpenDocument(fileName);
                this.Text = AppTitle + " - " + Path.GetFileName(fileName);
                btnFind.Enabled = true;
                btnPause.Enabled = false;
                btnStep.Enabled = true;
                toolStripStatusLabel1.Text = "Ready";
                toolStripNumParLabel.Text = "Num of Paragraphs: " + viewModel.ParagraphCount.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " Error opening document");
            }
        }

        private void searchAndReToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*  if (theSearchMode == SearchMode.FindOnly)
              {
                  regExCheckedListBoxReplace.Visible = true;
                  regExCheckedListBox.Visible = false;
                  currentListBox = regExCheckedListBoxReplace;
                  currentSearchCollection = SearchandReplaceCollection;
                  theSearchMode = SearchMode.FindAndReplace;
                  toolStripSearchLabel.Text = "Search and Replace";
                  searchAndReToolStripMenuItem.Text = "Search";
              }
              else
              {
                  regExCheckedListBoxReplace.Visible = false;
                  regExCheckedListBox.Visible = true;
                  currentListBox = regExCheckedListBox;
                  currentSearchCollection = SearchCollection;
                  theSearchMode = SearchMode.FindOnly;
                  toolStripSearchLabel.Text = "Search";
                  searchAndReToolStripMenuItem.Text = "Search and Replace";
             }
              // daves fix later
              theSearchMode = SearchMode.FindAndReplace;*/
        }

        private void commitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem item = statusListView.SelectedItems[0];
            int selItemIndex = statusListView.SelectedIndices[0];
            HitInfo hitInfo = (HitInfo)item.Tag;
            int delta = hitInfo.Text.Length - hitInfo.ReplaceText.Length;
            viewModel.Commit(ref hitInfo);
            statusListView.Items[selItemIndex].SubItems[0].ForeColor = Color.Black;
            statusListView.Items[selItemIndex].SubItems[3].Text = "Cmmitted";
        }

        private void revertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem item = statusListView.SelectedItems[0];
            int selItemIndex = statusListView.SelectedIndices[0];
            HitInfo hitInfo = (HitInfo)item.Tag;
            int delta = hitInfo.Text.Length - hitInfo.ReplaceText.Length;
            viewModel.Revert(ref hitInfo);
            statusListView.Items[selItemIndex].SubItems[0].ForeColor = Color.Magenta;
            statusListView.Items[selItemIndex].SubItems[2].Text = hitInfo.Text;
            statusListView.Items[selItemIndex].SubItems[3].Text = "Reverted";
        }

        private void searchComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (conversionsComboBox.SelectedIndex == -1)
                return;
            viewModel.LoadSearchEntries((string)conversionsComboBox.Items[conversionsComboBox.SelectedIndex]);
            relf.Clear();
            foreach (SearchStruct searchItem in viewModel.SearchCollection)
            {
                relf.Add(searchItem.Description, true,
                    viewModel.ConvertSearchToSystemColor(searchItem.TextColor));
            }

            relf.SetXmlName((string)conversionsComboBox.Items[conversionsComboBox.SelectedIndex]);
        }

        private void regExTesterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TestRegExForm testRegExForm = new TestRegExForm();
            testRegExForm.ShowDialog();
        }

        private void colorGuideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SupportedColorsForm supportedColorsForm = new SupportedColorsForm();
            supportedColorsForm.ShowDialog();
        }

        private void commitAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in statusListView.Items)
            {
                HitInfo hitInfo = (HitInfo)item.Tag;
                int delta = hitInfo.Text.Length - hitInfo.ReplaceText.Length;
                viewModel.Commit(ref hitInfo);
                item.SubItems[0].ForeColor = Color.Black;
                item.SubItems[3].Text = "Cmmitted";
            }
        }

        private void resultsContextMenu_Opening(object sender, CancelEventArgs e)
        {
            if (statusListView.SelectedItems.Count == 0)
                return;

            ListViewItem item = statusListView.SelectedItems[0];
            int selItemIndex = statusListView.SelectedIndices[0];
            HitInfo hitInfo = (HitInfo)item.Tag;
            if (hitInfo.committed || hitInfo.searchMode == SearchMode.FindOnly)
                foreach (ToolStripItem menuItem in resultsContextMenu.Items)
                    menuItem.Enabled = false;
        }

        private void searchListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            relf.Show();
        }

        private void ExportFile(ExportFormats exportFormat)
        {
            StringBuilder fileLine = new StringBuilder();
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            if (exportFormat == ExportFormats.efExcel)
                saveFileDialog1.Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*";
            else
                saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            // Show the dialog and get result.
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result != DialogResult.OK) // Test result. 
                return;

            if (exportFormat == ExportFormats.efExcel)
            {
    //            CreateCsvFile(saveFileDialog1.FileName);
        //        return;

            }

            using (StreamWriter outfile =
            new StreamWriter(saveFileDialog1.FileName))
            {
                for (int h = 0; h < statusListView.Columns.Count; h++)
                {
                    fileLine.Append(statusListView.Columns[h].Text);
                    if (exportFormat == ExportFormats.efExcel)
                        fileLine.Append(",");
                    else
                        fileLine.Append("\t");

                }
                outfile.WriteLine(fileLine.ToString());

                for (int i = 0; i < statusListView.Items.Count; i++)
                {
                    ListViewItem lItem = new ListViewItem();
                    fileLine.Length = 0;
                    lItem = statusListView.Items[i];
                    for (int j = 0; j < statusListView.Columns.Count; j++)
                    {
                        fileLine.Append(lItem.SubItems[j].Text);
                        fileLine.Append("\t");
                    }
                    //  fileLine.Append(lItem.SubItems[6].Text);
                    //  fileLine.Append("\n");
                    outfile.WriteLine(fileLine.ToString()); 
                }
            }
        }

        private void searchesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportFile(ExportFormats.efText);
        }

        private void searcAndReplacementsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportFile(ExportFormats.efExcel);
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Preferences pref = new Preferences();
            pref.ShowDialog();
        }
    }
}
