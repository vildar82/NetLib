using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace NetLib.WPF.Controls
{
    /// <summary>
    /// Interaction logic for PathSelector.xaml
    /// </summary>
    public partial class PathSelector
    {
        public PathSelector()
        {
            InitializeComponent();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                SelectedPathTxtBox.Text = fileDialog.FileName;
            }
        }

        public string SelectedPath
        {
            get => (string)GetValue(SelectedPathProperty);
            set => SetValue(SelectedPathProperty, value);
        }

        public static readonly DependencyProperty SelectedPathProperty =
            DependencyProperty.Register(
            "SelectedPath",
            typeof(string),
            typeof(PathSelector),
            new FrameworkPropertyMetadata(SelectedPathChanged)
            {
                BindsTwoWayByDefault = true,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });

        private static void SelectedPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PathSelector)d).SelectedPathTxtBox.Text = e.NewValue.ToString();
        }

        private void SelectedPathTxtBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            SelectedPath = SelectedPathTxtBox.Text;
        }

        private void OpenExplorer(object sender, RoutedEventArgs e)
        {
            var path = SelectedPathTxtBox.Text;
            if (!FileOrDirectoryExists(path))
            {
                MessageBox.Show("Путь не найден.");
                return;
            }
            string argument = "/select, \"" + path + "\"";
            Process.Start("explorer.exe", argument);
        }

        internal static bool FileOrDirectoryExists(string name)
        {
            return (Directory.Exists(name) || File.Exists(name));
        }


    }
}
