using System.Threading;
using NetLib.WPF;
using ReactiveUI;

namespace WpfApp1
{
    public class DialogViewModel : BaseViewModel
    {
        public DialogViewModel()
        {
            TestHide = CreateCommand(TestHideExec);
        }

        public ReactiveCommand TestHide { get; set; }

        private void TestHideExec()
        {
            using (HideWindow())
            {
                Thread.Sleep(1000);
            }
        }
    }
}
