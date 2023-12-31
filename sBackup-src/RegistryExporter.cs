/***

Copyright 2023, SV Foster. All rights reserved.

License:
    This program is free for personal, educational and/or non-profit usage    

Revision History:

***/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Win32;


namespace sBackup
{
    class RegistryExporter
    {
        public List<string> PathList { get; }


        public RegistryExporter(in string[] PathList)
        {
            this.PathList = new List<string>(PathList.Length);
            this.PathList.AddRange(PathList);
        }

        public void Export(in string OutputFile)
        {
            using (var fileWriter = new StreamWriter(OutputFile, false, Encoding.Unicode))
            {
                foreach (var n in PathList)
                {
                    string[] splitted = n.Split('\\');

                    if (splitted.Count() < 1)
                        throw new TRegistryExporterInvalidPathException("");

                    RegistryHive h = GetRegistryHive(splitted[0]);
                    string p = string.Join("\\", splitted, 1, splitted.Length - 1);

                    ExportRegistryKeysRecursive(h, p, fileWriter);
                }
            }
        }

        private void ExportRegistryKeysRecursive(RegistryHive hive, in string keyPath, in StreamWriter fileWriter)
        {
            RegistryKey BaseKey;

            try
            {
                BaseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64);
            }
            catch (ArgumentException ex)
            {
                throw new TRegistryExporterInvalidPathException(ex.Message, ex);
            }

            RegistryKey SubKey = BaseKey.OpenSubKey(keyPath, false);
            if (SubKey == null)
                return;

            // log current key path
            fileWriter.WriteLine("\n" + keyPath);
            // log all values in this key
            foreach (string valueName in SubKey.GetValueNames())
            {

                fileWriter.WriteLine(String.Format
                (
                    "[{0}]{1} = {2}",

                    SubKey.GetValueKind(valueName).ToString(),
                    valueName,
                    SubKey.GetValue(valueName)
                ));
            }
            // if there are subkeys -- recursively call myself
            foreach (string SubKeyEntry in SubKey.GetSubKeyNames())
            {
                ExportRegistryKeysRecursive(hive, keyPath + @"\" + SubKeyEntry, fileWriter);
            }
        }

        private RegistryHive GetRegistryHive(in string hive)
        {
            switch (hive.ToUpper())
            {
                case "HKEY_CLASSES_ROOT":
                case "HKCR":
                    return RegistryHive.ClassesRoot;

                case "HKEY_CURRENT_USER":
                case "HKCU":
                    return RegistryHive.CurrentUser;

                case "HKEY_LOCAL_MACHINE":
                case "HKLM":
                    return RegistryHive.LocalMachine;

                case "HKEY_USERS":
                case "HKU":
                    return RegistryHive.Users;

                case "HKEY_CURRENT_CONFIG":
                case "HKC":
                    return RegistryHive.CurrentConfig;

                default:
                    throw new TRegistryExporterRootHiveException(hive);
            }
        }
    }

    class TRegistryExporterRootHiveException : Exception
    {
        public TRegistryExporterRootHiveException(in string message)
            : base(message) { }
    }

    class TRegistryExporterInvalidPathException : Exception
    {
        public TRegistryExporterInvalidPathException(in string message)
            : base(message) { }

        public TRegistryExporterInvalidPathException(in string message, Exception innerException)
            : base(message, innerException) { }
    }
}
