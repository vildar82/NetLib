using System.Collections;
using System.ComponentModel;
using JetBrains.Annotations;

namespace NetLib.WPF.Behaviors
{
    [PublicAPI]
    public interface ICustomSorter : IComparer
    {
        ListSortDirection SortDirection { get; set; }
        string SortPropertyName { get; set; }
    }
}