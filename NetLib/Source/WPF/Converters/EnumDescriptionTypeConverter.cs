namespace NetLib.WPF.Converters
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Конвертер enum значений из описаний значений
    /// [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    /// public enum MyEnum
    /// {
    /// [Description("Центр")]
    /// Central,
    /// [Description("Восток")]
    /// East,
    /// [Description("Запад")]
    /// West
    /// }
    /// </summary>
    public class EnumDescriptionTypeConverter : EnumConverter
    {
        public EnumDescriptionTypeConverter(Type type)
            : base(type)
        {
        }

        public static string? GetEnumDescription(object? enumValue)
        {
            if (enumValue == null)
                return null;

            var fi = enumValue.GetType().GetField(enumValue.ToString());
            if (fi == null) return enumValue.ToString();
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : enumValue.ToString();
        }

        public override object? ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            return destinationType == typeof(string)
                ? GetEnumDescription(value)
                : base.ConvertTo(context, culture, value, destinationType);
        }
    }
}