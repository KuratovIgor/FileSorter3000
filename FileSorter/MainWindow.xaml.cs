using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;

namespace FileSorter
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<DriveInfo> DriveCollection { get; set; } = new ObservableCollection<DriveInfo> { };
        public ObservableCollection<CatalogItem> DirectoryCollection { get; set; } = new ObservableCollection<CatalogItem> { };
        private List<CatalogItem> _catalogItems = new List<CatalogItem> { };
        private DriveInfo[] _drives = null;
        private DirectoryInfo _currentDirectory = null;

        public MainWindow()
        {
            InitializeComponent();

            ObservableCollection<DriveInfo> newCollection = DirectoryMover.PullOutDrives();
            foreach (var item in newCollection)
            {
                DriveCollection.Add(item);
            }
        }

        private void updateDriveListBt_Click(object sender, RoutedEventArgs e)
        {
            DriveCollection.Clear();
            ObservableCollection<DriveInfo> newCollection = DirectoryMover.PullOutDrives();
            foreach (var item in newCollection)
            {
                DriveCollection.Add(item);
            }
        }

        private void listOfDrives_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DirectoryCollection.Clear();
            DirectoryInfo directory = new DirectoryInfo(listOfDrives.SelectedValue.ToString());
            ObservableCollection<CatalogItem> newCollection = DirectoryMover.MoveNext(directory);

            foreach(var item in newCollection)
            {
                if (!DirectoryMover.IsFileException(item))
                    DirectoryCollection.Add(item);
            }

            path.Text = directory.FullName;
            _currentDirectory = new DirectoryInfo(directory.FullName);
        }

        private void CatalogList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (catalogList.SelectedValue != null)
                MoveToDirectory();
        }

        private void MoveToDirectory()
        {
            try
            {
                CatalogItem catalogItem = catalogList.SelectedValue as CatalogItem;

                if ((catalogItem.GetCatalogItem() as DirectoryInfo) != null)
                {
                    CatalogItem catalog = catalogList.SelectedValue as CatalogItem;
                    DirectoryInfo directory = new DirectoryInfo((catalog.GetCatalogItem() as DirectoryInfo).FullName);

                    _currentDirectory = new DirectoryInfo(directory.FullName);
                    path.Text = _currentDirectory.FullName;

                    ObservableCollection<CatalogItem> newCollection = DirectoryMover.MoveNext(directory);

                    UpdateDirectoryCollection(newCollection);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Отказано в доступе");
            }
        }

        private void UpdateDirectoryCollection(ObservableCollection<CatalogItem> collection)
        {
            ObservableCollection<CatalogItem> newCollection = collection;

            foreach (var item in DirectoryCollection)
            {
                if (!DirectoryMover.IsFileException(item))
                    newCollection.Add(item);
            }

            foreach (var item in newCollection)
            {
                if (DirectoryCollection.Contains(item))
                    DirectoryCollection.Remove(item);
                else if (!DirectoryMover.IsFileException(item))
                    DirectoryCollection.Add(item);
            }
        }

        private void BackBt_OnClick(object sender, RoutedEventArgs e)
        {
            DirectoryCollection.Clear();
            DirectoryInfo directory = new DirectoryInfo(_currentDirectory.FullName);

            if (directory.Parent != null)
            {
                ObservableCollection<CatalogItem> newCollection = DirectoryMover.MoveBack(directory);

                foreach (var item in newCollection)
                {
                    if (!DirectoryMover.IsFileException(item))
                        DirectoryCollection.Add(item);
                }

                _currentDirectory = new DirectoryInfo(directory.Parent.FullName);
                path.Text = _currentDirectory.FullName;
            }
        }

        private void SortExtensionBt_OnClick(object sender, RoutedEventArgs e)
        {
            DirectoryMover.SortExtension(_currentDirectory);

            DirectoryCollection.Clear();

            List<CatalogItem> catalogItems = DirectoryMover.GetItemsOfCatalog(_currentDirectory);

            foreach (var item in catalogItems)
            {
                DirectoryCollection.Add(item);
            }
        }
    }
}
