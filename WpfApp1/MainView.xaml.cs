namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView
    {
        public MainView() : base (new MainViewModel())
        {
            InitializeComponent();
        }
    }
}