using NetLib.WPF;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.Threading;

namespace WpfApp1
{
    public class MainViewModel : BaseViewModel
    {
        public ReactiveCommand ShowDialog { get; set; }

        public ReactiveCommand ShowWindow1 { get; set; }
        public ReactiveCommand TestProgress { get; set; }

        public UserControlViewModel UserControl { get; set; }

        public MainViewModel()
        {
            UserControl = new UserControlViewModel(this);
            ShowDialog = CreateCommand(ShowDialogExec);
            TestProgress = CreateCommand(TestProgressExec);
            ShowWindow1 = CreateCommand(ShowWindow1Exec);
        }

        private void ShowDialogExec()
        {
            var dialogView = new DialogView(new DialogViewModel());
            dialogView.ShowDialog();
        }

        private void ShowWindow1Exec()
        {
            var win = new Window1();
            win.Show();
        }

        private async void TestProgressExec()
        {
            // ReSharper disable once NotAccessedVariable
            var res = await ShowMessage("Test", "Msg", "Yes", "No");
            // ReSharper disable once RedundantAssignment
            res = await ShowMessage("Test", "Msg", "Yes", "No", "Cancel");
            Debug.WriteLine($"{Thread.CurrentThread.ManagedThreadId} TestProgressExec start");
            await ShowProgressDialog(c =>
            {
                c.SetIndeterminate();
                Debug.WriteLine($"{Thread.CurrentThread.ManagedThreadId} TestProgressExec progress start");
                for (var i = 0; i < 200000000; i++)
                {
                    if (i % 10000000 == 0) DoEvents();
                    // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                    Math.Sqrt(i);
                    // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                    Math.Sqrt(i / (double)i);
                    // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                    Math.Sqrt(i * i);
                }
                Debug.WriteLine($"{Thread.CurrentThread.ManagedThreadId} TestProgressExec progress end");
            }, "Заголовок", "Сообщение", false);
            Debug.WriteLine($"{Thread.CurrentThread.ManagedThreadId} TestProgressExec end");
        }
    }
}