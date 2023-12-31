/***

Copyright 2023, SV Foster. All rights reserved.

License:
    This program is free for personal, educational and/or non-profit usage    

Revision History:

***/

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;


namespace sBackup
{
    class TGlobalOptions
    {
        public enum OperatingMode
        {
            Help,
            Backup
        }

        public OperatingMode Mode { get; set; } = OperatingMode.Backup;
        private string SavePathLocal = ".\\";
        public string SavePath { get { return SavePathLocal; } set { SavePathSet(value); } }
        public string TasksFile { get; set; } = "default.xml";
        public bool NoCopyrightLogo { get; set; } = false;
        public List<string> ExcludeFileStandardList { get; } = new List<string>(128);
        private static readonly string OptionsFileName = "options.xml";
        private static readonly string OptionsSchemaFileName = "options.xsd";


        public TGlobalOptions()
        {
            // load up data from the options file
            // validate XML first
            XmlReaderSettings SchemaOptionsXML = new XmlReaderSettings();
            SchemaOptionsXML.Schemas.Add(null, OptionsSchemaFileName);
            SchemaOptionsXML.ValidationType = ValidationType.Schema;
            SchemaOptionsXML.ValidationEventHandler += SettingsValidationEventHandler;

            XmlReader OptionsXMLReader = XmlReader.Create(OptionsFileName, SchemaOptionsXML);

            XmlDocument OptionsXML = new XmlDocument();
            OptionsXML.Load(OptionsXMLReader);
            XmlElement xRoot = OptionsXML.DocumentElement;
            
            // now read arrays
            var ExclFileNodes = xRoot?.SelectNodes("//ExcludeFileStandardList/Item");
            if (ExclFileNodes != null)
            {
                foreach (XmlNode node in ExclFileNodes)
                    ExcludeFileStandardList.Add(node.InnerText);
            }
        }

        private void SavePathSet(string n)
        {
            this.SavePathLocal = n.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
        }

        private static void SettingsValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Warning:
                    Console.WriteLine(String.Format(Properties.Resources.UIMSG_110_CONFIG_FILE_WARNING, e.Message));
                    break;

                case XmlSeverityType.Error:
                    Console.WriteLine(String.Format(Properties.Resources.UIMSG_111_CONFIG_FILE_ERROR, e.Message));

                    break;

                default:
                    break;
            }
        }
    }
}
