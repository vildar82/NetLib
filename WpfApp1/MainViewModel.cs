using NetLib.WPF;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.Threading;

namespace WpfApp1
{
    public class MainViewModel : BaseViewModel
    {
        public string Message { get; set; }

        public MainViewModel()
        {
            Message = "Продуктом обновлена поставка КПП/СПП/БПП от 24.01.2018.\r\nНаша библиотека и ТЭП соответствует поставке от 26.12.2017.\r\nВедутся работы по обновлению в плановом порядке.";
        }
    }
}