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

            List<DirectoryInfo> newDirectories = CreateNewDirectories(currentDirectory, catalogItems, Months);

            MoveCatalogItemsToDirectories(catalogItems, newDirectories, Months);
        }
    }
}
