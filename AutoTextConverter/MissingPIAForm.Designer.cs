namespace SaelSoft.AutoTextConverter
{
    partial class MissingPIAForm
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
            this.office2003PIAlinkLabel = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.officeXPLinkLabel = new System.Windows.Forms.LinkLabel();
            this.office2007PIAlinkLabel = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // office2003PIAlinkLabel
            // 
            this.office2003PIAlinkLabel.AutoSize = true;
            this.office2003PIAlinkLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.office2003PIAlinkLabel.Location = new System.Drawing.Point(31, 166);
            this.office2003PIAlinkLabel.Name = "office2003PIAlinkLabel";
            this.office2003PIAlinkLabel.Size = new System.Drawing.Size(125, 13);
            this.office2003PIAlinkLabel.TabIndex = 0;
            this.office2003PIAlinkLabel.TabStop = true;
            this.office2003PIAlinkLabel.Text = "MS Office 2003 PIAs";
            this.office2003PIAlinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.office2003PIAlinkLabel_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(31, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(352, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "It appears that your computer does not have the MS Office PIA\'s installed";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(224, 209);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "Close";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(430, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "MS Office PIAs  are needed in order for this .NET program to communicate with MS " +
                "Office";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(321, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Click one of the following links to install the PIAs on your computer.";
            // 
            // officeXPLinkLabel
            // 
            this.officeXPLinkLabel.AutoSize = true;
            this.officeXPLinkLabel.Location = new System.Drawing.Point(221, 166);
            this.officeXPLinkLabel.Name = "officeXPLinkLabel";
            this.officeXPLinkLabel.Size = new System.Drawing.Size(96, 13);
            this.officeXPLinkLabel.TabIndex = 5;
            this.officeXPLinkLabel.TabStop = true;
            this.officeXPLinkLabel.Text = "MS Office XP PIAs";
            this.officeXPLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.officeXPLinkLabel_LinkClicked);
            // 
            // office2007PIAlinkLabel
            // 
            this.office2007PIAlinkLabel.AutoSize = true;
            this.office2007PIAlinkLabel.Location = new System.Drawing.Point(403, 166);
            this.office2007PIAlinkLabel.Name = "office2007PIAlinkLabel";
            this.office2007PIAlinkLabel.Size = new System.Drawing.Size(106, 13);
            this.office2007PIAlinkLabel.TabIndex = 6;
            this.office2007PIAlinkLabel.TabStop = true;
            this.office2007PIAlinkLabel.Text = "MS Office 2007 PIAs";
            this.office2007PIAlinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.office2007PIAlinkLabel_LinkClicked);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(31, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(461, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "The MS Office 2003 PIAs are recommended since this program was tested and linked " +
                "with them.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(31, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(308, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "You may need to rebuild this program with the other assemblies. ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(31, 143);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(326, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Those links are provided for you to experiment with at your own risk.";
            // 
            // MissingPIAForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(523, 244);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.office2007PIAlinkLabel);
            this.Controls.Add(this.officeXPLinkLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.office2003PIAlinkLabel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MissingPIAForm";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "MS Office Primary Interop Assemblies Not Detected";
            this.Load += new System.EventHandler(this.MissingPIAForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel office2003PIAlinkLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel officeXPLinkLabel;
        private System.Windows.Forms.LinkLabel office2007PIAlinkLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}