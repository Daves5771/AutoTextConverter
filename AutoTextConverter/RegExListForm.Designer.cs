namespace SaelSoft.AutoTextConverter
{
    partial class RegExListForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.regExCheckedListBox = new AutoTextConverter.ColorCheckedListBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // regExCheckedListBox
            // 
            this.regExCheckedListBox.FormattingEnabled = true;
            this.regExCheckedListBox.Location = new System.Drawing.Point(12, 12);
            this.regExCheckedListBox.Name = "regExCheckedListBox";
            this.regExCheckedListBox.Size = new System.Drawing.Size(276, 229);
            this.regExCheckedListBox.TabIndex = 1;
            this.regExCheckedListBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.regExCheckedListBox_MouseDown);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(106, 257);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // RegExListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 295);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.regExCheckedListBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RegExListForm";
            this.Text = "RegExListForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RegExListForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private ColorCheckedListBox regExCheckedListBox;
        private System.Windows.Forms.Button btnClose;
    }
}