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
    public interface ISort
    {
        void Sort(DirectoryInfo currentDirectory);
    }
}
