using ReactiveUI;
using System;
using System.Reactive.Linq;

namespace NetLib.WPF.Controls
{
    /// <summary>
    /// Текстовое значение
    /// </summary>
    public class TextVM : BaseViewModel
    {
        /// <summary>
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

        public string Title { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public ReactiveCommand OK { get; }

        private void OkExecute()
        {
            DialogResult = true;
        }
    }

    public class DesignTextVM : TextVM
    {
        public DesignTextVM()
            : base("Новый объект", "Название объекта", "Вася", s => !string.IsNullOrEmpty(s))
        {
        }
    }
}
