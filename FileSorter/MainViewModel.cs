using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FileSorter.Sorts;
using System.Diagnostics;

namespace FileSorter
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public ObservableCollection<DriveInfo> DriveCollection { get; set; } = new ObservableCollection<DriveInfo> { };
        public ObservableCollection<CatalogItem> DirectoryCollection { get; set; } = new ObservableCollection<CatalogItem> { };

        public DirectoryInfo _currentDirectory = null;

        private string _pathDir;
        public string PathDir
        {
            get => _pathDir;
            set
            {
                _pathDir = value;
                OnPropertyChanged("PathDir");
            }
        }

        #region Selections
        private DriveInfo _drive;
        private CatalogItem _catalogItem;

        public DriveInfo SelectedDrive
        {
            get => _drive;
            set
            {
                _drive = value;

                ListOfDrives_SelectionChanged();

                OnPropertyChanged("SelectedDrive");
            }
        }
        public CatalogItem SelectedCatalogItem
        {
            get => _catalogItem;
            set
            {
                _catalogItem = value;

                if (_catalogItem != null)
                    MoveToDirectory();

                OnPropertyChanged("SelectedCatalogItem");
            }
        }
        #endregion

        #region Comands
        private RelayCommand updateDriveList;
        private RelayCommand backCommand;
        private RelayCommand sortExtensionCommand;
        private RelayCommand sortAlphaviteCommand;
        private RelayCommand sortDateCommand;
        public RelayCommand UpdateDriveList { get => updateDriveList ?? (updateDriveList = new RelayCommand(obj => UpdateDriveCollection())); }
        public RelayCommand BackCommand { get => backCommand ?? (backCommand = new RelayCommand(obj => Back())); }
        public RelayCommand SortExtensionCommand { get => sortExtensionCommand ?? (sortExtensionCommand = new RelayCommand(obj => Sort(new SortExtension()))); }
        public RelayCommand SortAlphaviteCommand { get => sortAlphaviteCommand ?? (sortAlphaviteCommand = new RelayCommand(obj => Sort(new SortAlphavite()))); }
        public RelayCommand SortDateCommand { get => sortDateCommand ?? (sortDateCommand = new RelayCommand(obj => Sort(new SortDate()))); }
        #endregion

        public MainViewModel()
        {
            UpdateDriveCollection();
        }

        private void UpdateDriveCollection()
        {
            DriveCollection.Clear();
            ObservableCollection<DriveInfo> newCollection = DirectoryNavigator.PullOutDrives();
            foreach (var item in newCollection)
            {
                DriveCollection.Add(item);
            }
        }

        private void Back()
        {
            DirectoryInfo directory = new DirectoryInfo(_currentDirectory.FullName);

            if (directory.Parent != null)
            {
                List<CatalogItem> newCollection = DirectoryNavigator.MoveBack(directory);

                UpdateDirectoryCollection(newCollection);

                _currentDirectory = new DirectoryInfo(directory.Parent.FullName);
                PathDir = _currentDirectory.FullName;
            }
        }

        public void UpdateDirectoryCollection(List<CatalogItem> collection)
        {
            DirectoryCollection.Clear();

            foreach (var item in collection)
            {
                DirectoryCollection.Add(item);
            }
        }

        private void Sort(ISort sortObject)
        {
            FSorter sorter = new FSorter(sortObject);
            sorter.Sort(_currentDirectory);

            List<CatalogItem> catalogItems = DirectoryNavigator.GetItemsOfCatalog(_currentDirectory);

            UpdateDirectoryCollection(catalogItems);
        }

        private void ListOfDrives_SelectionChanged()
        {
            DirectoryInfo directory = new DirectoryInfo(_drive.ToString());
            List<CatalogItem> newCollection = DirectoryNavigator.MoveNext(directory);

            UpdateDirectoryCollection(newCollection);

            PathDir = directory.FullName;
            _currentDirectory = new DirectoryInfo(directory.FullName);  
        }

        private void MoveToDirectory()
        {
            try
            {
                if ((_catalogItem.GetCatalogItem() as DirectoryInfo) != null)
                {
                    DirectoryInfo directory = new DirectoryInfo((_catalogItem.GetCatalogItem() as DirectoryInfo).FullName);

                    _currentDirectory = new DirectoryInfo(directory.FullName);
                    PathDir = _currentDirectory.FullName;

                    List<CatalogItem> newCollection = DirectoryNavigator.MoveNext(directory);

                    UpdateDirectoryCollection(newCollection);
                }
                else
                {
                    FileInfo file = new FileInfo((_catalogItem.GetCatalogItem() as FileInfo).FullName);

                    Process.Start(file.FullName);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Отказано в доступе");
            }
        }
    }
}
