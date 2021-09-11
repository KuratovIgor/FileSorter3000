using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Media;

namespace FileSorter
{
    public class CatalogItem : INotifyPropertyChanged
    {
        private DirectoryInfo _directory = null;
        private FileInfo _file = null;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public string Name { get; set; }
        public Brush _color;
        public Brush Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                OnPropertyChanged(nameof(Color));
            }
        }
        public string Extension { get; set; }
        public DateTime CreationTime { get; set; }

        public CatalogItem(DirectoryInfo directory)
        {
            _directory = directory;
            Name = directory.Name;
            Color = Brushes.LightGray;
            Extension = directory.Extension;
            CreationTime = directory.CreationTime;
        }

        public CatalogItem(FileInfo file)
        {
            _file = file;
            Name = file.Name;
            Extension = file.Extension;
            CreationTime = file.CreationTime;
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
