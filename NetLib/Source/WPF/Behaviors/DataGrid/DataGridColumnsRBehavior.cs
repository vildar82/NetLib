using JetBrains.Annotations;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using ReactiveUI;

namespace NetLib.WPF.Behaviors
{
    [PublicAPI]
    public class DataGridColumnsRBehavior
    {
        public static readonly DependencyProperty BindableColumnsProperty =
            DependencyProperty.RegisterAttached("BindableColumns",
                typeof(ReactiveList<DataGridColumn>),
                typeof(DataGridColumnsBehavior),
                new UIPropertyMetadata(null, BindableColumnsPropertyChanged));

        public static ReactiveList<DataGridColumn> GetBindableColumns([NotNull] DependencyObject element)
        {
            return (ReactiveList<DataGridColumn>)element.GetValue(BindableColumnsProperty);
        }

        public static void SetBindableColumns([NotNull] DependencyObject element, ReactiveList<DataGridColumn> value)
        {
            element.SetValue(BindableColumnsProperty, value);
        }

        private static void BindableColumnsPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            if (!(source is DataGrid dataGrid)) return;
            var columns = e.NewValue as ReactiveList<DataGridColumn>;
            dataGrid.Columns.Clear();
            if (columns == null)
            {
                return;
            }
            foreach (var column in columns)
            {
                try
                {
                    dataGrid.Columns.Add(column);
                }
                catch
                {
                    // ignored
                }
            }
            columns.CollectionChanged += (sender, e2) =>
            {
                var ne = e2;
                switch (ne.Action)
                {
                    case NotifyCollectionChangedAction.Reset:
                        dataGrid.Columns.Clear();
                        foreach (DataGridColumn column in ne.NewItems)
                        {
                            dataGrid.Columns.Add(column);
                        }
                        break;

                    case NotifyCollectionChangedAction.Add:
                        foreach (DataGridColumn column in ne.NewItems)
                        {
                            dataGrid.Columns.Add(column);
                        }
                        break;

                    case NotifyCollectionChangedAction.Move:
                        dataGrid.Columns.Move(ne.OldStartingIndex, ne.NewStartingIndex);
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        foreach (DataGridColumn column in ne.OldItems)
                        {
                            dataGrid.Columns.Remove(column);
                        }
                        break;

                    case NotifyCollectionChangedAction.Replace:
                        dataGrid.Columns[ne.NewStartingIndex] = ne.NewItems[0] as DataGridColumn;
                        break;
                }
            };
        }
    }
}