using System;
using JetBrains.Annotations;
using ReactiveUI;

namespace NetLib.WPF
{
    /// <summary>
    /// Базовый класс модели - MVVM
    /// </summary>
    [PublicAPI]
    public abstract class BaseModel : ReactiveObject, IBaseModel
    {
        protected BaseModel(IBaseViewModel baseVM)
        {
            BaseVm = baseVM;
        }

        public IBaseViewModel BaseVm { get; set; }

        [NotNull]
        public ReactiveCommand CreateCommand(Action execute, IObservable<bool> canExecute = null)
        {
            return BaseVm.CreateCommand(execute, canExecute);
        }
        
        [NotNull]
        public ReactiveCommand CreateCommand<T>(Action<T> execute, IObservable<bool> canExecute = null)
        {
            return BaseVm.CreateCommand(execute, canExecute);
        }

        public void ShowMessage(string msg, string title = "Ошибка")
        {
            BaseVm.ShowMessage(msg, title);
        }
    }
}
