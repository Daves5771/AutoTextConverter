using System;
using System.Collections.Generic;
using System.IO;

using System.Text;
using System.Xml;

// SaelSoft -- XmlModule.cs
// Purpose --  Reads and Parses Searches.XML
// 2008-2016   David Saelman

namespace SaelSoft.AutoTextConverter
{
    public class ProfileHeaderStruct
    {
        public string fileName;
        public string owner;
        public string project;
        public string creationDate;
        public string description;
        public string history;
        public string version;
        public List<string> listOfSearchEntries;
    }

    class XmlModule
    {
        XmlDocument xmldoc;
        string xmlPath;

        public XmlModule()
        {
            xmldoc = new XmlDocument();
            string aName = "";
            try
            {
                aName = System.Reflection.Assembly.GetExecutingAssembly().Location;// GetName().CodeBase;

                aName = System.IO.Path.GetDirectoryName(aName).ToString();
#if DEBUG
                int index = aName.IndexOf("bin");
                string pathName = aName.Substring(0, index - 1);

                xmlPath = pathName + @"\XML Files\";
#else
                xmlPath = aName + @"\XML Files\";
#endif
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(aName + " \\n" + ex.Message);
            }
        }

        public string GetXMLFilesPath()
        {
            string filepath;
            string aName = System.Reflection.Assembly.GetExecutingAssembly().Location;

            aName = System.IO.Path.GetDirectoryName(aName).ToString();
#if DEBUG
            int index = aName.IndexOf("bin");
            string pathName = aName.Substring(0, index - 1);
            filepath = pathName + @"\XML Files\";
#else
            filepath = aName + @"\XML Files\";
#endif
            return filepath;
        }

        public void GetListOfChoices(ref List<string> names)
        {
            DirectoryInfo di = new DirectoryInfo(GetXMLFilesPath());
            FileInfo[] rgFiles = di.GetFiles("*.xml");
            foreach (FileInfo fi in rgFiles)
            {
                names.Add(Path.GetFileNameWithoutExtension(fi.Name));
            }
        }

#if DONTUSE
        public void LoadSearcheAndReplaceEntries(List<SearchStruct> searches)
        {
            LoadSearches(xmlPath + "Search and Replace.xml", searches);
        }
        public void LoadSearchEntries(List<SearchStruct> searches)
        {
            LoadSearches(xmlPath + "searches.xml", searches);
        }
#endif
        public void LoadSearchEntries(string searchName, List<SearchStruct> searches)
        {
            LoadSearches(xmlPath + searchName +".xml", searches);
        }

        private void LoadSearches(string fileName, List<SearchStruct> searches)
        {
            try
            {
                SearchStruct searchStruct = new SearchStruct();
                xmldoc.Load(fileName);

                // parse optional Header
                ProfileHeaderStruct ProfileHeader = new ProfileHeaderStruct();
                XmlNode xmlNextChildNode;

                XmlNodeList xmlnode = xmldoc.GetElementsByTagName("Header");
                if (xmlnode.Count > 0)
                {
                    ProfileHeader.project = xmlnode[0].FirstChild.InnerText;

                    ProfileHeader.owner = xmlnode[0].FirstChild.NextSibling.InnerText;
                    xmlNextChildNode = xmlnode[0].FirstChild.NextSibling;

                    xmlNextChildNode = xmlNextChildNode.NextSibling;
                    ProfileHeader.creationDate = xmlNextChildNode.InnerText;

                    xmlNextChildNode = xmlNextChildNode.NextSibling;
                    ProfileHeader.version = xmlNextChildNode.InnerText;

                    xmlNextChildNode = xmlNextChildNode.NextSibling;
                    ProfileHeader.description = xmlNextChildNode.InnerText;

                    xmlNextChildNode = xmlNextChildNode.NextSibling;
                    ProfileHeader.history = xmlNextChildNode.InnerText;
                }

                // parse Searches
                xmlnode = xmldoc.GetElementsByTagName("Search");
                searches.Clear();

                for (int i = 0; i < xmlnode.Count; i++)
                {
                    XmlAttributeCollection xmlattrc = xmlnode[i].Attributes;

                    //XML Attribute Name and Value returned
                    //Example: <Search RegEx="([0-6]\d{2}|7[0-6]\d|77[0-2])([ \-]?)(\d{2})\2(\d{4})">
                    searchStruct.Identifier = xmlattrc[0].Value;
                    searchStruct.RegExpression = xmlattrc[0].Value;

                    searchStruct.Identifier = xmlnode[i].FirstChild.InnerText;
                    searchStruct.TextColor = xmlnode[i].FirstChild.NextSibling.InnerText;
                    xmlNextChildNode = xmlnode[i].FirstChild.NextSibling;
                    // Action - search, search and replace
                    xmlNextChildNode = xmlNextChildNode.NextSibling;
                    searchStruct.Action = xmlNextChildNode.InnerText;

                    // if we have a plugin get the details
                    if (xmlNextChildNode.NextSibling.OuterXml.Contains("yes"))
                    {
                        // pluggin assembly name
                        xmlNextChildNode = xmlNextChildNode.NextSibling;
                        xmlNextChildNode = xmlNextChildNode.FirstChild;
                        searchStruct.PlugInName = xmlNextChildNode.InnerText;

                        // pluggin validation function name
                        xmlNextChildNode = xmlNextChildNode.NextSibling;
                        searchStruct.PlugInValidationFunction = xmlNextChildNode.InnerText;

                        // pluggin fromat function name
                        xmlNextChildNode = xmlNextChildNode.NextSibling;
                        searchStruct.PlugInFormatFunction = xmlNextChildNode.InnerText;
                        // reserved
                        xmlNextChildNode = xmlNextChildNode.NextSibling;

                        xmlNextChildNode = xmlNextChildNode.ParentNode;
                    }
                    else
                    {
                        searchStruct.PlugInName = "none";
                        searchStruct.PlugInFormatFunction = "none";
                        searchStruct.PlugInValidationFunction = "none";
                    }

                    xmlNextChildNode = xmlNextChildNode.NextSibling;

                    //Last Child of the XML file
                    xmlattrc = xmlnode[i].LastChild.Attributes;
                    searchStruct.Description = xmlattrc[0].Value;

                    // reserved
       //             xmlNextChildNode = xmlNextChildNode.NextSibling;
                    // reserved
      //              xmlNextChildNode = xmlNextChildNode.NextSibling;

                    searches.Add(searchStruct);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
