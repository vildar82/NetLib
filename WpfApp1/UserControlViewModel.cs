using NetLib.WPF;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.Threading;

namespace WpfApp1
{
    public class UserControlViewModel : BaseViewModel
    {
        public ReactiveCommand TestProgress { get; set; }

        public UserControlViewModel(MainViewModel parent) : base(parent)
        {
            TestProgress = CreateCommand(TestProgressExec);
        }

        private void TestProgressExec()
        {
            Debug.WriteLine($"{Thread.CurrentThread.ManagedThreadId} TestProgressExec start");
            ShowProgressDialog(c =>
            {
                c.SetIndeterminate();
                Debug.WriteLine($"{Thread.CurrentThread.ManagedThreadId} TestProgressExec progress start");
                for (var i = 0; i < 100000000; i++)
                {
                    // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                    Math.Sqrt(i);
                    // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                    Math.Sqrt(i / (double)i);
                    // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                    Math.Sqrt(i * i);
                }
                Debug.WriteLine($"{Thread.CurrentThread.ManagedThreadId} TestProgressExec progress end");
            }, "title", "message", false);
            Debug.WriteLine($"{Thread.CurrentThread.ManagedThreadId} TestProgressExec end");
        }
    }
}