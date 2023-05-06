using System;
using System.Collections.Generic;
using System.Text;

namespace SharedUtilities.Utilties
{

    /// <summary>
    /// Warning - will erase files and subdirectories from target directory
    /// </summary>
    public static class FileUtilities
    {
        public static void Empty(this DirectoryInfo directory)
        {
            foreach (FileInfo file in directory.GetFiles()) file.Delete();
            foreach (DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
        }

    }
}
