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
        public DriveInfo[] _drives = null;
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
    }
}
