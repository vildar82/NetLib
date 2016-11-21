using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace NetLib.WPF.Converters
{
    /// <summary>
    /// ComboBox ItemsSource="{Binding Source={local:EnumBindingSource {x:Type local:MyEnum}}}"    /// 
    /// </summary>
    public class EnumBindingSourceExtension : MarkupExtension
    {
        private Type _enumType;
        public Type EnumType {
            get { return this._enumType; }
            set {
                if (value != this._enumType)
                {
                    if (null != value)
                    {
                        Type enumType = Nullable.GetUnderlyingType(value) ?? value;
                        if (!enumType.IsEnum)
                            throw new ArgumentException("Type must be for an Enum.");
                    }

                    this._enumType = value;
                }
            }
        }

        public EnumBindingSourceExtension () { }

        public EnumBindingSourceExtension (Type enumType)
        {
            this.EnumType = enumType;
        }

        public override object ProvideValue (IServiceProvider serviceProvider)
        {
            if (null == this._enumType)
                throw new InvalidOperationException("The EnumType must be specified.");

            Type actualEnumType = Nullable.GetUnderlyingType(this._enumType) ?? this._enumType;
            Array enumValues = Enum.GetValues(actualEnumType);

            if (actualEnumType == this._enumType)
                return enumValues;

            Array tempArray = Array.CreateInstance(actualEnumType, enumValues.Length + 1);
            enumValues.CopyTo(tempArray, 1);
            return tempArray;
        }
    }

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
        public EnumDescriptionTypeConverter (Type type)
            : base(type)
        {
        }
        public override object ConvertTo (ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return  GetEnumDescription(value);                
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public static string GetEnumDescription (object enumValue)
        {
            if (enumValue == null) return null;
            FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());
            if (fi == null) return null;
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute),false);            
            
            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return enumValue.ToString();
        }
    }

    public static class EnumDescriptionExt
    {
        public static string Description(this object enumValue)
        {
            return EnumDescriptionTypeConverter.GetEnumDescription(enumValue);
        }
    }
}
