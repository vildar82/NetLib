using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetLib.Comparers;
using NetLib;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            object value = 1;
            MyEnum res = value.GetValue<MyEnum>();
            
            Console.WriteLine(res);
            Console.ReadKey();
        }
    }

    public enum MyEnum
    {
        None,
        Default
    }
}
