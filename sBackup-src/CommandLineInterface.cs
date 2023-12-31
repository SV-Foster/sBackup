/***

Copyright 2023, SV Foster. All rights reserved.

License:
    This program is free for personal, educational and/or non-profit usage    

Revision History:

***/

using System;
using System.Reflection;
using System.Diagnostics;


namespace sBackup
{
    class TCommandLineInterface
    {
        public static void WorkModeGet(in string[] args, ref TGlobalOptions glo)
        {
            glo.Mode = TGlobalOptions.OperatingMode.Help;
            if (args.Length < 1)
                return;

            for (int i = 0; i < args.Length; ++i)
            {
                if (string.Equals(args[i], "/?", StringComparison.CurrentCultureIgnoreCase))
                    return;

                if (string.Equals(args[i], "/help", StringComparison.CurrentCultureIgnoreCase))
                    return;
            }

            if (string.Equals(args[0], "b", StringComparison.CurrentCultureIgnoreCase))
            {
                glo.Mode = TGlobalOptions.OperatingMode.Backup;
                return;
            }

            throw new TCommandLineInterfaceBadWorkModeException();
        }

        public static void SwitchesGet(in string[] args, ref TGlobalOptions glo)
        {
            for (int i = 1; i < args.Length; i++)
            {
                bool LengthError = false;


                if (string.Equals(args[i], "/NoCopyrightLogo", StringComparison.CurrentCultureIgnoreCase))
                {
                    glo.NoCopyrightLogo = true;
                    continue;
                }

                if (string.Equals(args[i], "/SavePath", StringComparison.CurrentCultureIgnoreCase))
                    if ((i + 1) < args.Length)
                    {
                        glo.SavePath = args[i + 1];
                        ++i;
                        continue;
                    }
                    else
                        LengthError = true;

                if (string.Equals(args[i], "/ProfileName", StringComparison.CurrentCultureIgnoreCase))
                    if ((i + 1) < args.Length)
                    {
                        glo.TasksFile = args[i + 1] + ".xml";
                        ++i;
                        continue;
                    }
                    else
                        LengthError = true;


                if (LengthError)
                    throw new TCommandLineInterfaceSwitchNoValueException(args[i]);

                if ((args[i].Length >= 1) && (args[i][0] == '/'))
                    throw new TCommandLineInterfaceInvalidSwitchException(args[i]);

                throw new TCommandLineInterfaceInvalidSwitchParamException(args[i]);
            }
        }

        public static void LogoPrint()
        {
            // Get the version of the current running app
            Assembly assembly = Assembly.GetExecutingAssembly();
            Version version = assembly.GetName().Version;

            // Get the versionInfo from resources
            var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);

            Console.WriteLine(String.Format
            (
                Properties.Resources.UIMSG_123_CMD_LOGO,

                versionInfo.FileDescription,
                version.Major,
                version.Minor,
                versionInfo.LegalCopyright
            ));
        }

        public static void HelpPrint()
        {
            Console.WriteLine(string.Format(Properties.Resources.UIMSG_124_HELP_TEXT));
        }

        public static void ProgramStartOperationPrint(TGlobalOptions.OperatingMode Mode)
        {
            switch (Mode)
            {
                case TGlobalOptions.OperatingMode.Backup:
                    Console.WriteLine(Properties.Resources.UIMSG_125_START_OM_BACKUP);
                    break;

                default:
                    break;
            }

        }

        public static void ProgramExitSummaryPrint()
        {
            Console.WriteLine(Properties.Resources.UIMSG_126_WORK_DONE_NOERROR);
        }

        public static void SetModeUTF16()
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
        }
    }


    class TCommandLineInterfaceBadWorkModeException : Exception
    {
        public TCommandLineInterfaceBadWorkModeException()
            { }
    }

    class TCommandLineInterfaceSwitchNoValueException : Exception
    {
        public TCommandLineInterfaceSwitchNoValueException(in string message)
            : base(message) { }
    }

    class TCommandLineInterfaceInvalidSwitchException : Exception
    {
        public TCommandLineInterfaceInvalidSwitchException(in string message)
            : base(message) { }
    }

    class TCommandLineInterfaceInvalidSwitchParamException : Exception
    {
        public TCommandLineInterfaceInvalidSwitchParamException(in string message)
            : base(message) { }
    }
}
