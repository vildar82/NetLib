using ReactiveUI;
using System;
using System.Reactive.Linq;
using JetBrains.Annotations;

namespace NetLib.WPF.Controls
{
    using System.Reactive;

    public class DesignTextVM : TextVM
    {
        public DesignTextVM()
            : base("Новый объект", "Название объекта", "Вася", s => !string.IsNullOrEmpty(s))
        {
        }
    }

    /// <summary>
    /// Текстовое значение
    /// </summary>
    [PublicAPI]
    public class TextVM : BaseViewModel
    {
        public string Name { get; set; }

        public ReactiveCommand<Unit, Unit> OK { get; }

        public string Title { get; set; }

        public string Value { get; set; }
        
        public string Error { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextVM"/> class.
        /// Заголовок, имя и значение
        /// </summary>
        /// <param name="title">Заголовок окна</param>
        /// <param name="name">Название значения</param>
        /// <param name="value">Значение редактируемое</param>
        /// <param name="allowValue">Проверка введенного значения</param>
        public TextVM(string title, string name, string value, Predicate<string> allowValue)
        {
            Title = title;
            Name = name;
            Value = value;
            OK = CreateCommand(OkExecute, this.WhenAnyValue(w => w.Value).Select(s => allowValue(s)));
        }

        public TextVM(string title, string name, string value, Func<string, string> allowValue)
        {
            Title = title;
            Name = name;
            Value = value;
            var canOk = this.WhenAnyValue(w => w.Value).Select(s =>
            {
                var err = allowValue(s);
                if (err.IsNullOrEmpty())
                {
                    Error = null;
                    return true;
                }
                Error = err;
                return false;
            });
            OK = CreateCommand(OkExecute, canOk);
        }

        private void OkExecute()
        {
            DialogResult = true;
        }
    }
}