using System;
using JetBrains.Annotations;
using ReactiveUI;

namespace NetLib.WPF
{
    [PublicAPI]
    public interface IBaseModel
    {
        IBaseViewModel BaseVm { get; set; }
        ReactiveCommand CreateCommand(Action execute, [CanBeNull] IObservable<bool> canExecute = null);
        ReactiveCommand CreateCommand<T>(Action<T> execute, [CanBeNull] IObservable<bool> canExecute = null);
        void ShowMessage(string msg, string title = "Ошибка");
    }
}