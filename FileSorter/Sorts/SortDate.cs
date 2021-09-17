using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileSorter.Sorts
{
    public class SortDate : ISort
    {
        public static Dictionary<int, string> Months = new Dictionary<int, string>
        {
            { 1, "Январь" },
            { 2, "Февраль" },
            { 3, "Март" },
            { 4, "Апрель" },
            { 5, "Май" },
            { 6, "Июнь" },
            { 7, "Июль" },
            { 8, "Август" },
            { 9, "Сентябрь" },
            { 10, "Октябрь" },
            { 11, "Ноябрь" },
            { 12, "Декабрь" },
        };

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
                DirectoryInfo newDir = new DirectoryInfo(_currentDirectory.FullName + $@"\{Months[item.CreationTime.Month]} {item.CreationTime.Year}");

                foreach (var directory in _newDirectories)
                {
                    if (directory.Name == newDir.Name)
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
                    if (Months[dir.CreationTime.Month] + " " + item.CreationTime.Year == directory.Name)
                        dir.MoveTo(directory.FullName + $@"\{item.Name}");
                }
                else
                {
                    FileInfo file = item.GetCatalogItem() as FileInfo;

                    if (Months[file.CreationTime.Month] + " " + item.CreationTime.Year == directory.Name)
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
