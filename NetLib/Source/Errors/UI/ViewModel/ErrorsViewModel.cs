using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroMvvm;

namespace NetLib.Errors.UI.ViewModel
{
    public class ErrorsViewModel : ViewModelBase
    {
        public ErrorsViewModel(List<IError> errors, string title, bool isDialog)
        {
            CollapseDialogButtons = !isDialog;
            Title = title;
            Errors = new List<ViewModelBase>();
            foreach (var grouping in errors.GroupBy(g=>g.Group))
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
        public List<ViewModelBase> Errors { get; set; }
        public bool CollapseDialogButtons { get; set; }
        public bool Hide { get; set; }
    }
}
