using ReactiveUI;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
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

        ReactiveCommand CreateCommand(Action execute, [CanBeNull] IObservable<bool> canExecute = null);
        ReactiveCommand CreateCommand<T>(Action<T> execute, [CanBeNull] IObservable<bool> canExecute = null);
        ReactiveCommand CreateCommandAsync(Func<CancellationToken, Task> execute, [CanBeNull] IObservable<bool> canExecute = null);
        ReactiveCommand CreateCommandAsync(Func<Task> execute, [CanBeNull] IObservable<bool> canExecute = null);
        ReactiveCommand CreateCommandAsync<TParam>(Func<TParam, Task> execute,[CanBeNull] IObservable<bool> canExecute = null);
        ReactiveCommand CreateCommandAsync<TParam>(Func<TParam, CancellationToken, Task> execute,[CanBeNull] IObservable<bool> canExecute = null);
        
        void HideMe();

        IDisposable HideWindow();

        void OnActivated();

        void OnClosed();

        void OnClosing();

        void OnInitialize();

        void ShowMessage(string msg, string title = "Ошибка");

        void VisibleMe();
    }
}