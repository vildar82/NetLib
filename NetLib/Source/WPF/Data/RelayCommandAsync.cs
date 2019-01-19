namespace NetLib.WPF.Data
{
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;

    public class RelayCommandAsync<T> : RelayCommand<T>
	{
		private bool isExecuting;
		public event EventHandler Started;
		public event EventHandler Ended;

		public bool IsExecuting => isExecuting;

		public RelayCommandAsync(Action<T> execute, Predicate<T> canExecute = null)
			: base(execute, canExecute)
		{
		}

		public override bool CanExecute(object parameter)
		{
			return base.CanExecute(parameter) && !isExecuting;
		}

		public override void Execute(object parameter)
		{
			try
			{
				isExecuting = true;
				Started?.Invoke(this, EventArgs.Empty);

				var task = Task.Factory.StartNew(() =>
				{
                    base.Execute(parameter);
                });
				task.ContinueWith(t =>
                {
					OnRunWorkerCompleted(EventArgs.Empty);
				}, TaskScheduler.FromCurrentSynchronizationContext());
			}
			catch (Exception ex)
			{
				OnRunWorkerCompleted(new RunWorkerCompletedEventArgs(null, ex, true));
			}
		}

		private void OnRunWorkerCompleted(EventArgs e)
		{
			isExecuting = false;
			Ended?.Invoke(this, e);
		}
	}

	public class RelayCommandAsync : RelayCommand
    {
        private bool isExecuting;
        public event EventHandler Started;
        public event EventHandler Ended;

        public bool IsExecuting => isExecuting;

        public RelayCommandAsync(Action execute, Func<bool> canExecute)
            : base(execute, canExecute)
        {
        }

        public RelayCommandAsync(Action execute)
            : base(execute)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter) && !isExecuting;
        }

        public override void Execute(object parameter)
        {
            try
            {
                isExecuting = true;
                Started?.Invoke(this, EventArgs.Empty);

                var task = Task.Factory.StartNew(() =>
                {
                    _execute();
                });
                task.ContinueWith(t =>
                {
                    OnRunWorkerCompleted(EventArgs.Empty);
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch (Exception ex)
            {
                OnRunWorkerCompleted(new RunWorkerCompletedEventArgs(null, ex, true));
            }
        }

        private void OnRunWorkerCompleted(EventArgs e)
        {
            isExecuting = false;
            Ended?.Invoke(this, e);
        }
    }
}
