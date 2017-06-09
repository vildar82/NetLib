using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NetLib.WPF.Controls
{
    /// <summary>
    /// Логика взаимодействия для TextView.xaml
    /// </summary>
    public partial class TextView 
    {
        public TextView(TextVM text)
        {
            InitializeComponent();
            DataContext = text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
