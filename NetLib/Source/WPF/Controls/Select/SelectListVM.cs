using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace NetLib.WPF.Controls.Select
{
    /// <summary>
    /// Использование - SelectList.Select()
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class SelectListVM<T> : BaseViewModel
    {
        private string filter;

        public SelectListVM(List<SelectListItem<T>> items, string title, string name = null)
        {
            Title = title;
            Name = name;
            HasName = !string.IsNullOrEmpty(name);
            ItemsView = new ListCollectionView(items)
            {
                Filter = FilterPredicate
            };
            HasFilter = items.Count > 10;
            OK = CreateCommand(OkExecute, this.WhenAnyValue(w => w.Selected).Select(s => s != null));
        }

        public string Title { get; set; }
        public string Name { get; set; }
        public bool HasName { get; set; }
        public ListCollectionView ItemsView { get; set; }
        [Reactive] public SelectListItem<T> Selected { get; set; }

        public string Filter
        {
            get => filter;
            set { filter = value; ItemsView.Refresh(); }
        }

        public bool HasFilter { get; set; }
        public ReactiveCommand OK { get; set; }

        private bool FilterPredicate(object obj)
        {
            if (string.IsNullOrEmpty(Filter)) return true;
            var item = (SelectListItem<T>)obj;
            return Regex.IsMatch(item.Name, Filter, RegexOptions.IgnoreCase);
        }

        private void OkExecute()
        {
            DialogResult = true;
        }
    }

    internal class SelectListDesignVM : SelectListVM<int>
    {
        public SelectListDesignVM() : base(GetItems(), "Выбор сети", "Сети в текущем чертеже")
        {
        }

        private static List<SelectListItem<int>> GetItems()
        {
            return new List<SelectListItem<int>>
            {
                new SelectListItem<int>("K1-1", 5),
                new SelectListItem<int>("T1-2", 1),
                new SelectListItem<int>("U2-1", 25),
                new SelectListItem<int>("I6-1", 1)
            };
        }
    }
}
