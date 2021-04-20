namespace NetLib.WPF
{
    using System;
    using System.Reactive;
    using ReactiveUI;

    public interface IBaseModel
    {
        IBaseViewModel BaseVm { get; set; }
        
        ReactiveCommand<Unit, Unit> CreateCommand(Action execute, IObservable<bool>? canExecute = null);
        
        ReactiveCommand<T, Unit> CreateCommand<T>(Action<T> execute, IObservable<bool>? canExecute = null);
        
        void ShowMessage(string msg, string title = "Ошибка");
    }
}