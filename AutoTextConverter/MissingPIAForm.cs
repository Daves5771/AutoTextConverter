using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
// SaelSoft -- MissingPIAForm.cs
// Purpose -- Provides help to install Primary Interop Assembles for Office
// 2008 David Saelman

namespace SaelSoft.AutoTextConverter
{
    public partial class MissingPIAForm : Form
    {
        bool isFromHelpMenu = false;
        string helpLabel = "MS Office Primary Interop Assemblies Installation Help";

        public MissingPIAForm(bool fromHelpMenu)
        {
            isFromHelpMenu = fromHelpMenu;
            InitializeComponent();
        }

        private void office2003PIAlinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("IExplore", "http://support.microsoft.com/kb/897646");
        }

        private void office2007PIAlinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("IExplore", "http://www.microsoft.com/downloads/details.aspx?FamilyID=59daebaa-bed4-4282-a28c-b864d8bfa513&displaylang=en");
        }
        
        private void officeXPLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("IExplore", "http://support.microsoft.com/kb/328912");      
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MissingPIAForm_Load(object sender, EventArgs e)
        {
            if (isFromHelpMenu)
            {
                Text = "Installing the MS Office PIAs";
                label1.Text = helpLabel;
            }
        }

    }
}