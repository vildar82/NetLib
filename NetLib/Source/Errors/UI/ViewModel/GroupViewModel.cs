using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MicroMvvm;
using NetLib.WPF;

namespace NetLib.Errors.UI.ViewModel
{
    public class GroupViewModel : BaseViewModel
    {
        public GroupViewModel(string group, List<IError> errors)
        {
            Group = group;
            Errors = new List<ErrorViewModel>(errors.Select(s=> new ErrorViewModel(s)));
            Icon = Errors.First().Icon;
        }

        public string Group { get; set; }
        public List<ErrorViewModel> Errors { get; set; }
        public Control Icon { get; set; }
    }
}
