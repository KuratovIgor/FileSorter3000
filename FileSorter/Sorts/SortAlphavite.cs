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
        public override void Sort(DirectoryInfo currentDirectory)
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

            List<DirectoryInfo> newDirectories = CreateNewDirectories(currentDirectory, catalogItems);

            MoveCatalogItemsToDirectories(catalogItems, newDirectories);
        }
    }
}
