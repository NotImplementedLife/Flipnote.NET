using System;
using System.Collections.Generic;
using System.IO;
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

namespace FlipnoteDesktop.Controls
{
    /// <summary>
    /// Interaction logic for FileSelector.xaml
    /// </summary>
    public partial class FileSelector : UserControl
    {
        public FileSelector()
        {
            InitializeComponent();
        }        
                                            

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            
        }


        private bool _IsRoot = false;
        public bool IsRoot
        {
            get => _IsRoot;
            set
            {
                _IsRoot = value;
                ParentFolderButton.Visibility = _IsRoot ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public void ResetFolders(bool isRoot = false)
        {
            IsRoot = isRoot;
            FoldersList.Children.RemoveRange(1, FoldersList.Children.Count - 1);
        }

        private string _Path = "";

        public string Path
        {
            get => _Path;
            set
            {
                if (Directory.Exists(value))
                {
                    _Path = value;
                    ConfirmButton.IsEnabled = true;
                    GetFoldersAndFiles();
                }
                else ConfirmButton.IsEnabled = false;
            }
        }

        public void GetDrives()
        {
            ResetFolders(true);
            DriveInfo[] drives = DriveInfo.GetDrives();
            for (int i = 0, cnt = drives.Count(); i < cnt; i++)
            {
                var drive = new Button();
                drive.Style = FindResource("Folder") as Style;
                drive.Content = drives[i].Name;
                drive.Click += FolderClick;
                FoldersList.Children.Add(drive);
            }
        }

        public string FileExtension = "*.ppm";

        public void GetFoldersAndFiles()
        {
            try
            {
                ResetFolders();
                string[] dirs = Directory.GetDirectories(Path);
                for (int i = 0, cnt = dirs.Count(); i < cnt; i++)
                {
                    var dir = new Button();
                    dir.Style = FindResource("Folder") as Style;
                    dir.Content = System.IO.Path.GetFileName(dirs[i]);
                    dir.Click += FolderClick;
                    FoldersList.Children.Add(dir);
                }
                string[] files = Directory.EnumerateFiles(Path, FileExtension, SearchOption.TopDirectoryOnly).ToArray();
                for (int i = 0, cnt = files.Count(); i < cnt; i++)
                {
                    var file = new Button();
                    file.Style = FindResource("File") as Style;
                    file.Content = System.IO.Path.GetFileName(files[i]);
                    file.Click += FileClick;
                    FoldersList.Children.Add(file);
                }
            }
            catch (Exception e)
            {                               
                ParentFolderButton_Click(null, null);
            }
        }

        Button SelectedFileButton = null;

        private void FileClick(object sender, RoutedEventArgs e)
        {
            Filename = (sender as Button).Content as string;
            ConfirmButton.IsEnabled = true;
            if(SelectedFileButton!=null)
            {
                SelectedFileButton.Tag = null;                
            }
            SelectedFileButton = sender as Button;
            SelectedFileButton.Tag = "Selected";
        }

        public string Filename = "";

        public void FolderClick(object o, EventArgs e)
        {
            string rel = (o as Button).Content as string; ;
            if (IsRoot) Path = rel;
            else Path = System.IO.Path.Combine(Path, rel);
            ConfirmButton.IsEnabled = false;
        }

        private void ParentFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var parent = Directory.GetParent(Path);
            if (parent == null)
            {
                GetDrives();
                return;
            }
            Path = parent.ToString();
            ConfirmButton.IsEnabled = false;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            ConfirmRequest?.Invoke(this);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            BackRequest?.Invoke(this);
        }

        public delegate void OnConfirmRequest(object sender);
        public event OnConfirmRequest ConfirmRequest;

        public delegate void OnBackRequest(object sender);
        public event OnBackRequest BackRequest;

        public static DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(FileSelector),
            new PropertyMetadata("Choose a file", new PropertyChangedCallback(MessagePropertyChanged)));

        private static void MessagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as FileSelector).MessageControl.Text = e.NewValue as string;
        }

        public string Message
        {
            get => GetValue(MessageProperty) as string;
            set => SetValue(MessageProperty, value);
        }
    }
}
