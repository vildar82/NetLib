using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Media;
using NetLib.WPF;
using NetLib.WPF.Controls.Progress;
using ReactiveUI;

namespace WpfApp1
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            Test = CreateCommand(TestExec);
        }

        public ReactiveCommand Test { get; set; }

        private void TestExec()
        {
            ProgressView.Execute(() => { Thread.Sleep(3000); });
        }
    }
}