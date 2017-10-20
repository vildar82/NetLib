using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetLib.Comparers;
using NetLib;
using NetLib.Errors;

namespace ConsoleApplication1
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();

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
            
            Console.ReadKey();
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
