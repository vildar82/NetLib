using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using JetBrains.Annotations;
using NLog;
using ReactiveUI;

namespace NetLib.WPF
{
    /// <summary>
    /// Базовый класс модели - MVVM
    /// </summary>
    [PublicAPI]
    public abstract class BaseModel : ReactiveObject, IBaseModel
    {
        protected static Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        protected BaseModel()
        {
            
        }

        protected BaseModel(IBaseViewModel baseVM)
        {
            BaseVm = baseVM;
        }

        protected BaseModel(UserControl userControl, IBaseViewModel baseVM)
        {
            UserControl = userControl;
            BaseVm = baseVM;
        }

        public UserControl UserControl { get; set; }
        public IBaseViewModel BaseVm { get; set; }

        [NotNull]
        public ReactiveCommand CreateCommand(Action execute, IObservable<bool> canExecute = null)
        {
             if (BaseVm != null) return BaseVm.CreateCommand(execute, canExecute);
            var command = ReactiveCommand.Create(execute, canExecute);
            command.ThrownExceptions.Subscribe(CommandException);
            return command;
        }
        
        [NotNull]
        public ReactiveCommand CreateCommand<T>(Action<T> execute, IObservable<bool> canExecute = null)
        {
            if (BaseVm != null) return BaseVm.CreateCommand(execute, canExecute);
            var command = ReactiveCommand.Create(execute, canExecute);
            command.ThrownExceptions.Subscribe(CommandException);
            return command;
        }

        [NotNull]
        public ReactiveCommand CreateCommandAsync(Func<CancellationToken,Task> execute, [CanBeNull] IObservable<bool> canExecute = null)
        {
            if (BaseVm != null) return BaseVm.CreateCommandAsync(execute, canExecute);
            var command = ReactiveCommand.CreateFromTask(execute, canExecute);
            command.ThrownExceptions.Subscribe(CommandException);
            return command;
        }

        [NotNull]
        public ReactiveCommand CreateCommandAsync(Func<Task> execute, [CanBeNull] IObservable<bool> canExecute = null)
        {
            if (BaseVm != null) return BaseVm.CreateCommandAsync(execute, canExecute);
            var command = ReactiveCommand.CreateFromTask(execute, canExecute);
            command.ThrownExceptions.Subscribe(CommandException);
            return command;
        }

        [NotNull]
        public ReactiveCommand CreateCommandAsync<TParam>(Func<TParam, Task> execute, [CanBeNull] IObservable<bool> canExecute = null)
        {
            if (BaseVm != null) return BaseVm.CreateCommandAsync(execute, canExecute);
            var command = ReactiveCommand.CreateFromTask(execute, canExecute);
            command.ThrownExceptions.Subscribe(CommandException);
            return command;
        }
        [NotNull]
        public ReactiveCommand CreateCommandAsync<TParam>(Func<TParam, CancellationToken, Task> execute, [CanBeNull] IObservable<bool> canExecute = null)
        {
            if (BaseVm != null) return BaseVm.CreateCommandAsync(execute, canExecute);
            var command = ReactiveCommand.CreateFromTask(execute, canExecute);
            command.ThrownExceptions.Subscribe(CommandException);
            return command;
        }

        public void CommandException([NotNull] Exception e)
        {
            if (e is OperationCanceledException) return;
            Logger.Error(e, "CommandException");
            ShowMessage(e.Message);
        }

        public void ShowMessage(string msg, string title = "Ошибка")
        {
            if (BaseVm != null) BaseVm.ShowMessage(msg, title);
            else MessageBox.Show(msg, title);
        }

        public virtual void OnPropertyChanged([CanBeNull] [CallerMemberName] string propertyName = null)
        {
            this.RaisePropertyChanged(propertyName);
        }
    }
}
