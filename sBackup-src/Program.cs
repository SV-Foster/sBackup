/***

Copyright 2023, SV Foster. All rights reserved.

License:
    This program is free for personal, educational and/or non-profit usage    

Revision History:

***/

using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;


namespace sBackup
{
    public enum ExitCodes: int
	{
        EXIT_SUCCESSFUL = 0,
        EXIT_MISCONFIG,
        EXIT_IOERROR,
        EXIT_NODEPENDENCY
    }

    class Program
    {
        public static readonly string TasksSchema = "tasks.xsd";


        static void Main(string[] args)
        {
            // main exception handler
            System.AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
            TGlobalOptions GlobalOptions = new TGlobalOptions();


            TCommandLineInterface.SetModeUTF16();
            Console.WriteLine();

            // collect parameters
            TCommandLineInterface.WorkModeGet(args, ref GlobalOptions);
            if (GlobalOptions.Mode == TGlobalOptions.OperatingMode.Help)
            {
                ModeHelp();
                Environment.Exit((int)ExitCodes.EXIT_SUCCESSFUL);
            }
            TCommandLineInterface.SwitchesGet(args, ref GlobalOptions);

            // start the job
            if (!GlobalOptions.NoCopyrightLogo)
                TCommandLineInterface.LogoPrint();

            TCommandLineInterface.ProgramStartOperationPrint(GlobalOptions.Mode);

            switch(GlobalOptions.Mode)
            {
                case TGlobalOptions.OperatingMode.Backup:
                    ModeBackup(GlobalOptions);
                    break;

                default:
                    throw new TsBackupMisconfigException();
            }

            TCommandLineInterface.ProgramExitSummaryPrint();
            Environment.Exit((int)ExitCodes.EXIT_SUCCESSFUL);
        }

        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;
            Console.Beep();
            Console.Beep();
            Console.Beep();

            // TCommandLineInterface
            if (ex is TCommandLineInterfaceBadWorkModeException)
            {
                TCommandLineInterface.LogoPrint();
                Console.WriteLine(Properties.Resources.UIMSG_118_CMD_NO_COMMAND);
                Environment.Exit((int)ExitCodes.EXIT_MISCONFIG);
            }

            if (ex is TCommandLineInterfaceSwitchNoValueException)
            {
                TCommandLineInterface.LogoPrint();
                Console.WriteLine(String.Format(Properties.Resources.UIMSG_119_CMD_NO_VALUE, ex.Message));
                Environment.Exit((int)ExitCodes.EXIT_MISCONFIG);
            }

            if (ex is TCommandLineInterfaceInvalidSwitchException)
            {
                TCommandLineInterface.LogoPrint();
                Console.WriteLine(String.Format(Properties.Resources.UIMSG_120_CMD_INVALID_SW, ex.Message));
                Environment.Exit((int)ExitCodes.EXIT_MISCONFIG);
            }

            if (ex is TCommandLineInterfaceInvalidSwitchParamException)
            {
                TCommandLineInterface.LogoPrint();
                Console.WriteLine(String.Format(Properties.Resources.UIMSG_121_CMD_INVALID_PARAM, ex.Message));
                Environment.Exit((int)ExitCodes.EXIT_MISCONFIG);
            }

            // Main
            if (ex is TsBackupMisconfigException)
            {
                Console.WriteLine(Properties.Resources.UIMSG_100_APP_MISCONFIG);
                Environment.Exit((int)ExitCodes.EXIT_MISCONFIG);
            }

            // TRegistryExporter
            if (ex is TRegistryExporterRootHiveException)
            {
                Console.WriteLine(String.Format(Properties.Resources.UIMSG_115_REG_HIVE_INVALID, ex.Message));
                Environment.Exit((int)ExitCodes.EXIT_MISCONFIG);
            }
            
            if (ex is TRegistryExporterInvalidPathException)
            {
                Console.WriteLine(String.Format(Properties.Resources.UIMSG_114_REG_PATH_INVALID, ex.Message));
                Environment.Exit((int)ExitCodes.EXIT_MISCONFIG);
            }

            // TFileLister            
            if (ex is TFileListerInvalidPathException)
            {
                Console.WriteLine(String.Format(Properties.Resources.UIMSG_112_EXCEPTION_FILE_PATH_INVALID, ex.Message));
                Environment.Exit((int)ExitCodes.EXIT_MISCONFIG);
            }

            // TWinRAR
            if (ex is TWinRARArchiverNotInstalledException)
            {
                var rex = (TWinRARArchiverNotInstalledException)ex;

                switch (rex.Qualification)
                {
                    case TWinRARArchiverNotInstalledException.TQualification.Registry:
                        Console.WriteLine(String.Format(Properties.Resources.UIMSG_117_WINRAR_NOT_INSTALLED_REGISTRY, rex.RegPath));
                        break;

                    case TWinRARArchiverNotInstalledException.TQualification.Executable:
                        Console.WriteLine(String.Format(Properties.Resources.UIMSG_128_WINRAR_NOT_INSTALLED_EXE, rex.Executable));
                        break;
                }
                
                Environment.Exit((int)ExitCodes.EXIT_NODEPENDENCY);
            }

