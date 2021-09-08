﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Media;

namespace FileSorter
{
    public class CatalogItem
    {
        private DirectoryInfo _directory = null;
        private FileInfo _file = null;

        public string Name { get; set; }
        public string TypeItem { get; set; }
        public Brush Color { get; set; }

        public CatalogItem(DirectoryInfo directory)
        {
            _directory = directory;
            Name = directory.Name;
            TypeItem = "Folder";
            Color = Brushes.LemonChiffon;
        }

        public CatalogItem(FileInfo file)
        {
            _file = file;
            Name = file.Name;
            TypeItem = "File";
        }

        public object GetCatalogItem()
        {
            if (_directory != null)
                return _directory;

            if (_file != null)
                return _file;

            throw new InvalidOperationException();
        }
    }
}