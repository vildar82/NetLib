namespace NetLib.WPF.Behaviors
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using JetBrains.Annotations;

    [PublicAPI]
    public class CustomSortBehaviour
    {
        public static readonly DependencyProperty AllowCustomSortProperty =
            DependencyProperty.RegisterAttached("AllowCustomSort", typeof(bool),
                typeof(CustomSortBehaviour), new UIPropertyMetadata(false, OnAllowCustomSortChanged));

        public static readonly DependencyProperty CustomSorterProperty =
                    DependencyProperty.RegisterAttached("CustomSorter", typeof(ICustomSorter), typeof(CustomSortBehaviour));

        public static bool GetAllowCustomSort([NotNull] DataGrid grid)
        {
            return (bool)grid.GetValue(AllowCustomSortProperty);
        }

        public static ICustomSorter GetCustomSorter([NotNull] DataGridColumn gridColumn)
        {
            return (ICustomSorter)gridColumn.GetValue(CustomSorterProperty);
        }

        public static void SetAllowCustomSort([NotNull] DataGrid grid, bool value)
        {
            grid.SetValue(AllowCustomSortProperty, value);
        }

        public static void SetCustomSorter([NotNull] DataGridColumn gridColumn, ICustomSorter value)
        {
            gridColumn.SetValue(CustomSorterProperty, value);
        }

        private static void HandleCustomSorting(object sender, DataGridSortingEventArgs e)
        {
            if (!(sender is DataGrid dataGrid) || !GetAllowCustomSort(dataGrid))
                return;
            if (!(dataGrid.ItemsSource is ListCollectionView listColView))
                throw new Exception("The DataGrid's ItemsSource property must be of type, ListCollectionView");

            // Sanity check
            var sorter = GetCustomSorter(e.Column);
            if (sorter == null)
                return;

            sorter.SortPropertyName = e.Column.SortMemberPath;

            // The guts.
            e.Handled = true;
            var direction = e.Column.SortDirection != ListSortDirection.Ascending
                ? ListSortDirection.Ascending
                : ListSortDirection.Descending;
            e.Column.SortDirection = sorter.SortDirection = direction;
            listColView.CustomSort = sorter;
        }

        private static void OnAllowCustomSortChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is DataGrid existing))
                return;

            var oldAllow = (bool)e.OldValue;
            var newAllow = (bool)e.NewValue;
            if (!oldAllow && newAllow)
            {
                existing.Sorting += HandleCustomSorting;
            }
            else
            {
                existing.Sorting -= HandleCustomSorting;
            }
        }
    }
}