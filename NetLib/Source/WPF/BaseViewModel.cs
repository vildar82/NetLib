using FluentValidation;
using FluentValidation.Results;
using JetBrains.Annotations;
using MahApps.Metro.Controls.Dialogs;
using NLog;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace NetLib.WPF
{
    /// <inheritdoc cref="IBaseViewModel" />
    /// <summary>
    /// ReactiveUI ViewModel, +FluentValidator (должен быть класс с таким же именем +Validator).
    /// </summary>
    [PublicAPI]
    public abstract class BaseViewModel : ReactiveObject, IBaseViewModel
    {
        protected readonly IDialogCoordinator dialogCoordinator = DialogCoordinator.Instance;
        protected IValidator validator;
        private static readonly HashSet<string> ignoreProps = new HashSet<string> { "Hide", "DialogResult", "Errors" };
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IValidator> validators =
            new ConcurrentDictionary<RuntimeTypeHandle, IValidator>();

        private ValidationResult validationResult;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        [Reactive] public bool? DialogResult { get; set; }
        [Reactive] public List<string> Errors { get; set; }
        public bool HasErrors => validationResult?.Errors?.Count > 0;
        [Obsolete("Use Hide()")]
        [Reactive]
        public bool Hide { get; set; }
        public bool IsValidated { get; private set; }
        public IBaseViewModel Parent { get; set; }
        public BaseWindow Window { get; set; }
        protected static Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        static BaseViewModel()
        {
            validators[typeof(BaseViewModel).TypeHandle] = null;
        }

        public BaseViewModel(IBaseViewModel parent)
        {
            InitValidator();
            using (SuppressChangeNotifications())
            {
                Parent = parent;
                PropertyChanged += BaseViewModel_PropertyChanged;
                ThrownExceptions.Subscribe(CommandException);
            }
        }

        public BaseViewModel() : this(null)
        {
        }

        /// <summary>
        /// Добавление команды - ThrownExceptions.Subscribe.
        /// </summary>
        [NotNull]
        public ReactiveCommand AddCommand([NotNull] ReactiveCommand command)
        {
            command.ThrownExceptions.Subscribe(CommandException);
            return command;
        }

        public void CommandException([NotNull] Exception e)
        {
            if (e is OperationCanceledException) return;
            Logger.Error(e, "CommandException");
            ShowMessage(e.Message);
        }

        [NotNull]
        public ReactiveCommand CreateCommand(Action execute, [CanBeNull] IObservable<bool> canExecute = null)
        {
            var command = ReactiveCommand.Create(execute, canExecute);
            command.ThrownExceptions.Subscribe(CommandException);
            return command;
        }

        [NotNull]
        public ReactiveCommand CreateCommand<T>(Action<T> execute, [CanBeNull] IObservable<bool> canExecute = null)
        {
            var command = ReactiveCommand.Create(execute, canExecute);
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

        public virtual void OnClosed()
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
            try
            {
                dialogCoordinator.ShowMessageAsync(this, title, msg);
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
                try
                {
                    var type = modelType.Assembly.GetType(typeName, true);
                    validator = (IValidator)Activator.CreateInstance(type);
                }
                catch
                {
                    Logger.Error($"No Validator in {modelType.FullName}.");
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
    }
}