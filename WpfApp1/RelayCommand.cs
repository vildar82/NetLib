using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp1
{
    public class RelayCommand : ICommand
    {
        private Predicate<object> canExecute;
        private Action<object> execute;

        public event EventHandler CanExecuteChanged
        {
            add {
                CommandManager.RequerySuggested += value;
                CanExecuteChangedInternal += value;
            }

            remove {
                CommandManager.RequerySuggested -= value;
                CanExecuteChangedInternal -= value;
            }
        }

        private event EventHandler CanExecuteChangedInternal;

        public RelayCommand(Action<object> execute)
            : this(execute, DefaultCanExecute)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        public bool CanExecute(object parameter)
        {
            return canExecute != null && canExecute(parameter);
        }

        public void Destroy()
        {
            canExecute = _ => false;
            execute = _ => { };
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }

        public void OnCanExecuteChanged()
        {
            var handler = CanExecuteChangedInternal;
            handler?.Invoke(this, EventArgs.Empty);
        }

        private static bool DefaultCanExecute(object parameter)
        {
            return true;
        }
    }
}