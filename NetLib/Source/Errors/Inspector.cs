using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NetLib.Errors.UI.View;
using NetLib.Errors.UI.ViewModel;

namespace NetLib.Errors
{
    /// <summary>
    /// Сбор ошибок и отображение
    /// </summary>
    public class Inspector
    {
        private readonly List<IError> errors = new List<IError>();

        public Inspector(string title = "Инфо")
        {
            Title = title;
        }

        public bool HasError => errors.Any();

        public string Title { get; set; }

        public void AddError(IError error)
        {
            if (error == null) return;
            errors.Add(error);
        }

        public void AddError(string group, string msg, ErrorLevel level = ErrorLevel.Error)
        {
            AddError(new Error {Group = group, Message = msg, Level = level});
        }

        public void AddError(Exception ex, string group)
        {
            AddError(new Error { Group = group, Message = ex.Message });
        }

        public void Show()
        {
            var view = GetWindow();
            view?.Show();
        }

        public Window GetWindow()
        {
            if (!HasError) return null;
            var errorsVM = new ErrorsViewModel(errors, Title, false);
            return new ErrorsView(errorsVM);
        }
    }
}
