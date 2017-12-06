using FluentValidation;
using FluentValidation.Results;
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
    public abstract class BaseViewModel : ReactiveObject, IBaseViewModel
    {
        private static readonly HashSet<string> ignoreProps = new HashSet<string> { "Hide", "DialogResult", "Errors" };
        private ValidationResult validationResult;

        protected static Logger Logger { get; } = LogManager.GetCurrentClassLogger();
        protected readonly IDialogCoordinator dialogCoordinator = DialogCoordinator.Instance;
        protected IValidator validator;

        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IValidator> validators =
            new ConcurrentDictionary<RuntimeTypeHandle, IValidator>();

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

        public BaseWindow Window { get; set; }
        public IBaseViewModel Parent { get; set; }
        [Reactive] public bool Hide { get; set; }
        [Reactive] public bool? DialogResult { get; set; }
        public bool HasErrors => validationResult?.Errors?.Count > 0;
        [Reactive] public List<string> Errors { get; set; }
        public bool IsValidated { get; private set; }
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private void BaseViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!ignoreProps.Contains(e.PropertyName))
            {
                Validate(e.PropertyName);
            }
        }

        /// <summary>
        /// Если модель передана в конструктор окна BaseWindow, то этот метод вызывается после инициализации окна.
        /// </summary>
        public virtual void OnInitialize()
        {

        }

        public virtual void OnClosed()
        {

        }

        private void InitValidator()
        {
            var modelType = GetType();
            if (!validators.TryGetValue(modelType.TypeHandle, out validator))
            {
                FindValidator(modelType);
            }
        }

        /// <summary>
        /// Валидация. 
        /// </summary>
        public async void Validate(string propName = null)
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

        public IDisposable HideWindow()
        {
            return new BoolUsage(Hide, true, h => Hide = h);
        }

        /// <summary>
        /// Добавление команды - ThrownExceptions.Subscribe.
        /// </summary>
        public ReactiveCommand AddCommand(ReactiveCommand command)
        {
            command.ThrownExceptions.Subscribe(CommandException);
            return command;
        }

        public ReactiveCommand CreateCommand(Action execute, IObservable<bool> canExecute = null)
        {
            var command = ReactiveCommand.Create(execute, canExecute);
            command.ThrownExceptions.Subscribe(CommandException);
            return command;
        }
        public ReactiveCommand CreateCommand<T>(Action<T> execute, IObservable<bool> canExecute = null)
        {
            var command = ReactiveCommand.Create(execute, canExecute);
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

        public IEnumerable GetErrors(string propertyName)
        {
            return validationResult?.Errors?
                .Where(x => x.PropertyName == propertyName)
                .Select(x => x.ErrorMessage);
        }

        public void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
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

        /// <summary>
        /// для propertyChanged (не уверен что очень нужно)
        /// </summary>
        /// <param name="args"></param>
        public void RaisePropertyChanged(PropertyChangedEventArgs args)
        {
            ((IReactiveObject)this).RaisePropertyChanged(args);
        }

        public virtual void Dispose()
        {
        }
    }
}
