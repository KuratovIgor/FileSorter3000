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
        private DirectoryInfo _currentDirectory = null;

        public MainWindow()
        {
            InitializeComponent();

            UpdateDriveCollection();
        }

        private void updateDriveListBt_Click(object sender, RoutedEventArgs e)
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

        private void listOfDrives_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DirectoryInfo directory = new DirectoryInfo(listOfDrives.SelectedValue.ToString());
            List<CatalogItem> newCollection = DirectoryNavigator.MoveNext(directory);

            UpdateDirectoryCollection(newCollection);

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

                    List<CatalogItem> newCollection = DirectoryNavigator.MoveNext(directory);

                    UpdateDirectoryCollection(newCollection);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Отказано в доступе");
            }
        }

        private void UpdateDirectoryCollection(List<CatalogItem> collection)
        {
            DirectoryCollection.Clear();

            foreach (var item in collection)
            {
                DirectoryCollection.Add(item);
            }
        }

        private void BackBt_OnClick(object sender, RoutedEventArgs e)
        {
            DirectoryInfo directory = new DirectoryInfo(_currentDirectory.FullName);

            if (directory.Parent != null)
            {
                List<CatalogItem> newCollection = DirectoryNavigator.MoveBack(directory);

                UpdateDirectoryCollection(newCollection);

                _currentDirectory = new DirectoryInfo(directory.Parent.FullName);
                path.Text = _currentDirectory.FullName;
            }
        }

        private void SortExtensionBt_OnClick(object sender, RoutedEventArgs e)
        {
            FSorter.SortExtension(_currentDirectory);

            List<CatalogItem> catalogItems = DirectoryNavigator.GetItemsOfCatalog(_currentDirectory);

            UpdateDirectoryCollection(catalogItems);
        }

        private void SortAlphaviteBt_OnClick(object sender, RoutedEventArgs e)
        {
            FSorter.SortAlphavite(_currentDirectory);

            List<CatalogItem> catalogItems = DirectoryNavigator.GetItemsOfCatalog(_currentDirectory);

            UpdateDirectoryCollection(catalogItems);
        }
    }
}
