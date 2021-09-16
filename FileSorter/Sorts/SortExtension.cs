using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSorter.Sorts
{
    public class SortExtension : ISort
    {
        public override void Sort(DirectoryInfo currentDirectory)
        {
            List<FileInfo> files = currentDirectory.GetFiles().ToList();
            List<DirectoryInfo> newDirectories = CreateNewDirectories(currentDirectory, files);

            MoveFilesToDirectories(files, newDirectories);
        }
    }
}
