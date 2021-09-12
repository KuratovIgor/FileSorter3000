using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileSorter
{
    public static class FSorter
    {
        public static void SortExtension(DirectoryInfo currentDirectory)
        {
            List<FileInfo> files = currentDirectory.GetFiles().ToList();
            List<DirectoryInfo> newDirectories = CreateNewDirectoriesForExtensions(currentDirectory, files);

            MoveFilesToDirectories(files, newDirectories);
        }

        private static List<DirectoryInfo> CreateNewDirectoriesForExtensions(DirectoryInfo currentDirectory, List<FileInfo> files)
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

        private static void MoveFilesToDirectories(List<FileInfo> files, List<DirectoryInfo> newDirectories)
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

        public static void SortAlphavite(DirectoryInfo currentDirectory)
        {
            List<CatalogItem> catalogItems = new List<CatalogItem> { };

            List<DirectoryInfo> directories = currentDirectory.GetDirectories().ToList();
            List<FileInfo> files = currentDirectory.GetFiles().ToList();

            foreach (var item in directories)
            {
                catalogItems.Add(new CatalogItem(item));
            }

            foreach (var item in files)
            {
                catalogItems.Add(new CatalogItem(item));
            }

            List<DirectoryInfo> newDirectories = CreateNewDirectoriesForAlphavite(currentDirectory, catalogItems);

            MoveCatalogItemsToDirectories(catalogItems, newDirectories);
        }

        private static List<DirectoryInfo> CreateNewDirectoriesForAlphavite(DirectoryInfo currentDirectory, List<CatalogItem> catalogItems)
        {
            List<DirectoryInfo> newDirectories = new List<DirectoryInfo> { };
            bool isExist = false;

            foreach (var item in catalogItems)
            {
                DirectoryInfo newDir = new DirectoryInfo(currentDirectory.FullName + $@"\{item.Name[0]}");

                foreach (var directory in newDirectories)
                {
                    if (directory.Name[0] == newDir.Name[0])
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

        private static void MoveCatalogItemsToDirectories(List<CatalogItem> catalogItems, List<DirectoryInfo> newDirectories)
        {
            foreach (var directory in newDirectories)
            {
                directory.Create();

                foreach (var item in catalogItems)
                {
                    TryMoveCatalogItem(item, directory);
                }
            }
        }

        private static void TryMoveCatalogItem(CatalogItem item, DirectoryInfo directory)
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
