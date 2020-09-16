namespace Test
{
    using System.Windows.Input;
    using NetLib.WPF;

    public class MainVm : BaseViewModel
    {
        public MainVm()
        {
            TestCommand = CreateCommand(TestCommandExec);
        }

        public ICommand TestCommand { get; set; }

        private void TestCommandExec()
        {
            ShowMessage("Tet");
        }
    }
}