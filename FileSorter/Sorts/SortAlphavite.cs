using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileSorter.Sorts
{
    public class SortAlphavite : ISort
    {
        private DirectoryInfo _currentDirectory = null;
        private readonly List<CatalogItem> _catalogItems = null;
        private readonly List<DirectoryInfo> _newDirectories = null;

        public void Sort(DirectoryInfo currentDirectory)
        {
            _currentDirectory = currentDirectory;

            List<DirectoryInfo> directories = _currentDirectory.GetDirectories().ToList();
            List<FileInfo> files = _currentDirectory.GetFiles().ToList();

            foreach (var item in directories)
            {
                _catalogItems.Add(new CatalogItem(item));
            }

            foreach (var item in files)
            {
                _catalogItems.Add(new CatalogItem(item));
            }

            CreateNewDirectories();

            MoveCatalogItemsToDirectories();
        }

        private void CreateNewDirectories()
        {
            bool isExist = false;

            foreach (var item in _catalogItems)
            {
                DirectoryInfo newDir = new DirectoryInfo(_currentDirectory.FullName + $@"\{item.Name[0]}");

                foreach (var directory in _newDirectories)
                {
                    if (directory.Name[0] == newDir.Name[0])
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

        private void MoveCatalogItemsToDirectories()
        {
            foreach (var directory in _newDirectories)
            {
                directory.Create();

                foreach (var item in _catalogItems)
                {
                    TryMoveCatalogItem(item, directory);
                }
            }
        }

        private void TryMoveCatalogItem(CatalogItem item, DirectoryInfo directory)
        {
            try
            {
                DirectoryInfo dir = item.GetCatalogItem() as DirectoryInfo;

                if (dir != null)
                {
                    if (dir.Name[0] == Convert.ToChar(directory.Name))
                        dir.MoveTo(directory.FullName + $@"\{item.Name}");
                }
                else
                {
                    FileInfo file = item.GetCatalogItem() as FileInfo;

                    if (file.Name[0] == Convert.ToChar(directory.Name))
                        file.MoveTo(directory.FullName + $@"\{item.Name}");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
