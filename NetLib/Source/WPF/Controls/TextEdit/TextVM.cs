using MicroMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLib.WPF.Controls
{
	/// <summary>
	/// Текстовое значение
	/// </summary>
	public class TextVM : ViewModelBase
	{
		private readonly Predicate<string> allowValue;

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
			OK = new RelayCommand(OkExecute, CanOkExecute);
			this.allowValue = allowValue;
		}

		public string Title { get; set; }
		public string Name { get; set; }
		public string Value { get; set; }

		public RelayCommand OK { get; }

		private bool CanOkExecute()
		{
			return allowValue(Value);
		}

		private void OkExecute()
		{
		}
	}

	public class DesignTextVM : TextVM
	{
		public DesignTextVM() 
			: base("Новый объект", "Название объекта", "Вася", s=> !string.IsNullOrEmpty(s))
		{
		}
	}
}
