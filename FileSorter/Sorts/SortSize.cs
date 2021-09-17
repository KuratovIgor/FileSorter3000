using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FileSorter.Sorts
{
    public class SortSize : ISort
    {
        private long _verySmallSize = 16 * 1024;
        private long _smallSize = 1024 * 1024;
        private long _middleSize = 128 * 1024 * 1024;
        private long _bigSize = 1024 * 1024 * 1024;

        private readonly Dictionary<string, string> FileSize = new Dictionary<string, string>
        {
            {"VerySmall", "0 - 16 Kb" },
            {"Small", "16 Kb - 1 Mb" },
            {"Middle", "1 - 128 Mb" },
            {"Big", "128 Mb - 1 Gb" },
            {"VeryBig", "1+ Gb" },
        };

        private List<FileInfo> _files = null;
        private readonly List<DirectoryInfo> _newDirectories = new List<DirectoryInfo>();
        private DirectoryInfo _currentDirectory;

        public void Sort(DirectoryInfo currentDirectory)
        {
            _currentDirectory = currentDirectory;

            _files = _currentDirectory.GetFiles().ToList();

            CreateNewDirectories();

            MoveFilesToDirectories();
        }

        private void CreateNewDirectories()
        {
            foreach (var key in FileSize.Keys)
                _newDirectories.Add(new DirectoryInfo(_currentDirectory.FullName + $@"\{FileSize[key]}"));
        }

        private void MoveFilesToDirectories()
        {
            foreach (var file in _files)
            {
                if (0 < file.Length && file.Length <= _verySmallSize)
                {
                    if (!_newDirectories[0].Exists)
                        _newDirectories[0].Create();
                    file.MoveTo(_newDirectories[0].FullName + $@"\{file.Name}");
                }

                if (_verySmallSize < file.Length && file.Length <= _smallSize)
                {
                    if (!_newDirectories[1].Exists)
                        _newDirectories[1].Create();
                    file.MoveTo(_newDirectories[1].FullName + $@"\{file.Name}");
                }

                if (_smallSize < file.Length && file.Length <= _middleSize)
                {
                    if (!_newDirectories[2].Exists)
                        _newDirectories[2].Create();
                    file.MoveTo(_newDirectories[2].FullName + $@"\{file.Name}");
                }

                if (_middleSize < file.Length && file.Length <= _bigSize)
                {
                    if (!_newDirectories[3].Exists)
                        _newDirectories[3].Create();
                    file.MoveTo(_newDirectories[3].FullName + $@"\{file.Name}");
                }

                if (_bigSize < file.Length)
                {
                    if (!_newDirectories[4].Exists)
                        _newDirectories[4].Create();
                    file.MoveTo(_newDirectories[4].FullName + $@"\{file.Name}");
                }
            }
        }
    }
}
