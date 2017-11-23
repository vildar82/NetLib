using FluentValidation;
using NetLib.WPF;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace WpfApp1
{
    public class ValueItem<T> : BaseViewModel
    {
        [Reactive] public T Value2 { get; set; }
    }
}