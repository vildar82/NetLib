namespace NetLib.WPF.Data
{
    using System;
    using System.Diagnostics;
    using System.Windows.Input;
    using JetBrains.Annotations;
    using NLog;

    /// <summary>
    /// A command whose sole purpose is to relay its functionality to other objects by invoking delegates. The default return value for the CanExecute method is 'true'.
    /// </summary>
    public class RelayCommand<T> : ICommand
    {
        private ILogger Logger { get; } = LogManager.GetCurrentClassLogger();

        protected readonly Action<Exception> _exception;
        protected readonly Predicate<T> _canExecute;
	    [NotNull]
	    protected readonly Action<T> _execute;

	    /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand([NotNull] Action<T> execute, Predicate<T> canExecute = null)
        {
	        _execute = execute ?? throw new ArgumentNullException("execute");
            _canExecute = canExecute;
        }

        public RelayCommand([NotNull] Action<T> execute, Action<Exception> exception, Predicate<T> canExecute = null)
            : this(execute, canExecute)
        {
            _exception = exception;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }

            remove
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        [DebuggerStepThrough]
        public virtual bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke((T)parameter) ?? true;
        }

        public virtual void Execute(object parameter)
        {
            try
            {
                _execute((T)parameter);
            }
            catch (Exception ex)
            {
                Logger?.Error(ex);
                _exception?.Invoke(ex);
            }
        }
    }

    /// <summary>
    /// A command whose sole purpose is to relay its functionality to other objects by invoking delegates. The default return value for the CanExecute method is 'true'.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private ILogger Logger { get; } = LogManager.GetCurrentClassLogger();

        protected readonly Action<Exception> _exception;
        protected readonly Func<bool> _canExecute;
	    [NotNull]
	    protected readonly Action _execute;

	    /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand([NotNull] Action execute, Func<bool> canExecute = null)
        {
	        _execute = execute ?? throw new ArgumentNullException("execute");
            _canExecute = canExecute;
        }

        public RelayCommand([NotNull] Action execute, Action<Exception> exception, Func<bool> canExecute = null)
            : this(execute, canExecute)
        {
            _exception = exception;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }

            remove
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        [DebuggerStepThrough]
        public virtual bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke() ?? true;
        }

        public virtual void Execute(object parameter)
        {
            try
            {
                _execute();
            }
            catch (Exception ex)
            {
                Logger?.Error(ex);
                _exception?.Invoke(ex);
            }
        }
    }
}
