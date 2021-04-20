namespace NetLib.WPF.Behaviors
{
    using System.Collections;
    using System.ComponentModel;

    public interface ICustomSorter : IComparer
    {
        ListSortDirection SortDirection { get; set; }

        string SortPropertyName { get; set; }
    }
}