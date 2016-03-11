using System;
using System.Collections.Generic;
using System.IO;

using System.Text;
using System.Xml;

// SaelSoft -- XmlModule.cs
// Purpose -- Reads and Parses Searches.XML
// 2008 David Saelman

namespace AutoTextConverter
{
    class XmlModule
    {
        XmlDocument xmldoc;
        string xmlPath;

        public XmlModule()
        {
            xmldoc = new XmlDocument();

            string aName = System.Reflection.Assembly.GetExecutingAssembly().Location;// GetName().CodeBase;

            aName = System.IO.Path.GetDirectoryName(aName).ToString();
            int index = aName.IndexOf("bin");
            string pathName = aName.Substring(0, index - 1);
            xmlPath = pathName + @"\XML Files\";

        }

        public string GetXMLFilesPath()
        {
            string filepath;
            string aName = System.Reflection.Assembly.GetExecutingAssembly().Location;// GetName().CodeBase;

            aName = System.IO.Path.GetDirectoryName(aName).ToString();
            int index = aName.IndexOf("bin");
            string pathName = aName.Substring(0, index - 1);
            filepath = pathName + @"\XML Files\";
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
                XmlNodeList xmlnode = xmldoc.GetElementsByTagName("Search");
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
                    XmlNode xmlNextChildNode = xmlnode[i].FirstChild.NextSibling;
                    // Action - search, search and replace
                    xmlNextChildNode = xmlNextChildNode.NextSibling;
                    searchStruct.Action = xmlNextChildNode.InnerText;
                    // pluggin assembly name
                    xmlNextChildNode = xmlNextChildNode.NextSibling;
                    searchStruct.PlugInName = xmlNextChildNode.InnerText;
                    // pluggin validation function name
                    xmlNextChildNode = xmlNextChildNode.NextSibling;
                    searchStruct.PlugInValidationFunction = xmlNextChildNode.InnerText;
                    // pluggin fromat function name
                    xmlNextChildNode = xmlNextChildNode.NextSibling;
                    searchStruct.PlugInFormatFunction = xmlNextChildNode.InnerText;
                    //Last Child of the XML file
                    xmlattrc = xmlnode[i].LastChild.Attributes;
                    searchStruct.Description = xmlattrc[0].Value;
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
