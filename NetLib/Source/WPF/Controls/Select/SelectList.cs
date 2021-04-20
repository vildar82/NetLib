namespace NetLib.WPF.Controls.Select
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;

    public static class SelectList
    {
        public static T Select<T>(List<SelectListItem<T>> items, string title, string? name = null)
        {
            return Select(items, null, title, name);
        }

        public static T Select<T>(
            List<SelectListItem<T>> items,
            SelectListItem<T>? selected,
            string title,
            string? name = null)
        {
            var selVM = new SelectListVM<T>(items, title, name) { Selected = selected ?? items[0] };
            var selView = new SelectListView(selVM);
            if (selView.ShowDialog() == true)
            {
                return selVM.Selected.Object;
            }

            throw new OperationCanceledException("Отменено пользователем");
        }

        public static List<T> SelectMany<T>(List<SelectListItem<T>> items, string title, string? name = null)
        {
            var selVM = new SelectListVM<T>(items, title, name, true);
            var selView = new SelectListView(selVM);
            if (selView.ShowDialog() == true)
            {
                return selVM.MultiSelected.Select(s => s.Object).ToList();
            }

            throw new OperationCanceledException("Отменено пользователем");
        }

        public static T Select<T>(
            List<SelectListItem<T>> items,
            T? selected,
            string title,
            string? name = null)
        {
            return Select(items, items.FirstOrDefault(i => i.Object.Equals(selected)), title, name);
        }
    }
}