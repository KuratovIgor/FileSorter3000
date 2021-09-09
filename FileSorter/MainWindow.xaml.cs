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

namespace FileSorter
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<DriveInfo> DriveCollection { get; set; } = new ObservableCollection<DriveInfo> { };
        public ObservableCollection<CatalogItem> DriveDirectoryCollection { get; set; } = new ObservableCollection<CatalogItem> { };
        private List<CatalogItem> _catalogItems = new List<CatalogItem> { };
        private DriveInfo[] _drives = null;
        private DirectoryInfo _driveDirectory = null;

        public MainWindow()
        {
            InitializeComponent();

            PullOutDrives();
        }

        private void updateDriveListBt_Click(object sender, RoutedEventArgs e)
        {
            DriveCollection.Clear();
            PullOutDrives();
        }

        private void PullOutDrives()
        {
            _drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in _drives)
            {
                DriveCollection.Add(drive);
            }
        }

        private void listOfDrives_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DriveDirectoryCollection.Clear();
            _driveDirectory = new DirectoryInfo(listOfDrives.SelectedValue.ToString());
            
            GetItemsOfCatalog();
         
            foreach(var item in _catalogItems)
            {
                DriveDirectoryCollection.Add(item);
            }

            path.Text = _driveDirectory.FullName;
        }

        private void GetItemsOfCatalog()
        {
            _catalogItems.Clear();
            foreach (var item in _driveDirectory.GetDirectories())
            {
                _catalogItems.Add(new CatalogItem(item));
            }
            foreach (var item in _driveDirectory.GetFiles())
            {
                _catalogItems.Add(new CatalogItem(item));
            }
        }
    }
}
