using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;
using NLog;
using ReactiveUI;

namespace NetLib.WPF
{
    public class RxDefaultExceptionHandler : IObserver<Exception>
    {
        private static Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        public RxDefaultExceptionHandler()
        {
            Debug.WriteLine($"RxDefaultExceptionHandler ctor");
        }

        public void OnNext(Exception value)
        {
            if (Debugger.IsAttached) Debugger.Break();

            RxApp.MainThreadScheduler.Schedule(() =>
            {
                Logger.Error(value);
            });
        }

        public void OnError(Exception error)
        {
            if (Debugger.IsAttached) Debugger.Break();

            RxApp.MainThreadScheduler.Schedule(() =>
            {
                Logger.Error(error);
            });
        }

        public void OnCompleted()
        {
            if (Debugger.IsAttached) Debugger.Break();
            RxApp.MainThreadScheduler.Schedule(() =>
            {
                throw new NotImplementedException();
            });
        }
    }
}
