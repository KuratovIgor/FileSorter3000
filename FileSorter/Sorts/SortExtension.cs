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
        private List<FileInfo> _files = null;
        private readonly List<DirectoryInfo> _newDirectories = new List<DirectoryInfo>();
        private DirectoryInfo _currentDirectory = null;

        public void Sort(DirectoryInfo currentDirectory)
        {
            _currentDirectory = currentDirectory;

            _files = _currentDirectory.GetFiles().ToList();
            CreateNewDirectories();

            MoveFilesToDirectories();
        }

        private void CreateNewDirectories()
        {
            bool isExist = false;

            foreach (var file in _files)
            {
                DirectoryInfo newDir = new DirectoryInfo(_currentDirectory.FullName + $@"\{file.Extension}");

                foreach (var directory in _newDirectories)
                {
                    if (directory.FullName == newDir.FullName)
                    {
                        isExist = true;
                        break;
                    }
                }

                if (!isExist)
                    _newDirectories.Add(newDir);

                isExist = false;
            }
        }

        private void MoveFilesToDirectories()
        {
            foreach (var directory in _newDirectories)
            {
                directory.Create();

                foreach (var file in _files)
                {
                    if (file.Extension == directory.Name)
                            file.MoveTo(directory.FullName + $@"\{file.Name}");
                }
            }
        }
    }
}
