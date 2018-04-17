using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Media;
using NetLib;
using NetLib.WPF;
using NetLib.WPF.Controls.Progress;
using ReactiveUI;

namespace WpfApp1
{
    public class MainViewModel : BaseViewModel
    {
        private FileWatcherRx watcher;

        public MainViewModel()
        {
            Test = CreateCommand(TestExec);
            watcher = new FileWatcherRx(@"c:\Users\khisyametdinovvt\AppData\Roaming\PIK\UE\R2\Tasks\Out", "*.json",
                NotifyFilters.LastWrite, WatcherChangeTypes.Changed);
            watcher.Created.Subscribe(s =>
            {
                ShowMessage($"Created - {s.EventArgs.FullPath}");
            });
            watcher.Changed.Throttle(TimeSpan.FromMilliseconds(1000)).Subscribe(s =>
            {
                ShowMessage($"Changed - {s.EventArgs.FullPath}");
            });
        }

        public ReactiveCommand Test { get; set; }

        private void TestExec()
        {
            ProgressView.Execute(() => { Thread.Sleep(3000); });
        }
    }
}