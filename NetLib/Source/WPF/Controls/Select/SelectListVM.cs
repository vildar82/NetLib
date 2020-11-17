namespace NetLib.WPF.Controls.Select
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Text.RegularExpressions;
    using System.Windows.Controls;
    using System.Windows.Data;
    using JetBrains.Annotations;
    using ReactiveUI;

    /// <summary>
    /// Использование - SelectList.Select()
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class SelectListVM<T> : BaseViewModel
    {
        private string filter;

        public string Filter
        {
            get => filter;
            set { filter = value; ItemsView.Refresh(); }
        }

        public bool HasFilter { get; set; }

        public bool HasName { get; set; }

        public ListCollectionView ItemsView { get; set; }

        public string Name { get; set; }

        public ReactiveCommand<Unit, Unit> OK { get; set; }

        public bool SelectAll { get; set; }

        public SelectListItem<T> Selected { get; set; }

        public List<SelectListItem<T>> MultiSelected { get; set; }

        /// <summary>
        /// Показывать галочки выбора
        /// </summary>
        public bool MultiSelect { get; set; }

        public string Title { get; set; }

        public bool CanCustomValue { get; set; }
        
        public Control CustomValue { get; set; }
        
        public Func<string> IsCustomValueValid { get; set; }

        public SelectListVM([NotNull] List<SelectListItem<T>> items, string title, Control customValue,
            Func<string> isCustomValueValid,             [CanBeNull] string name = null) 
            : this (items, title, name)
        {
            CanCustomValue = true;
            CustomValue = customValue;
            IsCustomValueValid = isCustomValueValid;
        }

        public SelectListVM([NotNull] List<SelectListItem<T>> items, string title, [CanBeNull] string name = null,
            bool multiSelect = false)
        {
            Title = title;
            Name = name;
            HasName = !string.IsNullOrEmpty(name);
            ItemsView = new ListCollectionView(items)
            {
                Filter = FilterPredicate
            };

            HasFilter = items.Count > 10;
            var canOk = this.WhenAnyValue(v => v.CanCustomValue, v => v.Selected).Select(s => s.Item2 != null || s.Item1);
            
            if (multiSelect)
            {
                OK = CreateCommand(OkExecute);
                MultiSelect = true;
            }
            else
            {
                OK = CreateCommand(OkExecute, canOk);
            }

            SelectAll = items.All(a => a.IsSelected);
            this.WhenAnyValue(v => v.SelectAll).Skip(1).Subscribe(s => items.ForEach(i => i.IsSelected = s));
        }

        private bool FilterPredicate(object obj)
        {
            if (string.IsNullOrEmpty(Filter))
                return true;

            var item = (SelectListItem<T>)obj;
            return Regex.IsMatch(item.Name, Filter, RegexOptions.IgnoreCase);
        }

        private void OkExecute()
        {
            if (MultiSelect)
            {
                MultiSelected = ItemsView.Cast<SelectListItem<T>>().Where(w => w.IsSelected).ToList();
            }
            else
            {
                if (CanCustomValue && Selected == null)
                {
                    var err = IsCustomValueValid();
                    if (!err.IsNullOrEmpty())
                    {
                        ShowMessage($"Свое значение некорректно - {err}.");
                        return;
                    }
                }
            }

            DialogResult = true;
        }
    }

    internal class SelectListDesignVM : SelectListVM<int>
    {
        public SelectListDesignVM() : base(GetItems(), "Выбор сети", "Сети в текущем чертеже")
        {
        }

        [NotNull]
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