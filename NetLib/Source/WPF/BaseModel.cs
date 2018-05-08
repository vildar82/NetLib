using System;
using System.Runtime.CompilerServices;
using System.Windows;
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
