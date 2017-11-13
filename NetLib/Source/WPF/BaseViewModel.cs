using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace NetLib.WPF
{
    /// <summary>
    /// ReactiveUI ViewModel, с реализацией INotifyDataErrorInfo
    /// </summary>
    public abstract class BaseViewModel : ReactiveObject, INotifyDataErrorInfo, IDisposable
    {
        protected readonly IDialogCoordinator dialogCoordinator = DialogCoordinator.Instance;
        private readonly Dictionary<string, ICollection<string>> errors = new Dictionary<string, ICollection<string>>();

        public BaseViewModel(BaseViewModel parent)
        {
            Parent = parent;
        }

        public BaseViewModel()
        {

        }

        public BaseWindow Window { get; set; }
        public BaseViewModel Parent { get; set; }
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public bool HasErrors => errors.Any(a => a.Value != null);
        [Reactive] public bool Hide { get; set; }
        [Reactive] public bool? DialogResult { get; set; }

        /// <summary>
        /// Если модель передана в конструктор окна BaseWindow, то этот метод вызывается после инициализации окна.
        /// </summary>
        public virtual void OnInitialize()
        {

        }

        public virtual void OnClosed()
        {

        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (propertyName.IsNullOrEmpty()) return null;
            errors.TryGetValue(propertyName, out ICollection<string> errs);
            return errs;
        }

        protected void RaiseErrorsChanged(string propertyName)
        {
            if (propertyName.IsNullOrEmpty()) return;
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Запись ошибки. Пустая строка или null очищает ошибку.
        /// </summary>
        protected void SetError(string propName, string err)
        {
            errors[propName] = err.IsNullOrEmpty() ? null : new List<string> { err };
            RaiseErrorsChanged(propName);
        }

        public BoolUsage HideUsing()
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

        public void CommandException(Exception e)
        {
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

        public virtual void Dispose()
        {
        }
    }
}
