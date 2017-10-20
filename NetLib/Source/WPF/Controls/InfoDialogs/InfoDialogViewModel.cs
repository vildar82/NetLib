using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroMvvm;

namespace NetLib.WPF.Controls.InfoDialogs
{
    internal class InfoDialogViewModel : ViewModelBase
    {
        public InfoDialogViewModel(string title, string msg)
        {
            Title = title;
            Message = msg;
        }

        public string Title { get; }
        public string Message { get; }
    }
}
