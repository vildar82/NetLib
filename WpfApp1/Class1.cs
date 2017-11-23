using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FluentValidation;
using FluentValidation.Validators;
using NetLib.WPF;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace WpfApp1
{
    public class Class1 : BaseViewModel
    {
        public Class1() : base(null)
        {
            AllowCustomValue = true;
            IsRequired = true;
            var values = new[] { 1, 2, 3, 4, 5};
            Values = new List<ValueItem<double>> (values.Select(s => new ValueItem<double> {Value2 = s}));
            var selVal = Values[2];
            CustomValue = new ValueItem<double> { Value2 = selVal.Value2};
            CustomValue.WhenAnyValue(w => w.Value2).Subscribe(
                s =>
                {
                    Value1 = s;
                    this.RaisePropertyChanged(nameof(CustomValue));
                    this.RaiseErrorsChanged("Values");
                });
        }

        [Reactive] public bool AllowCustomValue { get; set; }
        public bool IsRequired { get; set; }
        public List<ValueItem<double>> Values { get; set; }
        [Reactive] public ValueItem<double> SelectedValue { get; set; }
        [Reactive] public ValueItem<double> CustomValue { get; set; }
        [Reactive] public double Value1 { get; set; }
    }

    public class Class1Validator : AbstractValidator<Class1>
    {
        public Class1Validator()
        {
            RuleFor(w=>w.CustomValue).Must(ValidateValue).OverridePropertyName("Values");
            //RuleFor(w => w.SelectedValue).Must(ValidateValue).OverridePropertyName("Values");
            //RuleFor(r => r.Value1).Must(ValidateValue1);
        }

        private bool ValidateValue1(Class1 o, double v, PropertyValidatorContext c)
        {
            if (v > 0 && v < 10)
            {
                return true;
            }
            c.Rule.MessageBuilder = m => "значение должно быть от 0 до 10.";
            return false;
        }

        private bool ValidateValue(Class1 o, ValueItem<double> v, PropertyValidatorContext c)
        {
            if (v == null || v.Value2 > 0 && v.Value2<10)
            {
                return true;
            }
            c.Rule.MessageBuilder = m => "значение должно быть от 0 до 10.";
            return false;
        }
    }
}
