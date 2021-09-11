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

        private static void GetItemsOfCatalog()
        {
            foreach (var item in _directory.GetDirectories())
            {
                if (!IsFileException(item))
                    NewCatalogItems.Add(new CatalogItem(item));
            }
            foreach (var item in _directory.GetFiles())
            {
                if (!IsFileException(item))
                    NewCatalogItems.Add(new CatalogItem(item));
            }
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
            GetItemsOfCatalog();

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
    }
}
