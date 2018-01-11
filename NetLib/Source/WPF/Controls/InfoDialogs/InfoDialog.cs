using JetBrains.Annotations;
using NetLib.WPF.Controls.InfoDialogs;

// ReSharper disable once CheckNamespace
namespace NetLib.WPF.Controls
{
    [PublicAPI]
    public class InfoDialog
    {
        public static bool ShowDialog(string title, string msg)
        {
            var infoDlgVM = new InfoDialogViewModel(title, msg);
            var infoDlg = new InfoDialogView(infoDlgVM);
            return infoDlg.ShowDialog() == true;
        }
    }
}