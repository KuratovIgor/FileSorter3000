using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSorter
{
    public static class DirectoryNavigator
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

        public static List<CatalogItem> MoveNext(DirectoryInfo directory)
        {
            _directory = new DirectoryInfo(directory.FullName);

            List<CatalogItem> newCollection = GetNewDirectoryCollection();

            return newCollection;
        }

        public static List<CatalogItem> MoveBack(DirectoryInfo directory)
        {
            _directory = new DirectoryInfo(directory.Parent.FullName);

            List<CatalogItem> newCollection = GetNewDirectoryCollection();

            return newCollection;
        }

        private static List<CatalogItem> GetNewDirectoryCollection()
        {
            NewCatalogItems.Clear();
            NewCatalogItems = GetItemsOfCatalog(_directory);

            List<CatalogItem> newCollection = new List<CatalogItem> { };
            foreach (var item in NewCatalogItems)
            {
                newCollection.Add(item);
            }

            return newCollection;
        }

        public static bool IsFileException(DirectoryInfo item)
        {
            if (item.Name[0] != '$' && item.Name[0] != '~' && item.Extension != ".sys" &&
                item.Extension != ".tmp" && item.Extension != ".Msi")
                return false;

            return true;
        }

        public static bool IsFileException(FileInfo item)
        {
            if (item.Name[0] != '$' && item.Name[0] != '~' && item.Extension != ".sys" &&
                item.Extension != ".tmp" && item.Extension != ".Msi")
                return false;

            return true;
        }
    }
}
