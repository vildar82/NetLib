namespace Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
            : base(new MainVm())
        {
            InitializeComponent();
        }
    }
}