            if (ex is TWinRARArchiverCallException)
            {
                var rex = (TWinRARArchiverCallException)ex;

                Console.WriteLine(String.Format(Properties.Resources.UIMSG_116_WINRAR_EXCAPTION, rex.ExitCode));
                Environment.Exit((int)ExitCodes.EXIT_IOERROR);
            }

            // all others            
            string msg = String.Format
            (
                Properties.Resources.UIMSG_101_GENERAL_ERROR,

                ex.Message,
                ex.GetType().ToString()
            );
            Console.WriteLine(msg);
            Environment.Exit((int)ExitCodes.EXIT_IOERROR);
        }

        private static void ModeHelp()
        {
            TCommandLineInterface.LogoPrint();
            TCommandLineInterface.HelpPrint();       
        }

        private static void ModeBackup(in TGlobalOptions GlobalOptions)
        {
            // load up tasks from the file
            // validate XML first
            XmlReaderSettings SchemaOptionsXML = new XmlReaderSettings();
            SchemaOptionsXML.Schemas.Add(null, TasksSchema);
            SchemaOptionsXML.ValidationType = ValidationType.Schema;
            SchemaOptionsXML.ValidationEventHandler += SettingsValidationEventHandler;

            XmlReader OptionsXMLReader = XmlReader.Create(GlobalOptions.TasksFile, SchemaOptionsXML);

            // now deserialize
            XmlSerializer serializer = new XmlSerializer(typeof(BackupTasks));
            BackupTasks BackupTasksArray = (BackupTasks)serializer.Deserialize(OptionsXMLReader);

            // start tasks
            foreach (var n in BackupTasksArray.Task)
            {
                Console.WriteLine(String.Format(Properties.Resources.UIMSG_102_TASK_START, n.ID, n.Type));
                if (n.IsActive == false)
                {
                    Console.WriteLine(Properties.Resources.UIMSG_103_TASK_DISABLED);
                    continue;
                }

                switch (n.Type)
                {
                    case 0:
                        Console.WriteLine(String.Format(Properties.Resources.UIMSG_104_ARCHIVING, string.Join(", ", n.Files.InputPathList)));

                        var WinRAR = new TWinRARArchiver();

                        WinRAR.ArchivePath = GlobalOptions.SavePath + n.Files.OutFileName;
                        WinRAR.Recursive = n.Files.InputRecursive;
                        WinRAR.InputPathList.AddRange(n.Files.InputPathList);
                        if (n.Files.ExcludeStandard)
                            WinRAR.ExcludeList.AddRange(GlobalOptions.ExcludeFileStandardList);
                        WinRAR.ExcludeList.AddRange(n.Files.ExcludeList);
                        WinRAR.Compression = (TWinRARArchiver.TCompressionType)n.Files.CompressionType;

                        WinRAR.StartAndWait();

                        Console.WriteLine(Properties.Resources.UIMSG_105_TASK_DONE_NOERROR);
                        break;


                    case 1:
                        Console.WriteLine(String.Format(Properties.Resources.UIMSG_106_LISTING_FILES, string.Join(", ", n.FilesList.InputPathList)));

                        TFileLister FileLister = new TFileLister
                        (
                            n.FilesList.InputPathList,
                            n.FilesList.InputRecursiveDepthApplyed,
                            n.FilesList.InputRecursiveDepth
                        );

                        FileLister.ListFiles(GlobalOptions.SavePath + n.FilesList.OutFileName);

                        Console.WriteLine(Properties.Resources.UIMSG_105_TASK_DONE_NOERROR);
                        break;


                    case 2:
                        Console.WriteLine(String.Format(Properties.Resources.UIMSG_107_LISTING_REGISTRY, string.Join(", ", n.Registry.InputPathList)));

                        RegistryExporter RexExport = new RegistryExporter(n.Registry.InputPathList);

                        RexExport.Export(GlobalOptions.SavePath + n.Registry.OutFileName);

                        Console.WriteLine(Properties.Resources.UIMSG_105_TASK_DONE_NOERROR);
                        break;


                    default:
                        throw new TsBackupMisconfigException();
                }
            }
        }

        private static void SettingsValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Warning:
                    Console.WriteLine(String.Format(Properties.Resources.UIMSG_108_BACKUP_FILE_WARNING, e.Message));
                    break;

                case XmlSeverityType.Error:
                    Console.WriteLine(String.Format(Properties.Resources.UIMSG_109_BACKUP_FILE_ERROR, e.Message));

                    break;

                default:
                    break;
            }
        }

        class TsBackupMisconfigException : Exception
        {
            public TsBackupMisconfigException()
                { }
        }
    }

    static class StringExtensions
    {
        public static string ReplaceEscapeSequences(this string input)
        {
            return input
                .Replace("\\n", "\n")
                .Replace("\\r\\n", "\r\n")
                .Replace("\\t", "\t");
        }
    }
}
