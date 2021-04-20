namespace NetLib.WPF
{
    using System;
    using System.ComponentModel;
    using System.Reactive;
    using System.Threading;
    using System.Threading.Tasks;
    using ReactiveUI;

    public interface IBaseViewModel : INotifyDataErrorInfo, IDisposable
    {
        bool? DialogResult { get; set; }
        
        bool Hide { get; set; }
        
        IBaseViewModel Parent { get; set; }
        
        BaseWindow Window { get; set; }

        void CommandException(Exception e);

        ReactiveCommand<Unit, Unit> CreateCommand(Action execute, IObservable<bool>? canExecute = null);

        ReactiveCommand<T, Unit> CreateCommand<T>(Action<T> execute, IObservable<bool>? canExecute = null);

        ReactiveCommand<Unit, Unit> CreateCommandAsync(Func<CancellationToken, Task> execute, IObservable<bool>? canExecute = null);

        ReactiveCommand<Unit, Unit> CreateCommandAsync(Func<Task> execute, IObservable<bool>? canExecute = null);

        ReactiveCommand<TParam, Unit> CreateCommandAsync<TParam>(Func<TParam, Task> execute, IObservable<bool>? canExecute = null);

        ReactiveCommand<TParam, Unit> CreateCommandAsync<TParam>(Func<TParam, CancellationToken, Task> execute, IObservable<bool>? canExecute = null);

        void HideMe();

        IDisposable HideWindow();

        void OnActivated();

        void OnClosed();

        void OnClosing();

        void OnInitialize();

        void ShowMessage(string msg, string title = "Ошибка");

        Task<bool?> ShowMessage(string title, string msg, string affirmativeBtn, string negateBtn, string? auxiliaryBtn = null);

        void VisibleMe();
    }
}