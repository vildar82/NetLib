namespace NetLib.WPF
{
    using System;
    using System.Reactive;
    using JetBrains.Annotations;
    using ReactiveUI;

    [PublicAPI]
    public interface IBaseModel
    {
        IBaseViewModel BaseVm { get; set; }
        
        ReactiveCommand<Unit, Unit> CreateCommand(Action execute, [CanBeNull] IObservable<bool> canExecute = null);
        
        ReactiveCommand<T, Unit> CreateCommand<T>(Action<T> execute, [CanBeNull] IObservable<bool> canExecute = null);
        
        void ShowMessage(string msg, string title = "Ошибка");
    }
}