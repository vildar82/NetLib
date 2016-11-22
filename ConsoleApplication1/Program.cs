using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetLib.Comparers;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var values = new List<string>() { "0112301", "1001301"};
            values.Sort(AlphanumComparator.New);
        }
    }
}
