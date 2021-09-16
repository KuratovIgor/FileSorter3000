using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FileSorter.Sorts;

namespace FileSorter.Sorts
{ 
    public class FSorter
    {
        private ISort _sortObject = null;

        public FSorter(ISort sortObject)
        {
            _sortObject = sortObject;
        }

        public void Sort(DirectoryInfo currentDirectory)
        {
            _sortObject.Sort(currentDirectory);
        }
    }
}
