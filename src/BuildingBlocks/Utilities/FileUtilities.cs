using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{

    /// <summary>
    /// Warning - will erase files and subdirectories from target directory
    /// </summary>
    public static class FileUtilities
    {
        public static void Empty(this System.IO.DirectoryInfo directory)
        {
            foreach (System.IO.FileInfo file in directory.GetFiles()) file.Delete();
            foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
        }

    }
}
