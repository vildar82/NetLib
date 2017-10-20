using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MicroMvvm;
using NetLib.Errors;
using NetLib.WPF.Controls;

namespace WpfApp1
{
	public class Model : ViewModelBase
	{
	    public RelayCommand Start => new RelayCommand(StartExec);

	    private void StartExec()
	    {
	        var msg = File.ReadAllText(@"C:\temp\test\infoDialog.txt");
	        if (InfoDialog.ShowDialog("Продолжить?", msg))
	        {
	            
	        }

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
    }
}
