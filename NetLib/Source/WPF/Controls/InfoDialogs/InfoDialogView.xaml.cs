using System.Windows;

namespace NetLib.WPF.Controls.InfoDialogs
{
    /// <summary>
    /// Interaction logic for InfoDialogView.xaml
    /// </summary>
    internal partial class InfoDialogView
    {
        public InfoDialogView(InfoDialogViewModel infoDlgVM) : base(infoDlgVM)
        {
            InitializeComponent();
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
