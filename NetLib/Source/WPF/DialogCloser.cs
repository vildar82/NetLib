using JetBrains.Annotations;
using System.Windows;

namespace NetLib.WPF
{
    /// <summary>
    /// Window ...
    /// xmlns:xc="clr-namespace:ExCastle.Wpf"
    /// xc:DialogCloser.DialogResult="{Binding DialogResult}">
    /// </summary>
    [PublicAPI]
    public static class DialogCloser
    {
        public static readonly DependencyProperty DialogResultProperty =
            DependencyProperty.RegisterAttached("DialogResult", typeof(bool?), typeof(DialogCloser),
                new PropertyMetadata(DialogResultChanged));

        public static void SetDialogResult([NotNull] Window target, bool? value)
        {
            target.SetValue(DialogResultProperty, value);
        }

        private static void DialogResultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Window window && window.IsActive)
            {
                window.DialogResult = e.NewValue as bool?;
            }
        }
    }
}