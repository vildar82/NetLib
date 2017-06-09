using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace NetLib.WPF.Controls
{
	/// <summary>
	/// Interaction logic for PathSelector.xaml
	/// </summary>
	public partial class PathSelector : UserControl
	{
		public PathSelector()
		{
			InitializeComponent();
		}

		private void BrowseButton_Click(object sender, RoutedEventArgs e)
		{
			var fileDialog = new OpenFileDialog()
			{
				Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
			};
			if (fileDialog.ShowDialog() == true)
			{
				SelectedPathTxtBox.Text = fileDialog.FileName;
			}
		}
		
		public string SelectedPath
		{
			get { return (string)GetValue(SelectedPathProperty); }
			set { SetValue(SelectedPathProperty, value); }
		}

		public static readonly DependencyProperty SelectedPathProperty =
			DependencyProperty.Register(
			"SelectedPath",
			typeof(string),
			typeof(PathSelector),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(SelectedPathChanged))
			{
				BindsTwoWayByDefault = true,
				DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
			});

		private static void SelectedPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((PathSelector)d).SelectedPathTxtBox.Text = e.NewValue.ToString();			
		}		

		private void SelectedPathTxtBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			SelectedPath = SelectedPathTxtBox.Text;
		}

		private void OpenExplorer(object sender, RoutedEventArgs e)
		{
			var file = SelectedPath;			
			if (!File.Exists(file))
			{
				MessageBox.Show("Путь не найден.");
				return;
			}			
			string argument = "/select, \"" + file + "\"";
			Process.Start("explorer.exe", argument);
		}
	}
}
