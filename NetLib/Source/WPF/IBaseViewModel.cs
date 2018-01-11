using ReactiveUI;
using System;
using System.ComponentModel;
using JetBrains.Annotations;

namespace NetLib.WPF
{
    [PublicAPI]
    public interface IBaseViewModel : INotifyDataErrorInfo, IDisposable
    {
        bool? DialogResult { get; set; }
        bool Hide { get; set; }
        IBaseViewModel Parent { get; set; }
        BaseWindow Window { get; set; }

        ReactiveCommand AddCommand(ReactiveCommand command);

        void CommandException(Exception e);

        ReactiveCommand CreateCommand(Action execute, IObservable<bool> canExecute = null);

        void HideMe();

        IDisposable HideWindow();

        void OnClosed();

        void OnInitialize();

        void ShowMessage(string msg, string title = "Ошибка");

        void VisibleMe();
    }
}