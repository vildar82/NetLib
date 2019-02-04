namespace NetLib.WPF
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Concurrency;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Threading;
    using ControlzEx;
    using FluentValidation;
    using JetBrains.Annotations;
    using MahApps.Metro.Controls.Dialogs;
    using NLog;
    using ReactiveUI;
    using ValidationResult = FluentValidation.Results.ValidationResult;

    /// <inheritdoc cref="IBaseViewModel" />
    /// <summary>
    /// ReactiveUI ViewModel,
    /// FluentValidator (должен быть класс с таким же именем +Validator) -
    /// унаследованный от AbstractValidator
    /// </summary>
    [PublicAPI]
    public abstract class BaseViewModel : ReactiveObject, IBaseViewModel
    {
        private DateTime lastShowError;
        public readonly IDialogCoordinator dialogCoordinator = DialogCoordinator.Instance;
        protected IValidator validator;
        private static readonly HashSet<string> ignoreProps = new HashSet<string> { "Hide", "DialogResult", "Errors" };
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IValidator> validators =
            new ConcurrentDictionary<RuntimeTypeHandle, IValidator>();

        public static Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
        private readonly object context;
        private ValidationResult validationResult;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool? DialogResult { get; set; }

        public List<string> Errors { get; set; }

        public bool HasErrors => validationResult?.Errors?.Count > 0;

        [Obsolete("Use Hide()")]
        public bool Hide { get; set; }

        public bool IsValidated { get; private set; }

        public IBaseViewModel Parent { get; set; }

        public BaseWindow Window { get; set; }

        protected static Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        static BaseViewModel()
        {
            validators[typeof(BaseViewModel).TypeHandle] = null;
        }

        public BaseViewModel([CanBeNull] IBaseViewModel parent)
        {
            context = this;
            InitValidator();
            using (SuppressChangeNotifications())
            {
                Parent = parent;
                if (Window == null && parent is BaseViewModel parentVM)
                {
                    context = parentVM;
                    Window = parent.Window;
                    dialogCoordinator = parentVM.dialogCoordinator;
                }
                PropertyChanged += BaseViewModel_PropertyChanged;
                ThrownExceptions.Subscribe(CommandException);
            }
        }

        public BaseViewModel() : this(null)
        {
        }

        public void AddWindowButton(string toolTip, PackIconBase icon, Action onClick)
        {
            if (Window == null) return;
            var button = new Button
            {
                Content = icon,
                ToolTip = toolTip
            };
            button.Click += (o, s) => onClick();
            Window.AddWindowButton(button);
        }

        /// <summary>
        /// Прокачка очереди сообщений
        /// </summary>
        public static void DoEvents()
        {
            dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
        }

        public void CommandException([NotNull] Exception e)
        {
            if (e is OperationCanceledException)
                return;
            Logger.Error(e, "CommandException");
            ShowMessage(e.Message);
        }

        [NotNull]
        public ReactiveCommand<Unit, Unit> CreateCommandAsync(Func<CancellationToken, Task> execute, IObservable<bool> canExecute = null)
        {
            var command = ReactiveCommand.CreateFromTask(execute, canExecute, new DispatcherScheduler(Dispatcher.CurrentDispatcher));
            command.ThrownExceptions.Subscribe(CommandException);
            return command;
        }

        [NotNull]
        public ReactiveCommand<Unit, Unit> CreateCommandAsync(Func<Task> execute, IObservable<bool> canExecute = null)
        {
            var command = ReactiveCommand.CreateFromTask(execute, canExecute, new DispatcherScheduler(Dispatcher.CurrentDispatcher));
            command.ThrownExceptions.Subscribe(CommandException);
            return command;
        }

        [NotNull]
        public ReactiveCommand<TParam, Unit> CreateCommandAsync<TParam>(Func<TParam, Task> execute, IObservable<bool> canExecute = null)
        {
            var command = ReactiveCommand.CreateFromTask(execute, canExecute, new DispatcherScheduler(Dispatcher.CurrentDispatcher));
            command.ThrownExceptions.Subscribe(CommandException);
            return command;
        }

        [NotNull]
        public ReactiveCommand<TParam, Unit> CreateCommandAsync<TParam>(Func<TParam, CancellationToken, Task> execute, IObservable<bool> canExecute = null)
        {
            var command = ReactiveCommand.CreateFromTask(execute, canExecute, new DispatcherScheduler(Dispatcher.CurrentDispatcher));
            command.ThrownExceptions.Subscribe(CommandException);
            return command;
        }

        [NotNull]
        public ReactiveCommand<TParam, TResult> CreateCommandAsync<TParam, TResult>(Func<TParam, CancellationToken, Task<TResult>> execute, IObservable<bool> canExecute = null)
        {
            var command = ReactiveCommand.CreateFromTask(execute, canExecute, new DispatcherScheduler(Dispatcher.CurrentDispatcher));
            command.ThrownExceptions.Subscribe(CommandException);
            return command;
        }

        [NotNull]
        public ReactiveCommand<Unit, TResult> CreateCommandAsync<TResult>(Func<Task<TResult>> execute, IObservable<bool> canExecute = null)
        {
            var command = ReactiveCommand.CreateFromTask(execute, canExecute, new DispatcherScheduler(Dispatcher.CurrentDispatcher));
            command.ThrownExceptions.Subscribe(CommandException);
            return command;
        }

        [NotNull]
        public ReactiveCommand<Unit, Unit> CreateCommand(Action execute, IObservable<bool> canExecute = null)
        {
            var command = ReactiveCommand.Create(execute, canExecute, new DispatcherScheduler(Dispatcher.CurrentDispatcher));
            command.ThrownExceptions.Subscribe(CommandException);
            return command;
        }

        [NotNull]
        public ReactiveCommand<T, Unit> CreateCommand<T>(Action<T> execute, IObservable<bool> canExecute = null)
        {
            var command = ReactiveCommand.Create(execute, canExecute, new DispatcherScheduler(Dispatcher.CurrentDispatcher));
            command.ThrownExceptions.Subscribe(CommandException);
            return command;
        }

        public virtual void Dispose()
        {
        }

        [CanBeNull]
        public IEnumerable GetErrors(string propertyName)
        {
            return validationResult?.Errors?
                .Where(x => x.PropertyName == propertyName)
                .Select(x => x.ErrorMessage);
        }

        public void HideMe()
        {
            if (Window != null) Window.Visibility = Visibility.Hidden;
            else Parent?.HideMe();
        }

        [NotNull]
        public IDisposable HideWindow()
        {
            return new ActionUsage(HideMe, VisibleMe);
        }

        public virtual void OnActivated()
        {
        }

        public virtual void OnClosed()
        {
        }

        public virtual void OnClosing()
        {
        }

        /// <summary>
        /// Если модель передана в конструктор окна BaseWindow, то этот метод вызывается после инициализации окна.
        /// </summary>
        public virtual void OnInitialize()
        {
        }

        public void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        /// <summary>
        /// для propertyChanged (не уверен что очень нужно)
        /// </summary>
        /// <param name="args"></param>
        public void RaisePropertyChanged(PropertyChangedEventArgs args)
        {
            ((IReactiveObject)this).RaisePropertyChanged(args);
        }

        public void ShowMessage(string msg, string title = "Ошибка")
        {
            if ((DateTime.Now - lastShowError).TotalMilliseconds < 1000)
            {
                lastShowError = DateTime.Now;
                return;
            }

            try
            {
                var task = dialogCoordinator.ShowMessageAsync(this, title, msg);
                task.ContinueWith(o => lastShowError = DateTime.Now);
                return;
            }
            catch
            {
                //
            }

            if (Parent != null)
            {
                try
                {
                    Parent.ShowMessage(msg, title);
                    return;
                }
                catch
                {
                    //
                }
            }

            try
            {
                MessageBox.Show(msg, title);
            }
            catch
            {
                //
            }
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
        public async Task<bool?> ShowMessage(string title, string msg, string affirmativeBtn, string negateBtn,
            string auxiliaryBtn = null)
        {
            if (auxiliaryBtn == null)
            {
                var dlgRes = await dialogCoordinator.ShowMessageAsync(context, title, msg,
                    MessageDialogStyle.AffirmativeAndNegative,
                    new MetroDialogSettings { AffirmativeButtonText = affirmativeBtn, NegativeButtonText = negateBtn });
                return dlgRes == MessageDialogResult.Affirmative;
            }
            else
            {
                var dlgRes = await dialogCoordinator.ShowMessageAsync(context, title, msg,
                    MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary,
                    new MetroDialogSettings
                    {
                        AffirmativeButtonText = affirmativeBtn,
                        NegativeButtonText = negateBtn,
                        FirstAuxiliaryButtonText = auxiliaryBtn
                    });
                if (dlgRes == MessageDialogResult.FirstAuxiliary) return null;
                return dlgRes == MessageDialogResult.Affirmative;
            }
        }

        /// <summary>
        /// Запуск окна ожидания длительного процесса
        /// </summary>
        /// <param name="job">Работа</param>
        /// <param name="title">Заголовок</param>
        /// <param name="msg">сообщение</param>
        /// <param name="inMainThread">Выполнять в основном потоке (обязательно для AutoCAD/Revit,
        /// т.к. любые операции с чертежом нужно делать из основного потока).
        /// При этом нужно периодически выполнять прокачку очереди сообщений (DoEvents)</param>
        public async Task ShowProgressDialog([NotNull] Action<ProgressDialogController> job, string title, string msg,
            bool inMainThread = true)
        {
            Exception jobEx = null;
            var controller = await Task.Run(() => dialogCoordinator.ShowProgressAsync(context, title, msg));
            if (inMainThread)
            {
                try
                {
                    job(controller);
                }
                catch (Exception ex)
                {
                    jobEx = ex;
                }
            }
            else
            {
                await Task.Run(() => job(controller));
            }
            await controller.CloseAsync();
            if (jobEx != null)
            {
                throw jobEx;
            }
        }

        /// <summary>
        /// Валидация.
        /// </summary>
        public async void Validate([CanBeNull] string propName = null)
        {
            if (validator == null) return;
            await Task.Run(() =>
            {
                validationResult = validator.Validate(this);
                Errors = new List<string>(validationResult.Errors.Select(s => s.ErrorMessage).ToList());
                if (propName == null)
                {
                    if (validationResult?.Errors?.Any() == true)
                    {
                        foreach (var error in validationResult.Errors)
                        {
                            RaiseErrorsChanged(error.PropertyName);
                        }
                    }
                }
                else
                {
                    RaiseErrorsChanged(propName);
                }
                IsValidated = true;
                Logger.Debug($"Validate {this}");
            });
        }

        public void VisibleMe()
        {
            if (Window != null) Window.Visibility = Visibility.Visible;
            else Parent?.VisibleMe();
        }

        private void BaseViewModel_PropertyChanged(object sender, [NotNull] PropertyChangedEventArgs e)
        {
            if (!ignoreProps.Contains(e.PropertyName))
            {
                Validate(e.PropertyName);
            }
        }

        private void FindValidator(Type modelType)
        {
            Task.Run(() =>
            {
                var typeName = $"{modelType.Namespace}.{modelType.Name}Validator";
                var type = modelType.Assembly.GetType(typeName, false);
                if (type != null)
                {
                    validator = (IValidator)Activator.CreateInstance(type);
                }
                validators[modelType.TypeHandle] = validator;
                Validate();
            });
        }

        private void InitValidator()
        {
            var modelType = GetType();
            if (!validators.TryGetValue(modelType.TypeHandle, out validator))
            {
                FindValidator(modelType);
            }
        }

        public virtual void OnPropertyChanged([CanBeNull] [CallerMemberName] string propertyName = null)
        {
            this.RaisePropertyChanged(propertyName);
        }

        protected void OnPropertyChanged([NotNull] PropertyChangedEventArgs eventArgs)
        {
            this.RaisePropertyChanged(eventArgs.PropertyName);
        }
    }
}
