using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace NetLib.WPF.Controls.Progress
{
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
