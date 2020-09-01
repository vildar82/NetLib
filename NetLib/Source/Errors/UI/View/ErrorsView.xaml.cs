namespace NetLib.Errors.UI.View
{
    using ViewModel;

    /// <summary>
    /// Interaction logic for ErrorsView.xaml
    /// </summary>
    public partial class ErrorsView
    {
        public ErrorsView(ErrorsViewModel errorsVM)
            : base (errorsVM)
        {
            InitializeComponent();
        }
    }
}
