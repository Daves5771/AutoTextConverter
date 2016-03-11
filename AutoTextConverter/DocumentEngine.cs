using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using Word = Microsoft.Office.Interop.Word;


// SaelSoft -- DocumentEngine.cs
// Purpose -- Performs MS Office Interopt and RegEx Operations
// Copyright 2008-2016 David Saelman

namespace SaelSoft.AutoTextConverter
{
    public enum OperationMode { noMode, DotNetRegExMode, Word97Mode };
    public enum SearchMode { FindOnly, FindAndReplace };
    public struct SearchStruct
    {
        public string RegExpression;
        public string Identifier;
        public string Description;
        public string TextColor;
        public string Action;
        public string PlugInName;
        public string PlugInValidationFunction;
        public string PlugInFormatFunction;
    }
 
    public struct HitInfo
    {
        public string Text;
        public int StartDocPosition; // position in MS document where Text resides
        public string ReplaceText;
        public System.Drawing.Color TextColor;
        public Word.WdColor OriginalTextColor;
        public SearchMode searchMode;
        public bool reverted;
        public bool committed;
    }

    public class DocumentEngine
    {
   //     var WordApp = new ApplicationClass();
   //     WordApp.Visible = true;
  //      string fileName = @"NewTest.doc";
   //     Document aDoc = WordApp.Documents.Open(fileName, ReadOnly: true, Visible: true);
  //      aDoc.Activate();

        Word.Application app;
        Word.Document theDoc;
        int paraCount = 0;
        bool hasValidationAssembly = false;
        bool hilightText = true;
        private XmlModule xmlMod;

        Regex[] regExList;

        public bool HilightText
        {
            get { return hilightText; }
            set
            {
                hilightText = value;
            }
        }
        public string DocName
        {
            get{ return theDoc.Name;}
        }

        public void LoadSearchEntries(string searchName, List<SearchStruct> searches)
        {
            xmlMod.LoadSearchEntries(searchName, searches);
            regExList = new Regex[searches.Count];

            for (int i = 0; i< searches.Count; i++)
            {
                regExList[i] = new Regex(searches[i].RegExpression);
            }
        }

        public void GetListofChoices(ref List<string> names)
        {

            xmlMod.GetListOfChoices(ref names);
        }

        // dynamic assembly variables
        // Get the method to call.
        MethodInfo validationMethod;
        MethodInfo formattedMatchMethod;

        // Create an instance.
        Object assemblyInstance;

        // returns the number of paragraphs in the document
        public int ParagraphCount
        {
            get { return theDoc.Paragraphs.Count; }
        }

        public DocumentEngine()
        {
            app = new Word.Application();
            xmlMod = new XmlModule();

#if !DOTNET4
            // the following line will not compile if the Microsoft
            // Primary Interrupt Assemblies (PIAs) for Office 2003 are not installed
            // They can be downloaded for free from http://support.microsoft.com/kb/897646
#endif
            // we need to perform this cast to avoid an ambiguous call with the 
            // Office.Interop.Word._Application QUIT
            ((Word.ApplicationEvents4_Event)app).Quit += new Microsoft.Office.Interop.Word.ApplicationEvents4_QuitEventHandler(App_Quit);
        }

        // notifcation that application was quit by user
        private void App_Quit()
        {
            app = null;
            theDoc = null;
        }

         // this function needs to be called before the application shuts down
        // it will deallocate Word app object if it is not null
        public void CloseDocEngine()
        {
             if (app == null)
                 return;

             if (theDoc != null)
                 CloseDocument();

            object noPrompt = true;
            object routeDocument = false;
            object originalFormat = Word.WdOriginalFormat.wdPromptUser;

            // we need to perform this cast to avoid an ambiguous call with the 
            // ApplicationEvents4_Event QUIT
            ((Microsoft.Office.Interop.Word._Application)app).Quit(ref noPrompt, ref originalFormat, ref routeDocument);
        }
#if DOTNET4
        // Opens a MS WORD or RTF document
        public void OpenDocument(string documentName)
        {
             if (app == null)
                app = new Word.Application();

            app.Visible = true;

            try
            {
                // have Word open the document
                theDoc = app.Documents.Open(documentName, Visible: true);

                paraCount = theDoc.Paragraphs.Count;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + ": Error opening document");
            }
        }

