using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace NetLib.WPF.Converters
{
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public abstract class ConvertorBase : MarkupExtension, IValueConverter        
    {
        /// <summary>
        /// Must be implemented in inheritor.
        /// </summary>
        public abstract object Convert (object value, Type targetType, object parameter,
            CultureInfo culture);

        /// <summary>
        /// Override if needed.
        /// </summary>
        public virtual object ConvertBack (object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException($"ConvertBack в ConvertorBase - из значения {value} в {targetType}");
        }

        public override object ProvideValue (IServiceProvider serviceProvider)
        {
            return this;
        }

        object IValueConverter.Convert (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                return Convert(value, targetType, parameter, culture);
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }

        object IValueConverter.ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                return ConvertBack(value, targetType, parameter, culture);
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }
    }
}
