using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace SaelSoft.AutoTextConverter
{
    public partial class MainViewModel : Component
    {
        private DocumentEngine model;
        private MainForm view;

        public int[] CheckedIndices;

        // stop search variable
        private bool stopSearch = false;

        //step mode
        private static ManualResetEvent stepevent = new ManualResetEvent(true);
        private bool stepMode = false;

        //step mode
        private static ManualResetEvent pauseEvent = new ManualResetEvent(true);

        public List<SearchStruct> SearchCollection;

        public MainViewModel(MainForm theView)
        {
            InitializeComponent();
            model = new DocumentEngine();
            view = theView;
            SearchCollection = new List<SearchStruct>();
        }

        public MainViewModel(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public bool DocExists
        {
            get { return model.DocExists(); }
        }

        public bool AppExists
        {
            get { return model.AppExists(); }
        }

        public void OpenDocument(string fileName)
        {
            if (AppExists)
                model.OpenDocument(fileName);
        }

        public void CloseDocument()
        {
            if (DocExists)
                model.CloseDocument();
        }

        public void CloseDocEngine()
        {
            if (DocExists)
                model.CloseDocEngine();
        }

        public void SaveDocument()
        {
            if (DocExists)
                model.SaveDocument();
        }

        public void SaveAsDocument(string fileName)
        {
            if (DocExists)
                model.SaveAsDocument(fileName);
        }

        public bool StepMode
            { get { return stepMode; } }



        public bool HilightText
        {
            set {
                if (DocExists)
                    model.HilightText = value;
            }
        }

        public int ParagraphCount
        {
            get {if (DocExists)
                    return model.ParagraphCount;
                else return 0;
            }
        }

        public void FindTextInRange(HitInfo hitInfo)
        {
            if (DocExists)
                model.FindTextInRange(hitInfo);
        }

        public void Commit(ref HitInfo hitInfo)
        {
            if (DocExists)
                model.Commit(ref hitInfo);
        }

        public void Revert(ref HitInfo hitInfo)
        {
            if (DocExists)
                model.Revert(ref hitInfo);
        }

        public void LoadSearchEntries(string searchName)
        {
            model.LoadSearchEntries(searchName, SearchCollection);
        }

        public void GetListofChoices(ref List<string>choices)
        {
            model.GetListofChoices(ref choices);
        }

        public System.Drawing.Color ConvertSearchToSystemColor(string color)
        {
            return model.ConvertSearchToSystemColor(color);
        }

        public void CommandExecute(int cmdType)
        {
            switch(cmdType)
            {
                case Commands.Process:
                    stepMode = false;

                    // signal the pause and step events if they are reset       
                    pauseEvent.Set();
                    stepevent.Set();
                    break;

                case Commands.Step:
                    stepMode = true;
                    pauseEvent.Set();
                    stepevent.Set();
                    break;

                case Commands.PausePause:
                    pauseEvent.Reset();
                    break;

                case Commands.PauseResume:
                    pauseEvent.Set();
                    break;

                case Commands.Stop:
                    stopSearch = true;
                    stepMode = false;

                    // if we are paused or in step mode signal the event(s) to proceed
                    pauseEvent.Set();
                    stepevent.Set();
                    break;

                default:
                    throw new Exception(cmdType.ToString() + "Is an undefined command.");                    
            }
        }

        public void StartProcessing()
        {
            stepevent.Set();
            pauseEvent.Set();

            processBackgroundWorker.RunWorkerAsync();
        }
        // work thread that performs the search

        private void processBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string docText = "";
            List<HitInfo> hits;
            hits = new List<HitInfo>();
            // number of paragraphs in the document
            int paraCount = ParagraphCount;

            // decide if found items will be selected
            model.HilightText = true;

            // loop through each paragraph
            // note: Word interopt works on 1 based indexing meaning that the first
            // paragraph starts at 1 and not 0
            for (int currentParaNum = 1; currentParaNum <= paraCount; currentParaNum++)
            {
                view.Invoke(view.UpdateCurrParaNum, (object)currentParaNum.ToString());
                if (stepMode)
                    stepevent.WaitOne();

                foreach (int index in CheckedIndices)
                {
                    // convert the text in the MS Office document into a .NET string
                    docText = model.GetRng(currentParaNum).Text;

                    // do the search on the paragraph of text
                    try
                    {
                        model.RegularExpressionFind(currentParaNum, docText, SearchCollection[index], index, out hits);
                    }
                    catch (Exception ex)
                    {
          //              MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK,
          //                  MessageBoxIcon.Error);
                        break;
                    }

                    // update the GUI if we have any hits
                    foreach (HitInfo hit in hits)
                    {
                        object[] invokeParams = new object[2];
                        invokeParams[0] = currentParaNum;
                        invokeParams[1] = hit;
                        view.Invoke(view.UpdateStatusListView, invokeParams);

                        // if we are stepping then pause the thread at the beginning of the loop
                        if (stepMode)
                            stepevent.Reset();
                    }

                    // stop searhing if user pressed stop
                    if (stopSearch)
                        return;

                    // if the user pressed pause wait hear until resume is pressed
                    pauseEvent.WaitOne();
                }
            }
        }

        private void processBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            view.ProcessingCompleted(stopSearch);
            stopSearch = false;
        }

        private void processBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }
    }
}
