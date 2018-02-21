using JetBrains.Annotations;
using NetLib.WPF;
using System.Collections.Generic;
using System.Linq;

namespace NetLib.Errors.UI.ViewModel
{
    public class ErrorsViewModel : BaseViewModel
    {
        public ErrorsViewModel([NotNull] List<IError> errors, string title, bool isDialog)
        {
            CollapseDialogButtons = !isDialog;
            Title = title;
            Errors = new List<BaseViewModel>();
            foreach (var grouping in errors.GroupBy(g => g.Group))
            {
                if (grouping.Skip(1).Any())
                {
                    Errors.Add(new GroupViewModel(grouping.Key, grouping.ToList()));
                }
                else
                {
                    Errors.Add(new ErrorViewModel(grouping.First()));
                }
            }
        }

        public string Title { get; set; }
        public new List<BaseViewModel> Errors { get; set; }
        public bool CollapseDialogButtons { get; set; }
    }
}
