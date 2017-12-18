using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using NetLib.WPF;
using PIK_GP_Civil.Parkings.Area;
using ReactiveUI;

namespace WpfApp1.Dialog
{
    public class DialogVM : BaseViewModel
    {
        public DialogVM()
        {
        }

        public DialogVM(IBaseViewModel parent) : base(parent)
        {
            Test = CreateCommand(TestExec);
        }

        public ReactiveCommand Test { get; set; }

        private void TestExec()
        {
            HideMe();
        }
    }
}
