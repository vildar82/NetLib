using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Data;
using MicroMvvm;

namespace NetLib.WPF.Controls.Select
{
	/// <summary>
	/// Использование - SelectList.Select()
	/// </summary>
	/// <typeparam name="T"></typeparam>
	internal class SelectListVM<T> : BaseViewModel
	{
	    private string filter;

	    public SelectListVM(List<SelectListItem<T>> items, string title, string name=null)
		{
			Title = title;
			Name = name;
			HasName = !string.IsNullOrEmpty(name);
			ItemsView = new ListCollectionView(items)
			{
				Filter = FilterPredicate
			};
			HasFilter = items.Count > 10;
		}

		public string Title { get; set; }
		public string Name { get; set; }
		public bool HasName { get; set; }
		public ListCollectionView ItemsView { get; set; }
		public SelectListItem<T> Selected { get; set; }

	    public string Filter
	    {
	        get => filter;
	        set { filter = value; ItemsView.Refresh(); }
	    }

	    public bool HasFilter { get; set; }
		public RelayCommand OK => new RelayCommand(OkExecute, OkCanExecute);

		private bool FilterPredicate(object obj)
		{
			if (string.IsNullOrEmpty(Filter)) return true;
			var item = (SelectListItem<T>)obj;
			return Regex.IsMatch(item.Name, Filter, RegexOptions.IgnoreCase);
		}

		private void OkExecute()
		{
			
		}
		private bool OkCanExecute()
		{
			return Selected != null;
		}
	}

	internal class SelectListDesignVM : SelectListVM<Item>
	{
		public SelectListDesignVM() : base(GetItems(), "Выбор сети", "Сети в текущем чертеже")
		{
		}

		private static List<SelectListItem<Item>> GetItems()
		{
			return new List<SelectListItem<Item>>
			{
				new SelectListItem<Item>("K1-1", new Item()),
				new SelectListItem<Item>("T1-2", new Item()),
				new SelectListItem<Item>("U2-1", new Item()),
				new SelectListItem<Item>("I6-1", new Item())
			};
		}
	}

	public class Item
	{
	}
}
