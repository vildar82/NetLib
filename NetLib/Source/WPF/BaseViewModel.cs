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
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace NetLib.WPF
{
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

        public BaseViewModel(IBaseViewModel parent)
        {
            Parent = parent;
            PropertyChanged += Validate;
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
        public virtual async void OnInitialize()
        {
            validator = await GetValidator(GetType());
            validationResult = validator?.Validate(this);
        }

        public virtual void OnClosed()
        {

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

        private void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private static async Task<IValidator> GetValidator(Type modelType)
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (validators.TryGetValue(modelType.TypeHandle, out IValidator validator)) return validator;
                    var typeName = $"{modelType.Namespace}.{modelType.Name}Validator";
                    var type = modelType.Assembly.GetType(typeName, true);
                    validators[modelType.TypeHandle] = validator = (IValidator) Activator.CreateInstance(type);
                    return validator;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, $"No Validator in {modelType.FullName}.");
                    return null;
                }
            });
        }

        public virtual void Dispose()
        {
        }
    }
}