        // save document
        public void SaveDocument()
        {
            app.Documents.Save(NoPrompt: true, OriginalFormat: Word.WdOriginalFormat.wdPromptUser);
        }

        // saves document with name specified in documentName
        public void SaveAsDocument(string documentName)
        {
            try
            {
                Word.WdSaveFormat originalFormat;
                string ext = Path.GetExtension(documentName).ToLower();
                if (ext == ".docx")
                    originalFormat = Word.WdSaveFormat.wdFormatDocument;
                else if (ext == ".doc")
                    originalFormat = Word.WdSaveFormat.wdFormatDocument;
                else if (ext == ".rtf")
                    originalFormat = Word.WdSaveFormat.wdFormatRTF;
                else
                {
                    MessageBox.Show("Invalid file format");
                    return;
                }

                theDoc.SaveAs(FileName: documentName, FileFormat: originalFormat);
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                MessageBox.Show(ex.Message + "\nDocument: " + theDoc.Name, "MS Word Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // closes the document
        public bool CloseDocument()
        {
            bool closeStatus = false;
            try
            {
                Word.WdSaveOptions saveChanges = Word.WdSaveOptions.wdSaveChanges;

                // the following code is a work around
                DialogResult dr = MessageBox.Show("Do you want to save the changes to: " + theDoc.Name, "Microsoft Office Word", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                if (dr == DialogResult.No)
                {
                    saveChanges = Word.WdSaveOptions.wdDoNotSaveChanges;
                    closeStatus = true;
                }

                else if (dr == DialogResult.Cancel)
                    return closeStatus;

                app.Documents.Close(saveChanges, OriginalFormat: saveChanges, RouteDocument: true);
                theDoc = null;
                closeStatus = true;
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                MessageBox.Show(ex.Message + "\nDocument: " + theDoc.Name, "MS Word Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return closeStatus;
        }

        // find the regular expression match in the MS WORD Doc.
        internal bool FindTextInDoc(OperationMode opMode, int currentParaNum, string textToFind, Word.WdColor color, int start, out int end, out int textStartPoint, out Word.WdColor originalTextColor, string replaceStr, SearchMode theSearchMode)
        {
            string strFind = textToFind;
            textStartPoint = 0;
            originalTextColor = Word.WdColor.wdColorAutomatic;


            // get the range of the curent paragraph
            Word.Range rngDoc = GetRng(currentParaNum);

            // make sure we are not past the end of the range
            if (start >= rngDoc.End)
            {
                end = 0;
                return false;
            }
            rngDoc.Start = start;

            // setup Microsoft Word Find based upon
            // Regular Expression Match
            rngDoc.Find.ClearFormatting();
            rngDoc.Find.Forward = true;
            rngDoc.Find.Text = textToFind;

            // find the text in the word document
            rngDoc.Find.Execute(MatchCase: true);

            // select text if true
            if (hilightText)
                rngDoc.Select();

            // if we are in replace mode, replace the found text with its
            // formatted version
            if (hasValidationAssembly && theSearchMode == SearchMode.FindAndReplace)
                rngDoc.Text = replaceStr;

            // set new end point to the character position afer
            // the found test
            end = rngDoc.End + 1;

            // get segment string
            //           string txtSegStr = "";
            //            GetTextSegmentNeighborhood(rngDoc, out txtSegStr);

            // character position where the matched string begins
            textStartPoint = rngDoc.Start;
            textStartPoint = rngDoc.Start; 
            // we found the text
            if (rngDoc.Find.Found)
            {
                // save the original color
                originalTextColor = rngDoc.Font.Color;

                rngDoc.Font.Color = color;
                // the range endpoint will change if we modified the text
                return true;
            }
            return false;
        }

        // selects text within range in the document determined by
        // hitInfo.StartDocPosition and hitInfo.StartDocPosition + hitInfo.Text.Length
        public void FindTextInRange(HitInfo hitInfo)
        {
            int segmentLength;
            if (hitInfo.searchMode == SearchMode.FindOnly)
                segmentLength = hitInfo.Text.Length;
            else
                segmentLength = hitInfo.ReplaceText.Length;

            Word.Range rngDoc = theDoc.Range(Start: hitInfo.StartDocPosition, End: hitInfo.StartDocPosition + segmentLength);

            rngDoc.Select();
            app.Activate();
        }

        public void Commit(ref HitInfo hitInfo)
        {
            int segmentLength;
            if (hitInfo.searchMode  == SearchMode.FindOnly)
                segmentLength = hitInfo.Text.Length;
            else
                segmentLength = hitInfo.ReplaceText.Length;

            hitInfo.committed = true;  

            Word.Range rngDoc = theDoc.Range(hitInfo.StartDocPosition, hitInfo.StartDocPosition + segmentLength);
            rngDoc.Font.Color = hitInfo.OriginalTextColor;
            rngDoc.Select();

            app.Activate();
        }

        public void Revert(ref HitInfo hitInfo)
        {
            int segmentLength;
            if (hitInfo.searchMode == SearchMode.FindOnly)
                segmentLength = hitInfo.Text.Length;
            else
                segmentLength = hitInfo.ReplaceText.Length;

            hitInfo.reverted = true;

            Word.Range rngDoc = theDoc.Range(hitInfo.StartDocPosition, hitInfo.StartDocPosition + segmentLength);
            rngDoc.Select();
            rngDoc.Text = hitInfo.Text;

            // this is temporarly hardcoded
            rngDoc.Font.Color = Microsoft.Office.Interop.Word.WdColor.wdColorPink;

            app.Activate();
        }
#else

        // Opens a MS WORD or RTF document
        public void OpenDocument(string documentName)
        {
            object optional = Missing.Value;
            object visible = true;
            object fileName = documentName;
            if (app == null)
                app = new Word.Application();

            app.Visible = true;

            try
            {
                // have Word open the document
                theDoc = app.Documents.Open(ref fileName, ref optional,
                    ref optional, ref optional, ref optional, ref optional, ref optional,
                    ref optional, ref optional, ref optional, ref optional, ref visible,
                    ref optional, ref optional, ref optional, ref optional);

                paraCount = theDoc.Paragraphs.Count;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ": Error opening document");
            }
        }

       // save document
        public void SaveDocument()
        {
            object noPrompt = true;
            object originalFormat = Word.WdOriginalFormat.wdPromptUser;
            app.Documents.Save(ref noPrompt, ref originalFormat);
        }
       
        // saves document with name specified in documentName
        public void SaveAsDocument(string documentName)
        {
            try
            {
                object optional = Missing.Value;
                object docName = (object)documentName;
                object originalFormat;
                string ext = Path.GetExtension(documentName).ToLower();
                if (ext == ".doc")
                    originalFormat = Word.WdSaveFormat.wdFormatDocument;
                else if (ext == ".rtf")
                    originalFormat = Word.WdSaveFormat.wdFormatRTF;
                else
                {
                    MessageBox.Show("Invalid file format");
                    return;
                }

                theDoc.SaveAs(ref docName, ref originalFormat, ref optional, ref optional, ref optional,
                    ref optional, ref optional, ref optional, ref optional, ref optional, ref optional, ref optional,
                    ref optional, ref optional, ref optional, ref optional);
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                MessageBox.Show(ex.Message + "\nDocument: " + theDoc.Name, "MS Word Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // closes document
        public void CloseDocument()
        {
            try
            {
                object optional = Missing.Value;
                object saveChanges = Word.WdSaveOptions.wdPromptToSaveChanges;
                object routeDocument = true;
                object originalFormat = Word.WdOriginalFormat.wdPromptUser;

                // the following code is a work around
                DialogResult dr = MessageBox.Show("Do you want to save the changes to: " + theDoc.Name, "Microsoft Office Word", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                if (dr == DialogResult.No)
                {
                    saveChanges = Word.WdSaveOptions.wdDoNotSaveChanges;
                }
                else if (dr == DialogResult.Cancel)
                {
                    return;
                }

                app.Documents.Close(ref saveChanges, ref originalFormat, ref routeDocument);
                theDoc = null;
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                MessageBox.Show(ex.Message + "\nDocument: " + theDoc.Name, "MS Word Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

      internal bool FindTextInDoc(OperationMode opMode, int currentParaNum, string textToFind, Word.WdColor color, int start, out int end, out int textStartPoint, string replaceStr, SearchMode theSearchMode)
        {
            string strFind = textToFind;
            textStartPoint = 0;

            // get the range of the curent paragraph
            Word.Range rngDoc = GetRng(currentParaNum);

            // make sure we are not past the end of the range
            if (start >= rngDoc.End)
            {
                end = 0;
                return false;
            }
            rngDoc.Start = start;

            // setup Microsoft Word Find based upon
            // Regular Expression Match
            rngDoc.Find.ClearFormatting();
            rngDoc.Find.Forward = true;
            rngDoc.Find.Text = textToFind;

            // make search case sensitive
            object caseSensitive = "1";
            object missingValue = Type.Missing;

            // wild cards
            object matchWildCards = Type.Missing;

            // this is for a future version
            if (opMode == OperationMode.Word97Mode)
                matchWildCards = "1";

            // find the text in the word document
            rngDoc.Find.Execute(ref missingValue, ref caseSensitive,
                ref missingValue, ref missingValue, ref missingValue,
                ref missingValue, ref missingValue, ref missingValue,
                ref missingValue, ref missingValue, ref missingValue,
                ref missingValue, ref missingValue, ref missingValue,
                ref missingValue);

            // select text if true
            if (hilightText)
                rngDoc.Select();

            // if we are in replace mode, replace the found text with its
            // formatted version
            if (hasValidationAssembly && theSearchMode == SearchMode.FindAndReplace)
                rngDoc.Text = replaceStr;

            // set new end point to the character position afer
            // the found test
            end = rngDoc.End + 1;

            // get segment string
            //           string txtSegStr = "";
            //            GetTextSegmentNeighborhood(rngDoc, out txtSegStr);

            // character position where the matched string begins
            textStartPoint = rngDoc.Start;

            // we found the text
            if (rngDoc.Find.Found)
            {
                rngDoc.Font.Color = color;
                // the range endpoint will change if we modified the text
                return true;
            }
            return false;
        }

        public void FindTextInRange(HitInfo hitInfo, SearchMode searchMode)
        {
            int segmentLength;
            if (searchMode == SearchMode.FindOnly)
                segmentLength = hitInfo.Text.Length;
            else
                segmentLength = hitInfo.ReplaceText.Length;

            object start = (object)hitInfo.StartDocPosition;
            object end = (object)(hitInfo.StartDocPosition + segmentLength);
            Word.Range rngDoc = theDoc.Range(ref start, ref end);
            rngDoc.Select();
            app.Activate();
        }

        public void Revert(HitInfo hitInfo, SearchMode searchMode)
        {
            int segmentLength;
            if (searchMode == SearchMode.FindOnly)
                segmentLength = hitInfo.Text.Length;
            else
                segmentLength = hitInfo.ReplaceText.Length;

            object start = (object)hitInfo.StartDocPosition;
            object end = (object)(hitInfo.StartDocPosition + segmentLength);
            Word.Range rngDoc = theDoc.Range(ref start, ref end);
            rngDoc.Select();
            rngDoc.Text = hitInfo.Text;
            // this is temporarly hardcoded
            rngDoc.Font.Color = Microsoft.Office.Interop.Word.WdColor.wdColorPink;

            app.Activate();
        }
#endif

        // Returns the range of characters for the given paragraph number
        public Word.Range GetRng(int nParagraphNumber)
        {
            try
            {
                return theDoc.Paragraphs[nParagraphNumber].Range;
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                MessageBox.Show(ex.Message + "\nParagraph Number: " + nParagraphNumber.ToString() + " does not exist.");
                return null;
            }
        }

        // Returns the specific MS Word color based on the string name
        public Word.WdColor GetSearchColor(string colorStr)
        {
            switch(colorStr)
            {
                 case "Aqua":
                    return Word.WdColor.wdColorAqua;
                 case "Blue":
                    return Word.WdColor.wdColorBlue;
                 case "Brown":
                    return Word.WdColor.wdColorBrown;
                 case "BrightGreen":
                    return Word.WdColor.wdColorBrightGreen;
                 case "DarkBLue":
                    return Word.WdColor.wdColorDarkBlue;
                 case "DarkRed":
                    return Word.WdColor.wdColorDarkRed;
                 case "Gold":
                    return Word.WdColor.wdColorGold;
                 case "Gray":
                    return Word.WdColor.wdColorGray50;
                 case "Green":
                    return Word.WdColor.wdColorGreen;
                case "Indigo":
                    return Word.WdColor.wdColorIndigo;
                case "Lime":
                    return Word.WdColor.wdColorLime;
                case "Olive":
                    return Word.WdColor.wdColorOliveGreen;
                case "Orange":
                    return Word.WdColor.wdColorOrange;
                case "Pink":
                    return Word.WdColor.wdColorPink;
                case "Plum":
                    return Word.WdColor.wdColorPlum;
                case "Rose":
                    return Word.WdColor.wdColorRose;
                case "SeaGreen":
                    return Word.WdColor.wdColorSeaGreen;
                case "SkyBlue":
                    return Word.WdColor.wdColorSkyBlue;
                case "Tan":
                    return Word.WdColor.wdColorTan;
                case "Teal":
                    return Word.WdColor.wdColorTeal;
                case "Turquoise":
                    return Word.WdColor.wdColorTurquoise;
                case "Violet":
                    return Word.WdColor.wdColorViolet;
                case "Yellow":
                    return Word.WdColor.wdColorYellow;
               case "BlueGray":
                    return Word.WdColor.wdColorBlueGray;
               default:
                    return Word.WdColor.wdColorRed;
            }
        }

        // converts string color name to System.Drwing.Color type
        public System.Drawing.Color ConvertSearchToSystemColor(string colorStr)
        {
            switch(colorStr)
            {
                case "Aqua":
                    return System.Drawing.Color.Aqua;
                case "Blue":
                    return System.Drawing.Color.Blue;
                case "Brown":
                    return System.Drawing.Color.Brown;
                case "Green":
                    return System.Drawing.Color.Green;
                case "Gold":
                    return System.Drawing.Color.Gold;
                case "Violet":
                    return System.Drawing.Color.Violet;
                 case "Indigo":
                    return System.Drawing.Color.Indigo;
                case "Teal":
                    return System.Drawing.Color.Teal;
                case "BrightGreen":
                    return System.Drawing.Color.Honeydew;
                case "DarkBLue":
                    return System.Drawing.Color.DarkBlue;
                case "DarkRed":
                    return System.Drawing.Color.DarkRed;
                case "Gray":
                    return System.Drawing.Color.Gray;
                case "Olive":
                    return System.Drawing.Color.Olive;
                case "Orange":
                    return System.Drawing.Color.Orange;
                case "Pink":
                    return System.Drawing.Color.Pink;
                case "Plum":
                    return System.Drawing.Color.Plum;
                case "Red":
                    return System.Drawing.Color.Red;
                case "Rose":
                    return System.Drawing.Color.RosyBrown;
                case "SeaGreen":
                    return System.Drawing.Color.SeaGreen;
                case "SkyBlue":
                    return System.Drawing.Color.SkyBlue;
                case "Tan":
                    return System.Drawing.Color.Tan;
                case "Turquoise":
                    return System.Drawing.Color.Turquoise;
                case "Lime":
                    return System.Drawing.Color.Lime;
                case "Yellow":
                    return System.Drawing.Color.Yellow;
                default:
                    return System.Drawing.Color.Red;
             }
        }


        // loads the search assembly and the desired plug-in function
        public bool LoadSearchAssembly(string plugginName, string plugInValidationFunction, string plugInFormatFunction)
        {
            try
            {
                // if there is no validation assembly, leave
                if (plugginName.ToLower() == null || plugginName.ToLower() == "none")
                {
                    hasValidationAssembly = false;
                    return true;
                }
                hasValidationAssembly = true;

                // Use the file name to load the assembly into the current 
                // application domain.

                string plugginPath =  Path.GetDirectoryName(Application.ExecutablePath) + @"\Plugins\" + plugginName;
                if (!File.Exists(plugginPath))
                    throw new Exception("Cannot find path to assembly: " + plugginName);

                Assembly a = Assembly.LoadFrom(plugginPath);
                // Get the type to use.
                Type[] types = a.GetTypes();

                // Get the method to call.
                validationMethod = types[0].GetMethod(plugInValidationFunction);

                // Get the method to call.
                formattedMatchMethod = types[0].GetMethod(plugInFormatFunction);

                // Create an instance.
                assemblyInstance = Activator.CreateInstance(types[0]);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        // perform a search based on regular expressions
        public bool RegularExpressionFind(int paraNum, string docText, SearchStruct selSearchStruct, int indx, out List<HitInfo> hits)
        {
            HitInfo hitInfo = new HitInfo();
            hits = new List<HitInfo>();

            Word.WdColor color = GetSearchColor(selSearchStruct.TextColor);

            SearchMode searchMode = new SearchMode();
            if (selSearchStruct.Action == "find")
                searchMode = SearchMode.FindOnly;
            else
                searchMode = SearchMode.FindAndReplace;

            hitInfo.searchMode = searchMode;
            
            MatchCollection matches = regExList[indx].Matches(docText);

            // no matches go on to next paragraph
            if (matches.Count == 0)
                return false;

            // check if we have a validation assembly
            try
            {
                if (!LoadSearchAssembly(selSearchStruct.PlugInName, selSearchStruct.PlugInValidationFunction,
                        selSearchStruct.PlugInFormatFunction))
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            int index = 0;

            // this is the start point in the MS Office document
            int startSearchPos = GetRng(paraNum).Start;

            // if we are in search and replace mode this variable hold
            // the fromatted match that will replace the original match
            string formattedMatchStr = "";

            foreach (Match match in matches)
            {
                // Perform validation check
                if (hasValidationAssembly)
                {
                    Object[] objList = new Object[1];
                    objList[0] = (Object)match;
                    if (validationMethod != null)
                    {
                        if (!Convert.ToBoolean(validationMethod.Invoke(assemblyInstance, objList)))
                            continue;
                    }
                    if (formattedMatchMethod != null && searchMode == SearchMode.FindAndReplace)
                    {
                        formattedMatchStr = Convert.ToString(formattedMatchMethod.Invoke(assemblyInstance, objList));
                    }
                    else
                        formattedMatchStr = match.Value;

                }
                index = docText.IndexOf(match.Value, index);
                
                // we assume the URL extends until first white space
                string matchStr = docText.Substring(index, match.Value.Length);
                index += matchStr.Length - 1;

                // find the pattern in the word document
                FindTextInDoc(OperationMode.DotNetRegExMode, paraNum, matchStr, color, startSearchPos, out  startSearchPos, out hitInfo.StartDocPosition, out hitInfo.OriginalTextColor, formattedMatchStr, searchMode);

                // add match to our hit list
                hitInfo.Text = match.Value;
                hitInfo.ReplaceText = formattedMatchStr;
                hitInfo.TextColor = ConvertSearchToSystemColor(selSearchStruct.TextColor);
                hits.Add(hitInfo);
           }
            return true;
        }

        public void GetTextSegmentNeighborhood(Word.Range rngWord, out string segStr)
        {
            int offset = 15;
            int start = rngWord.Start;
            int end = rngWord.End;

            int paraStart = GetRng(1).Start;
            int paraEnd = GetRng(1).End;
            start -= offset;
            end += offset;

            if (start< paraStart)
                start = paraStart;
            if (end> paraEnd)
                end = paraEnd;

            object ostart = (object)start;
            object oend = (object)end;
          Word.Range rngSeg = theDoc.Range(ref ostart, ref oend);


            // need to see if target string is unique within segment.
            // actually using offset as an indicator should be sufficient
            // make it a member variable
            segStr = rngSeg.Text;

        }
    }
}
