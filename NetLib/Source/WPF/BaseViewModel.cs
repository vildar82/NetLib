using FluentValidation;
using FluentValidation.Results;
using MahApps.Metro.Controls.Dialogs;
using NLog;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
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
        private static Logger Logger { get; } = LogManager.GetCurrentClassLogger();
        protected readonly IDialogCoordinator dialogCoordinator = DialogCoordinator.Instance;
        private ValidationResult validationResult;
        private IValidator validator;

        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IValidator> validators =
            new ConcurrentDictionary<RuntimeTypeHandle, IValidator>();

        static BaseViewModel()
        {
            validators[typeof(BaseViewModel).TypeHandle] = null;
        }

        public BaseViewModel(IBaseViewModel parent)
        {
            Parent = parent;
            PropertyChanged += Validate;
            InitValidator();
            ThrownExceptions.Subscribe(CommandException);
        }

        public BaseViewModel() : this(null)
        {
        }

        public BaseWindow Window { get; set; }
        public IBaseViewModel Parent { get; set; }
        [Reactive] public bool Hide { get; set; }
        [Reactive] public bool? DialogResult { get; set; }
        public bool HasErrors => validationResult?.Errors?.Count > 0;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// Если модель передана в конструктор окна BaseWindow, то этот метод вызывается после инициализации окна.
        /// </summary>
        public virtual  void OnInitialize()
        {
            
        }

        public virtual void OnClosed()
        {

        }

        private async void InitValidator()
        {
            Debug.WriteLine($"InitValidator for {GetType().Name}");
            validator = await GetValidator(GetType());
            Debug.WriteLine($"Validator for {GetType().Name} = {validator}");
            if (validator != null)
            {
                validationResult = validator.Validate(this);
                if (validationResult.Errors?.Any() == true)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        RaiseErrorsChanged(error.PropertyName);
                    }
                }
            }
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

        public void CommandException(Exception e)
        {
            Logger.Error(e, "CommandException");
            ShowMessage(e.Message);
        }

        public void ShowMessage(string msg, string title = "Ошибка")
        {
            try
            {
                dialogCoordinator.ShowMessageAsync(this, title, msg);
            }
            catch
            {
                MessageBox.Show(msg, title);
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return validationResult?.Errors?
                .Where(x => x.PropertyName == propertyName)
                .Select(x => x.ErrorMessage);
        }

        private void Validate(object sender, PropertyChangedEventArgs e)
        {
            if (validator == null) return;
            validationResult = validator.Validate(this);
            foreach (var error in validationResult.Errors)
            {
                RaiseErrorsChanged(error.PropertyName);
            }
        }

        public void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private static async Task<IValidator> GetValidator(Type modelType)
        {
            return await Task.Run(() =>
            {
                if (validators.TryGetValue(modelType.TypeHandle, out var validator)) return validator;
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
                return validator;
            });
        }

        public virtual void Dispose()
        {
        }
    }
}
