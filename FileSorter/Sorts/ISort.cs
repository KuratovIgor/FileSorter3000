using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileSorter.Sorts
{
    public abstract class ISort
    {
        public abstract void Sort(DirectoryInfo currentDirectory);

        internal List<DirectoryInfo> CreateNewDirectories(DirectoryInfo currentDirectory, List<FileInfo> files)
        {
            List<DirectoryInfo> newDirectories = new List<DirectoryInfo> { };
            bool isExist = false;

            foreach (var file in files)
            {
                DirectoryInfo newDir = new DirectoryInfo(currentDirectory.FullName + $@"\{file.Extension}");

                foreach (var directory in newDirectories)
                {
                    if (directory.FullName == newDir.FullName)
                    {
                        isExist = true;
                        break;
                    }
                }

                if (!isExist)
                    newDirectories.Add(newDir);

                isExist = false;
            }

            return newDirectories;
        }

        internal List<DirectoryInfo> CreateNewDirectories(DirectoryInfo currentDirectory, List<CatalogItem> catalogItems, Dictionary<int, string> Months = null)
        {
            List<DirectoryInfo> newDirectories = new List<DirectoryInfo> { };
            bool isExist = false;

            foreach (var item in catalogItems)
            {
                DirectoryInfo newDir;
                if (Months == null)
                    newDir = new DirectoryInfo(currentDirectory.FullName + $@"\{item.Name[0]}");
                else
                    newDir = new DirectoryInfo(currentDirectory.FullName + $@"\{Months[item.CreationTime.Month]} {item.CreationTime.Year}");

                foreach (var directory in newDirectories)
                {
                    if (Months == null) 
                    {
                        if (directory.Name[0] == newDir.Name[0])
                        {
                            isExist = true;
                            break;
                        }
                    }
                    else if (directory.Name == newDir.Name)
                    {
                        isExist = true;
                        break;
                    }

                }

                if (!isExist)
                    newDirectories.Add(newDir);

                isExist = false;
            }

            return newDirectories;
        }

        internal void MoveFilesToDirectories(List<FileInfo> files, List<DirectoryInfo> newDirectories)
        {
            foreach (var directory in newDirectories)
            {
                directory.Create();

                foreach (var file in files)
                {
                    if (file.Extension == directory.Name)
                        file.MoveTo(directory.FullName + $@"\{file.Name}");
                }
            }
        }

        internal void MoveCatalogItemsToDirectories(List<CatalogItem> catalogItems, List<DirectoryInfo> newDirectories, Dictionary<int, string> Months = null)
        {
            foreach (var directory in newDirectories)
            {
                directory.Create();

                foreach (var item in catalogItems)
                {
                    TryMoveCatalogItem(item, directory, Months);
                }
            }
        }

        internal void TryMoveCatalogItem(CatalogItem item, DirectoryInfo directory, Dictionary<int, string> Months = null)
        {
            try
            {
                DirectoryInfo dir = item.GetCatalogItem() as DirectoryInfo;

                if (dir != null)
                {
                    if (Months == null)
                    {
                        if (dir.Name[0] == Convert.ToChar(directory.Name))
                            dir.MoveTo(directory.FullName + $@"\{item.Name}");
                    }
                    else 
                    {
                        if (Months[dir.CreationTime.Month] + " " + item.CreationTime.Year == directory.Name)
                            dir.MoveTo(directory.FullName + $@"\{item.Name}");
                    }
                }
                else
                {
                    FileInfo file = item.GetCatalogItem() as FileInfo;

                    if (Months == null)
                    {
                        if (file.Name[0] == Convert.ToChar(directory.Name))
                            file.MoveTo(directory.FullName + $@"\{item.Name}");
                    }
                    else
                    {
                        if (Months[file.CreationTime.Month] + " " + item.CreationTime.Year == directory.Name)
                            file.MoveTo(directory.FullName + $@"\{item.Name}");
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
