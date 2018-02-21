using NetLib.Errors.UI.ViewModel;

namespace NetLib.Errors.UI.View
{
    /// <summary>
    /// Interaction logic for ErrorsView.xaml
    /// </summary>
    public partial class ErrorsView
    {
        public ErrorsView(ErrorsViewModel errorsVM) : base (errorsVM)
        {
            InitializeComponent();
        }
    }
}
