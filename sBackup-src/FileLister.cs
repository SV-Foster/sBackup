/***

Copyright 2023, SV Foster. All rights reserved.

License:
    This program is free for personal, educational and/or non-profit usage    

Revision History:

***/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace sBackup
{
    class TFileLister
    {
        public List<string> Path { get; }
        public bool DepthLimitApplyed { get; }
        public uint Depth { get; }


        public TFileLister(in string[] Path, bool DepthLimitApplyed = false, uint Depth = 0)
        {
            this.Path = new List<string>(Path.Length);
            this.Path.AddRange(Path);
            this.DepthLimitApplyed = DepthLimitApplyed;
            this.Depth = Depth;
        }

        public void ListFiles(in string OutputFile)
        {
            using (StreamWriter writer = new StreamWriter(OutputFile, false, Encoding.Unicode))
            {
                foreach (var n in this.Path)
                {
                    string PathExtended = Environment.ExpandEnvironmentVariables(n);
                    ListFilesRecursive(PathExtended, writer, this.DepthLimitApplyed, this.Depth);
                }
            }
        }

        private void ListFilesRecursive(in string directoryPath, in StreamWriter writer, bool DepthLimitApplyed, uint depthlim, uint depth = 0)
        {
            if (!Directory.Exists(directoryPath))
            {
                throw new TFileListerInvalidPathException(directoryPath);
            }
            string[] files;

            try
            {
                files = Directory.GetFileSystemEntries(directoryPath);
            }
            catch (UnauthorizedAccessException ex)
            {
                writer.WriteLine( String.Format(Properties.Resources.UIMSG_113_FILE_SKIPPED_EXPORT, ex.Message));
                Console.WriteLine(String.Format(Properties.Resources.UIMSG_113_FILE_SKIPPED_EXPORT, ex.Message));
                return;
            }

            foreach (string file in files)
            {
                writer.WriteLine(file);

                if (!(!DepthLimitApplyed || (depth < depthlim)))
                    continue;

                if (!Directory.Exists(file))
                    continue;

                ListFilesRecursive(file, writer, DepthLimitApplyed, depthlim, depth + 1);
            }
        }

    }
    class TFileListerInvalidPathException : Exception
    {
        public TFileListerInvalidPathException(in string message)
            : base(message) { }
    }
}
