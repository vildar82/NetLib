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
            var res = NetLib.IO.Path.GetTempFile(".xlsx");
            
            Console.WriteLine(res);
            Console.ReadKey();
        }
    }   
}
