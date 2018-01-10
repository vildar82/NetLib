using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
