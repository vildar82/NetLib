namespace NetLib.WPF
{
    using System;
    using System.ComponentModel;
    using System.Reactive;
    using System.Reactive.Concurrency;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Threading;
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using NLog;
    using ReactiveUI;

    /// <summary>
    /// Базовый класс модели - MVVM
    /// </summary>
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

        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public UserControl UserControl { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public IBaseViewModel BaseVm { get; set; }

        public ReactiveCommand<Unit, Unit> CreateCommand(Action execute, IObservable<bool>? canExecute = null)
        {
            if (BaseVm != null) return BaseVm.CreateCommand(execute, canExecute);
            var command = ReactiveCommand.Create(execute, canExecute, new DispatcherScheduler(Dispatcher.CurrentDispatcher));
            command.ThrownExceptions.Subscribe(CommandException);
            return command;
        }

        public ReactiveCommand<T, Unit> CreateCommand<T>(Action<T> execute, IObservable<bool>? canExecute = null)
        {
            if (BaseVm != null) return BaseVm.CreateCommand<T>(execute, canExecute);
            var command = ReactiveCommand.Create(execute, canExecute, new DispatcherScheduler(Dispatcher.CurrentDispatcher));
            command.ThrownExceptions.Subscribe(CommandException);
            return command;
        }

        public ReactiveCommand<Unit, Unit> CreateCommandAsync(Func<CancellationToken,Task> execute, IObservable<bool>? canExecute = null)
        {
            if (BaseVm != null) return BaseVm.CreateCommandAsync(execute, canExecute);
            var command = ReactiveCommand.CreateFromTask(execute, canExecute, new DispatcherScheduler(Dispatcher.CurrentDispatcher));
            command.ThrownExceptions.Subscribe(CommandException);
            return command;
        }

        public ReactiveCommand<Unit, Unit> CreateCommandAsync(Func<Task> execute, IObservable<bool>? canExecute = null)
        {
            if (BaseVm != null) return BaseVm.CreateCommandAsync(execute, canExecute);
            var command = ReactiveCommand.CreateFromTask(execute, canExecute, new DispatcherScheduler(Dispatcher.CurrentDispatcher));
            command.ThrownExceptions.Subscribe(CommandException);
            return command;
        }

        public ReactiveCommand<TParam, Unit> CreateCommandAsync<TParam>(Func<TParam, Task> execute, IObservable<bool>? canExecute = null)
        {
            if (BaseVm != null) return BaseVm.CreateCommandAsync(execute, canExecute);
            var command = ReactiveCommand.CreateFromTask(execute, canExecute, new DispatcherScheduler(Dispatcher.CurrentDispatcher));
            command.ThrownExceptions.Subscribe(CommandException);
            return command;
        }

        public ReactiveCommand<TParam, Unit> CreateCommandAsync<TParam>(Func<TParam, CancellationToken, Task> execute, IObservable<bool>? canExecute = null)
        {
            if (BaseVm != null) return BaseVm.CreateCommandAsync(execute, canExecute);
            var command = ReactiveCommand.CreateFromTask(execute, canExecute, new DispatcherScheduler(Dispatcher.CurrentDispatcher));
            command.ThrownExceptions.Subscribe(CommandException);
            return command;
        }

        public ReactiveCommand<TParam, TResult> CreateCommandAsync<TParam, TResult>(Func<TParam, CancellationToken, Task<TResult>> execute, IObservable<bool> canExecute = null)
        {
            var command = ReactiveCommand.CreateFromTask(execute, canExecute, new DispatcherScheduler(Dispatcher.CurrentDispatcher));
            command.ThrownExceptions.Subscribe(CommandException);
            return command;
        }

        public ReactiveCommand<Unit, TResult> CreateCommandAsync<TResult>(Func<Task<TResult>> execute, IObservable<bool> canExecute = null)
        {
            var command = ReactiveCommand.CreateFromTask(execute, canExecute, new DispatcherScheduler(Dispatcher.CurrentDispatcher));
            command.ThrownExceptions.Subscribe(CommandException);
            return command;
        }

        public ReactiveCommand<Unit, TResult> CreateCommandAsync<TResult>(Func<CancellationToken, Task<TResult>> execute, IObservable<bool> canExecute = null)
        {
            var command = ReactiveCommand.CreateFromTask(execute, canExecute, new DispatcherScheduler(Dispatcher.CurrentDispatcher));
            command.ThrownExceptions.Subscribe(CommandException);
            return command;
        }

        public void CommandException(Exception e)
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

        /// <summary>
        /// Запрос ответа у пользователя с утвердительным или отрицательным ответом
        /// </summary>
        /// <param name="title">Заголовок</param>
        /// <param name="msg">Вопрос</param>
        /// <param name="affirmativeBtn">Надпись для кнопки утвердительного ответа</param>
        /// <param name="negateBtn">Надпись для кнопки отрицательного ответа</param>
        /// <param name="auxiliaryBtn">Дополнительноая кнопка (возвращается null)</param>
        /// <returns></returns>
        public Task<bool?> ShowMessage(string title, string msg, string affirmativeBtn, string negateBtn,
            string? auxiliaryBtn = null)
        {
            if (BaseVm == null) throw new Exception($"Не задана BaseViewModel - {this}.");
            return BaseVm.ShowMessage(msg, title, affirmativeBtn, negateBtn, auxiliaryBtn);
        }

        public virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.RaisePropertyChanged(propertyName);
        }

        public IDisposable HideWindow()
        {
            return BaseVm.HideWindow();
        }
    }
}
