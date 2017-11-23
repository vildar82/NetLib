using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MicroMvvm;
using NetLib.Errors;
using NetLib.WPF;
using NetLib.WPF.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Validar;

namespace WpfApp1
{
    public class Model : BaseViewModel
    {
        [Reactive] public double? Value { get;set; }
        public RelayCommand Start => new RelayCommand(StartExec);
        [Reactive] public ObservableCollection<double> Values { get; set; }
        public Class1 Class1 { get; set; }
        public string Layer { get; set; } = "fgh_gfh";

        public Model()
        {
            Class1 = new Class1();
	        Value = 1111;
            Values = new ObservableCollection<double>(new [] {0d,1d,2d,3d,4d,5d}) ;
	        Values.CollectionChanged += (s, a) =>
	        {
	            if (Math.Abs(Values.Last()) > 0.0001)
	            {
	                Values.Add(0);
	            }
            };
	        this.WhenAnyValue(w => w.Values).Subscribe(s =>
	        {
	            if (Math.Abs(Values.Last()) > 0.0001)
	            {
	                Values.Add(0);
	            }
            });
        }

	    private void StartExec()
	    {
	        var msg = File.ReadAllText(@"C:\temp\test\infoDialog.txt");
	        if (InfoDialog.ShowDialog("Продолжить?", msg))
	        {
	            
	        }
	        TestErrors();

	        //var inspector = new Inspector();
	        //GenerateErrors(inspector, "Тестовая группа ошибок 1", "Сообщение об ошибке", 100, ErrorLevel.Exclamation);
	        //GenerateErrors(inspector, "Тестовая группа ошибок 2",
	        //    "Представляет скрытое содержимое, раскрывающееся по нажатию мышкой на указатель в виде стрелки. " +
	        //    "Причем содержимое опять же может быть самым разным: кнопки, текст, картинки и т.д.",
	        //    100, ErrorLevel.Info);
	        //GenerateErrors(inspector, "Тестовая группа ошибок 3",
	        //    "Осталось добавить обработчик нажатия кнопки Click для обработки заказа и можно заказывать блюда.",
	        //    500, ErrorLevel.Error);

	        //inspector.Show();
	    }

	    private static void GenerateErrors(Inspector inspector, string group, string msg, int count, ErrorLevel level)
	    {
	        for (var i = 0; i < count; i++)
	        {
	            var err = new Error
	            {
	                Message = msg + i,
	                Group = group,
	                Level = level
	            };
	            inspector.AddError(err);
	        }
	    }

        private void TestErrors()
	    {
	        var inspector = new Inspector();
	        GenerateErrors(inspector, "Тестовая группа ошибок 1", "Сообщение об ошибке", 100, ErrorLevel.Exclamation);
	        GenerateErrors(inspector, "Тестовая группа ошибок 2",
	            "Представляет скрытое содержимое, раскрывающееся по нажатию мышкой на указатель в виде стрелки. " +
	            "Причем содержимое опять же может быть самым разным: кнопки, текст, картинки и т.д.",
	            100, ErrorLevel.Info);
	        GenerateErrors(inspector, "Тестовая группа ошибок 3",
	            "Осталось добавить обработчик нажатия кнопки Click для обработки заказа и можно заказывать блюда.",
	            500, ErrorLevel.Error);
	        inspector.Show();
	    }
    }
}
