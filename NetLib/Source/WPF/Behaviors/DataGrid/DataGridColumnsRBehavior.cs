namespace NetLib.WPF.Behaviors
{
    using System.Collections.Specialized;
    using System.Windows;
    using System.Windows.Controls;
    using DynamicData.Binding;
    using JetBrains.Annotations;

    [PublicAPI]
    public class DataGridColumnsRBehavior
    {
        public static readonly DependencyProperty BindableColumnsProperty =
            DependencyProperty.RegisterAttached("BindableColumns",
                typeof(ObservableCollectionExtended<DataGridColumn>),
                typeof(DataGridColumnsBehavior),
                new UIPropertyMetadata(null, BindableColumnsPropertyChanged));

        public static ObservableCollectionExtended<DataGridColumn> GetBindableColumns([NotNull] DependencyObject element)
        {
            return (ObservableCollectionExtended<DataGridColumn>)element.GetValue(BindableColumnsProperty);
        }

        public static void SetBindableColumns([NotNull] DependencyObject element, ObservableCollectionExtended<DataGridColumn> value)
        {
            element.SetValue(BindableColumnsProperty, value);
        }

        private static void BindableColumnsPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            if (!(source is DataGrid dataGrid))
                return;

            var columns = e.NewValue as ObservableCollectionExtended<DataGridColumn>;
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