using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
//using System.Collections.Specialized;

namespace SaelSoft.AutoTextConverter
{
    public partial class Preferences : Form
    {
        public static bool UseColorization = false;

        public Preferences()
        {
            InitializeComponent();
        }

        private void Preferences_Load(object sender, EventArgs e)
        {
            string sAttr = ""; 
            try
            {
                sAttr = ReadSetting("UseColors");

                if (sAttr == "Not Found")
                {
                    AddUpdateAppSettings("UseColors", "true");
                    sAttr = "true";
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Cannot read config values!" + ex.Message);
                sAttr = "true"; 
            }

            if (sAttr == "true")
            {
                UseColorization = true;
                checkBoxTextColorization.Checked = true;
            }
            else
            {
                UseColorization = false;
                checkBoxTextColorization.Checked = false;
            }
        }

        static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }

        static string ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? "Not Found";
                return result;
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
                return "Not Found";
            }
        }

        static void ReadAllSettings()
        {
            try
            {
                var appSettings = System.Configuration.ConfigurationManager.AppSettings;

                if (appSettings.Count == 0)
                {
                    Console.WriteLine("AppSettings is empty.");
                }
                else
                {
                    foreach (var key in appSettings.AllKeys)
                    {
                        Console.WriteLine("Key: {0} Value: {1}", key, appSettings[key]);
                    }
                }
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            UseColorization = checkBoxTextColorization.Checked;
            try
            {
                string boolVal;
                if (UseColorization)
                    boolVal = "true";
                else
                    boolVal = "false";
                AddUpdateAppSettings("UseColors", boolVal);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Cant save config values\n " + ex.Message);
            }
        }
    }
}
