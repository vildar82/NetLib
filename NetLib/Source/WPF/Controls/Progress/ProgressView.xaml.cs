namespace NetLib.WPF.Controls.Progress
{
    using System;
    using System.ComponentModel;
    using System.Threading;
    using System.Windows.Threading;

    /// <summary>
    /// Interaction logic for ProgressView.xaml
    /// </summary>
    public partial class ProgressView : IDisposable
    {
        private BackgroundWorker worker;

        public ProgressView() : base(new ProgressVM())
        {
            InitializeComponent();
        }

        internal void InternalExecute(Action operation)
        {
            worker = new BackgroundWorker {WorkerSupportsCancellation = true};
            worker.DoWork += (s, e) =>
            {
                operation();
                Thread.Sleep(500);
            };
            worker.RunWorkerCompleted +=
                (s, e) =>
                {
                    Dispatcher.BeginInvoke(DispatcherPriority.Send, (SendOrPostCallback) delegate { Close(); }, null);
                };
            worker.RunWorkerAsync();
            ShowDialog();
        }

        public static void Execute(Action operation)
        {
            var progressView = new ProgressView();
			progressView.InternalExecute(operation);
        }

        public void Dispose()
        {
            Close();
        }
    }

    public class ProgressVM : BaseViewModel
    {
    }
}
