using NetLib.WPF;
using ReactiveUI;

namespace WpfApp1
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            ShowDialog = CreateCommand(ShowDialogExec);
        }

        public ReactiveCommand ShowDialog { get; set; }

        private void ShowDialogExec()
        {
            var dialogView = new DialogView(new DialogViewModel());
            dialogView.ShowDialog();
        }
    }
}
