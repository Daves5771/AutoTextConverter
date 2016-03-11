using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaelSoft.AutoTextConverter
{
    public partial class RegExListForm : Form
    {
        public RegExListForm()
        {
            InitializeComponent();
        }

        public void SetXmlName(string name)
        {
            this.Text = name;
        }

        public void Clear()
        {
            regExCheckedListBox.Items.Clear();
        }

        public ColorCheckedListBox XmlCheckedListBox
        {
            get
            {
                return regExCheckedListBox;
            }
        }

        public int CheckedIndicesCount
        {
            get
            {
                return regExCheckedListBox.CheckedIndices.Count;
            }
        }

        public void Add(SearchStruct searchInfo, bool state, Color color)
        {
            regExCheckedListBox.Add(searchInfo, state, color);
        }

        public void Add(string desc, bool state, Color color)
        {
            regExCheckedListBox.Add(desc, state, color);
        }

        private void RegExListForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true; // this cancels the close event.
        }

        // toggles the checks in the regex check list box
        private void allOrNoneCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < regExCheckedListBox.Items.Count; i++)
            {
 ///               regExCheckedListBox.SetItemChecked(i, allOrNoneCheckBox.Checked);
            }
        }


        private void regExCheckedListBox_MouseDown(object sender, MouseEventArgs e)
        {
            CheckedListBox i = (CheckedListBox)sender;
            if (i.SelectedItem == null)
                return;
            if (e.Button == MouseButtons.Right)
            {
                Point p = new Point(e.X, e.Y);
                regExCheckedListBox.SelectedIndex = regExCheckedListBox.IndexFromPoint(p);
                ItemState item = (ItemState)regExCheckedListBox.SelectedItem;
/*daves todo
                if (!entryViewer.Visible)
                    entryViewer.Show();
                entryViewer.Description = item.SearchInfo.Description;
*/
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
