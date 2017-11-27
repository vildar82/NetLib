namespace NetLib.WPF.Controls.InfoDialogs
{
    internal class InfoDialogViewModel : BaseViewModel
    {
        public InfoDialogViewModel(string title, string msg)
        {
            Title = title;
            Message = msg;
        }

        public string Title { get; }
        public string Message { get; }
    }
}
