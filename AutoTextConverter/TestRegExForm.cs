using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SaelSoft.AutoTextConverter
{
    public partial class TestRegExForm : Form
    {
        public TestRegExForm()
        {
            InitializeComponent();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            System.Text.RegularExpressions.Regex r;
            MatchCollection matches;

            try
            {
                r = new Regex(regExtextBox.Text);

                matches = r.Matches(testStringTestBox.Text);

                // no matches go on to next paragraph
                if (matches.Count == 0)
                {
                    resultTextBox.Text = "No matches found";
                    resultTextBox.ForeColor = Color.Red;
                }
                else if (matches[0].Value == "")
                {
                    resultTextBox.Text = "null string";
                    resultTextBox.ForeColor = Color.Red;
                }
                else
                {
                    int i = 0;
                    StringBuilder matchString = new StringBuilder();
                    foreach (Match matchItem in matches)
                        matchString.Append("match " + (i++).ToString() + ": " + matchItem.Value + " ");
                    resultTextBox.Text = matchString.ToString();
                    resultTextBox.ForeColor = Color.Green;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
