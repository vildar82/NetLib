using System;
using System.Reactive.Linq;
using System.Threading;
using NetLib;
using NetLib.WPF;
using NetLib.WPF.Controls.Progress;
using ReactiveUI;

namespace WpfApp1
{
    public class MainViewModel : BaseViewModel
    {
        private FileWatcherRx watcher;
        private string text;
        private IDisposable r;

        public MainViewModel()
        {
            Test = CreateCommand(TestExec);
            watcher = new FileWatcherRx(@"c:\Users\khisyametdinovvt\AppData\Roaming\PIK\UE\R2\Tasks\Out", "*.json");
            watcher.Created.Subscribe(s =>
            {
                ShowMessage($"Created - {s.EventArgs.FullPath}");
            });
            watcher.Changed.Throttle(TimeSpan.FromMilliseconds(1000)).Subscribe(s =>
            {
                ShowMessage($"Changed - {s.EventArgs.FullPath}");
            });
            r = this.WhenAnyValue(v => v.Text).Subscribe(s => UpdateText());
        }

        private void UpdateText()
        {
            Text2 = Text;
        }

        public ReactiveCommand Test { get; set; }

        public string Text2{ get; set; }
        public string Text{ get; set; }
        //{
        //    get => text;
        //    set {
        //        text = value;
        //        OnPropertyChanged();
        //    }
        //}

        private void TestExec()
        {
            ProgressView.Execute(() => { Thread.Sleep(3000); });
        }
    }
}