using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSorter
{
    public static class DirectoryMover
    {
        private static List<CatalogItem> NewCatalogItems = new List<CatalogItem>{};
        private static DirectoryInfo _directory = null;

        public static List<CatalogItem> GetItemsOfCatalog(DirectoryInfo directory)
        {
            List<CatalogItem> newCatalogItems = new List<CatalogItem> { };

            foreach (var item in directory.GetDirectories())
            {
                if (!IsFileException(item))
                    newCatalogItems.Add(new CatalogItem(item));
            }
            foreach (var item in directory.GetFiles())
            {
                if (!IsFileException(item))
                    newCatalogItems.Add(new CatalogItem(item));
            }

            return newCatalogItems;
        }

        public static ObservableCollection<DriveInfo> PullOutDrives()
        {
            ObservableCollection<DriveInfo> driveCollection = new ObservableCollection<DriveInfo> { };

            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                driveCollection.Add(drive);
            }

            return driveCollection;
        }

        public static ObservableCollection<CatalogItem> MoveNext(DirectoryInfo directory)
        {
            _directory = new DirectoryInfo(directory.FullName);

            ObservableCollection<CatalogItem> newCollection = GetNewDirectoryCollection();

            return newCollection;
        }

        public static ObservableCollection<CatalogItem> MoveBack(DirectoryInfo directory)
        {
            _directory = new DirectoryInfo(directory.Parent.FullName);

            ObservableCollection<CatalogItem> newCollection = GetNewDirectoryCollection();

            return newCollection;
        }

        private static ObservableCollection<CatalogItem> GetNewDirectoryCollection()
        {
            NewCatalogItems.Clear();
            NewCatalogItems = GetItemsOfCatalog(_directory);

            ObservableCollection<CatalogItem> newCollection = new ObservableCollection<CatalogItem> { };
            foreach (var item in NewCatalogItems)
            {
                if (!IsFileException(item))
                    newCollection.Add(item);
            }

            return newCollection;
        }

        public static bool IsFileException(CatalogItem item)
        {
            if (item.Name[0] != '$' && item.Name[0] != '~' && item.Extension.ToString() != ".sys" &&
                item.Extension.ToString() != ".tmp" && item.Extension.ToString() != ".Msi")
                return false;

            return true;
        }

        public static bool IsFileException(DirectoryInfo item)
        {
            if (item.Name[0] != '$' && item.Name[0] != '~' && item.Extension.ToString() != ".sys" &&
                item.Extension.ToString() != ".tmp" && item.Extension.ToString() != ".Msi")
                return false;

            return true;
        }

        public static bool IsFileException(FileInfo item)
        {
            if (item.Name[0] != '$' && item.Name[0] != '~' && item.Extension.ToString() != ".sys" &&
                item.Extension.ToString() != ".tmp" && item.Extension.ToString() != ".Msi")
                return false;

            return true;
        }

        public static void SortExtension(DirectoryInfo currentDirectory)
        {
            List<FileInfo> files = currentDirectory.GetFiles().ToList();
            List<DirectoryInfo> newDirectories = new List<DirectoryInfo> { };

            bool isExist = false;

            foreach (var file in files)
            {
                DirectoryInfo dir = new DirectoryInfo(currentDirectory.FullName + $@"\{file.Extension}");

                foreach (var directory in newDirectories)
                {
                    if (directory.FullName == dir.FullName)
                    {
                        isExist = true;
                        break;
                    }
                }

                if (!isExist)
                    newDirectories.Add(dir);

                isExist = false;
            }

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
    }
}
