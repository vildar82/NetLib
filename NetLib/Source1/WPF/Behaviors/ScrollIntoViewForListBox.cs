using System;
using System.Windows.Controls;
using System.Windows.Interactivity;
using JetBrains.Annotations;

namespace NetLib.WPF.Behaviors
{
    /// <summary>
    /// Скрол до выделенного элемента
    /// <ListBox ItemsSource="{Binding Path=MyList}"
    /// SelectedItem="{Binding Path=MyItem, Mode=TwoWay}"
    /// SelectionMode="Single">
    /// i:Interaction.Behaviors>
    /// Behaviors:ScrollIntoViewForListBox />
    /// i:Interaction.Behaviors>
    /// </ListBox>
    /// </summary>
    [PublicAPI]
    public class ScrollIntoViewForListBox : Behavior<ListBox>
    {
        /// <summary>
        /// When Beahvior is attached
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
        }

        /// <summary>
        /// When behavior is detached
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
        }

        /// <summary>
        /// On Selection Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox?.SelectedItem != null)
            {
                listBox.Dispatcher.BeginInvoke((Action)(() =>
               {
                   listBox.UpdateLayout();
                   if (listBox.SelectedItem != null)
                       listBox.ScrollIntoView(listBox.SelectedItem);
               }));
            }
        }
    }
}