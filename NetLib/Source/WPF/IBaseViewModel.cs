using ReactiveUI;
using System;
using System.ComponentModel;

namespace NetLib.WPF
{
    public interface IBaseViewModel : INotifyDataErrorInfo, IDisposable
    {
        bool? DialogResult { get; set; }
        bool Hide { get; set; }
        IBaseViewModel Parent { get; set; }
        BaseWindow Window { get; set; }

        ReactiveCommand AddCommand(ReactiveCommand command);
        ReactiveCommand CreateCommand(Action execute, IObservable<bool> canExecute = null);
        void CommandException(Exception e);
        IDisposable HideWindow();
        void OnClosed();
        void OnInitialize();
        void ShowMessage(string msg, string title = "Ошибка");
    }
}