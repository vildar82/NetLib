using FluentValidation;
using FluentValidation.Results;
using MahApps.Metro.Controls.Dialogs;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using NetLib.Locks;
using NLog;

namespace NetLib.WPF
{
    /// <inheritdoc cref="IBaseViewModel" />
    /// <summary>
    /// ReactiveUI ViewModel, +FluentValidator (должен быть класс с таким же именем +Validator).
    /// </summary>
    public abstract class BaseViewModel : ReactiveObject, IBaseViewModel
    {
        protected static Logger Logger { get; } = LogManager.GetCurrentClassLogger();
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
            Changed.Where(w => !w.PropertyName.EqualsAny("Window", "Parent", "Hide", "DialogResult", "HasErrors",
                "Errors")).Throttle(TimeSpan.FromMilliseconds(500)).Delay(TimeSpan.FromMilliseconds(100))
                .Subscribe(s => Validate(s.PropertyName));
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
        [Reactive] public List<string> Errors { get; set; }
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// Если модель передана в конструктор окна BaseWindow, то этот метод вызывается после инициализации окна.
        /// </summary>
        public virtual void OnInitialize()
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
            await Task.Delay(100);
            Validate(null);
        }

        private async void Validate(string propName)
        {
            if (validator == null) return;
            await Task.Run(() =>
            {
                validationResult = validator.Validate(this);
                Errors = new List<string>(validationResult.Errors.Select(s => s.ErrorMessage).ToList());
            });
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
            Logger.Debug($"Validate {this}");
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
            if (e is OperationCanceledException) return;
            Logger.Error("CommandException", e);
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
