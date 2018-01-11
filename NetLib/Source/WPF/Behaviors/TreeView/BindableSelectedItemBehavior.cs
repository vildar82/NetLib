using JetBrains.Annotations;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace NetLib.WPF.Behaviors
{
    /// <summary>
    /// e:Interaction.Behaviors
    ///     behaviours:BindableSelectedItemBehavior SelectedItem = "{Binding SelectedItem, Mode=TwoWay}"
    /// e:Interaction.Behaviors
    /// </summary>
    [PublicAPI]
    public class BindableSelectedItemBehavior : Behavior<TreeView>
    {
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(BindableSelectedItemBehavior),
                new UIPropertyMetadata(null, OnSelectedItemChanged));

        public object SelectedItem
        {
            // ReSharper disable once UnusedMember.Global
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectedItemChanged += OnTreeViewSelectedItemChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject != null)
            {
                AssociatedObject.SelectedItemChanged -= OnTreeViewSelectedItemChanged;
            }
        }

        private static void OnSelectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var item = e.NewValue as TreeViewItem;
            item?.SetValue(TreeViewItem.IsSelectedProperty, true);
        }

        private void OnTreeViewSelectedItemChanged(object sender, [NotNull] RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedItem = e.NewValue;
        }
    }
}