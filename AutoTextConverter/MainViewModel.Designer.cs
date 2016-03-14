namespace SaelSoft.AutoTextConverter
{
    partial class MainViewModel
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.processBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            // 
            // processBackgroundWorker
            // 
            this.processBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.processBackgroundWorker_DoWork);
            this.processBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.processBackgroundWorker_ProgressChanged);
            this.processBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.processBackgroundWorker_RunWorkerCompleted);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker processBackgroundWorker;
    }
}
