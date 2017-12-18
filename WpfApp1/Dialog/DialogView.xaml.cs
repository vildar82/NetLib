using WpfApp1.Dialog;

namespace PIK_GP_Civil.Parkings.Area
{
    /// <summary>
    /// Interaction logic for DialogView.xaml
    /// </summary>
    public partial class DialogView
    {
        public DialogView(DialogVM vm) : base(vm)
        {
            InitializeComponent();
            IsUnclosing = true;
        }
    }
}
