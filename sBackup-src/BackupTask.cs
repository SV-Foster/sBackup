/***

Copyright 2023, SV Foster. All rights reserved.

License:
    This program is free for personal, educational and/or non-profit usage    

Revision History:

***/

using System.Xml;
using System.Xml.Serialization;


namespace sBackup
{
    [XmlRoot("BackupTasks")]
    public class BackupTasks
    {
        [XmlElement("Task")]
        public Task[] Task { get; set; }
    }

    public class Task
    {
        public uint ID { get; set; }
        public uint Type { get; set; }
        public bool IsActive { get; set; }
        [XmlElement("Files")]
        public Files Files { get; set; }
        [XmlElement("FilesList")]
        public FilesList FilesList { get; set; }
        [XmlElement("Registry")]
        public Registry Registry { get; set; }
    }

    public class Files
    {
        public string OutFileName { get; set; }
        public bool InputRecursive { get; set; }
        [XmlArray("InputPathList")]
        [XmlArrayItem("Item")]
        public string[] InputPathList { get; set; }
        public bool ExcludeStandard { get; set; }
        [XmlArray("ExcludeList")]
        [XmlArrayItem("Item")]
        public string[] ExcludeList { get; set; }
        public uint CompressionType { get; set; }
    }

    public class FilesList
    {
        public string OutFileName { get; set; }
        [XmlArray("InputPathList")]
        [XmlArrayItem("Item")]
        public string[] InputPathList { get; set; }
        public bool InputRecursiveDepthApplyed { get; set; }
        public uint InputRecursiveDepth { get; set; }
    }

    public class Registry
    {
        public string OutFileName { get; set; }
        [XmlArray("InputPathList")]
        [XmlArrayItem("Item")]
        public string[] InputPathList { get; set; }
        public bool InputRecursiveDepthApplyed { get; set; }
        public uint InputRecursiveDepth { get; set; }
    }

    class TBackupTask
    {
    }
}
