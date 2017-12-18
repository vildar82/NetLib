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
using PIK_GP_Civil.Parkings.Area;
using ReactiveUI;
using WpfApp1.Dialog;

namespace WpfApp1
{
    public class Model : BaseViewModel
    {
        private DialogView dialog;
        public ReactiveCommand Test { get; set; }

        public Model()
        {
            Test = CreateCommand(TestExec);
        }

        private void TestExec()
        {
            if (dialog == null)
            {
                dialog = new DialogView(new DialogVM(this));
            }
            dialog.Show();
        }
    }
}
