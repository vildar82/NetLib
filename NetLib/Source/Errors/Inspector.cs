using JetBrains.Annotations;
using NetLib.Errors.UI.View;
using NetLib.Errors.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace NetLib.Errors
{
    /// <summary>
    /// Сбор ошибок и отображение
    /// </summary>
    [PublicAPI]
    public class Inspector
    {
        private readonly List<IError> errors = new List<IError>();

        public bool HasError => errors.Any();

        public string Title { get; set; }

        public Inspector(string title = "Инфо")
        {
            Title = title;
        }

        public void AddError([CanBeNull] IError error)
        {
            if (error == null) return;
            errors.Add(error);
        }

        public void AddError(string group, string msg, ErrorLevel level = ErrorLevel.Error)
        {
            AddError(new Error { Group = group, Message = msg, Level = level });
        }

        public void AddError([NotNull] Exception ex, string group)
        {
            AddError(new Error { Group = group, Message = ex.Message });
        }

        [CanBeNull]
        public Window GetWindow()
        {
            if (!HasError) return null;
            var errorsVM = new ErrorsViewModel(errors, Title, false);
            return new ErrorsView(errorsVM);
        }

        public void Show()
        {
            var view = GetWindow();
            view?.Show();
        }
    }
}