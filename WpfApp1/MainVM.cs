using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetLib.WPF;

namespace WpfApp1
{
    public class MainVM : BaseViewModel
    {
        public List<string> Items { get; set; } = LoadItems();

        private static List<string> LoadItems()
        {
            return Enumerable.Range(1, 1000).Select(s => new string(Enumerable.Range(0, s).Select(c=> (char)c).ToArray())).ToList();
        }
    }
}
