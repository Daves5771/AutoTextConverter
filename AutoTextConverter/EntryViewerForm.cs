using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MSOfficeRegExDemo
{
    public partial class EntryViewerForm : Form
    {
      //  public bool Visible;
        public string Description
        {
            get { return descLbl.Text; }
            set {
                descLbl.Text = value;
                 }
        }
 
        public EntryViewerForm()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Visible = false;
        }

        private void EntryViewerForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
