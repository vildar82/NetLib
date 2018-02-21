using JetBrains.Annotations;
using NetLib.WPF;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace NetLib.Errors.UI.ViewModel
{
    public class GroupViewModel : BaseViewModel
    {
        public GroupViewModel(string group, [NotNull] List<IError> errors)
        {
            Group = group;
            Errors = new List<ErrorViewModel>(errors.Select(s => new ErrorViewModel(s)));
            Icon = Errors.First().Icon;
        }

        public string Group { get; set; }
        public new List<ErrorViewModel> Errors { get; set; }
        public Control Icon { get; set; }
    }
}
