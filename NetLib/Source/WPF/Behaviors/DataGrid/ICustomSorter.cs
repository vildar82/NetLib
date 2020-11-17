namespace NetLib.WPF.Behaviors
{
    using System.Collections;
    using System.ComponentModel;
    using JetBrains.Annotations;

    [PublicAPI]
    public interface ICustomSorter : IComparer
    {
        ListSortDirection SortDirection { get; set; }

        string SortPropertyName { get; set; }
    }
}