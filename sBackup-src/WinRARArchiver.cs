/***

Copyright 2023, SV Foster. All rights reserved.

License:
    This program is free for personal, educational and/or non-profit usage    

Revision History:

***/

using System;
using System.Collections.Generic;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;


namespace sBackup
{
    class TWinRARArchiver
    {
        public enum TCompressionType
        {
            None,
            Max
        }

        private const string ExecutableName = "rar.exe";
        private const string RegistryPath = @"SOFTWARE\WinRAR";
        private const string RegistryVal = "exe64";

        public static string InstallFolder { get; private set; }
        public string ExeFullPath { get { return (InstallFolder + ExecutableName); }}
        public string CommandLineParams { get { return CommandLineParamsForm(); }}
        public string ArchivePath { get; set; }
        public List<string> InputPathList { get; } = new List<string>();
        public bool Recursive { get; set; } = true;
        public List<string> ExcludeList { get; } = new List<string>();
        public TCompressionType Compression { get; set; } = TCompressionType.Max;
        

        public TWinRARArchiver()
        {
            // on first init, read where the WinRAR is installed
            if (String.IsNullOrEmpty(InstallFolder))
            {
                RegistryKey localKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);

                RegistryKey SubKey = localKey.OpenSubKey(RegistryPath, false);
                if (SubKey == null)
                    throw new TWinRARArchiverNotInstalledException(TWinRARArchiverNotInstalledException.TQualification.Registry, RegistryPath, "");

                Object Value = SubKey.GetValue(RegistryVal);
                if (Value == null)
                    throw new TWinRARArchiverNotInstalledException(TWinRARArchiverNotInstalledException.TQualification.Registry, $"{RegistryPath}\\{RegistryVal}", "");

                InstallFolder = Value as string;
                if (String.IsNullOrEmpty(InstallFolder))
                    throw new TWinRARArchiverNotInstalledException(TWinRARArchiverNotInstalledException.TQualification.Registry, $"{RegistryPath}\\{RegistryVal}", "");

                // IncludeTrailingBackslash()
                InstallFolder = Path.GetDirectoryName(InstallFolder).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;

                if (!File.Exists(ExeFullPath))
                    throw new TWinRARArchiverNotInstalledException(TWinRARArchiverNotInstalledException.TQualification.Executable, $"{RegistryPath}\\{RegistryVal}", ExeFullPath);
            }

        }

        public void StartAndWait()
        {
            // Create a new process start info
            ProcessStartInfo startInfo = new ProcessStartInfo(ExeFullPath, CommandLineParamsForm());

            // Redirect the standard output to display the output in the console
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;

            // Create a new process and start it
            Process process = new Process();
            process.StartInfo = startInfo;
            process.OutputDataReceived += (s, evt) => {
                if (evt.Data != null)
                {
                    Console.WriteLine(evt.Data);
                }
            };
            process.ErrorDataReceived += (s, evt) => {
                if (evt.Data != null)
                {
                    Console.WriteLine(evt.Data);
                }
            };

            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();

            // Wait for the process to exit
            process.WaitForExit();

            // Get the exit code
            if (process.ExitCode != 0)
                throw new TWinRARArchiverCallException(process.ExitCode);
        }

        private string CommandLineParamsForm()
        {
            // rar.exe a -scUR -r -ri1 -k -mt4 -m0 -rr5p -ts -x*\desktop.ini Pictures.rar D:\Pictures\
            const string template = @"a -scUR {0} -ri1 -k -mt4 {1} -md2g -rr5p -ts {2} {3} {4}";

            string Result = String.Format
            (
                template,
                this.Recursive ? "-r" : "",
                this.Compression == TCompressionType.None ? "-m0" : "-m5",
                CommandLineExcludes(),
                QuoteIfHasSpaces(Environment.ExpandEnvironmentVariables(this.ArchivePath)),
                CommandLineInputs()
            );

            return Result;
        }

        private string CommandLineExcludes()
        {
            string Result = "";

            foreach (var n in this.ExcludeList)
            {
                if (!String.IsNullOrEmpty(Result))
                    Result += " ";

                Result += "-x" + QuoteIfHasSpaces(Environment.ExpandEnvironmentVariables(n));
            }

            return Result;
        }

        private string CommandLineInputs()
        {
            string Result = "";

            foreach (var n in this.InputPathList)
            {
                if (!String.IsNullOrEmpty(Result))
                    Result += " ";

                Result += QuoteIfHasSpaces(Environment.ExpandEnvironmentVariables(n));
            }

            return Result;
        }

        private string QuoteIfHasSpaces(in string s)
        {
            if (s.Contains(" "))
                return string.Format("\"{0}\"", s);

            return s;
        }
    }

    class TWinRARArchiverNotInstalledException : Exception
    {
        public enum TQualification
        {
            Registry,
            Executable
        }

        public TQualification Qualification { get; private set; }
        public string RegPath { get; private set; }
        public string Executable { get; private set; }

        public TWinRARArchiverNotInstalledException(TQualification q, in string Reg, in string Exe)
        {
            this.Qualification = q;
            this.RegPath = Reg;
            this.Executable = Exe;
        }
    }

    class TWinRARArchiverCallException : Exception
    {
        public int ExitCode { get; private set; }

        public TWinRARArchiverCallException(int e)
        {
            this.ExitCode = e;
        }
    }
}
