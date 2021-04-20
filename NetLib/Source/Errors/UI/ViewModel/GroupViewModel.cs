namespace NetLib.Errors.UI.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Controls;
    using WPF;

    public class GroupViewModel : BaseViewModel
    {
        public GroupViewModel(string group, List<IError> errors)
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
