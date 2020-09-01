namespace NetLib.WPF.Behaviors
{
    using System.ComponentModel;
    using Comparers;
    using JetBrains.Annotations;

    [PublicAPI]
    public class AlphaNumSorter : ICustomSorter
    {
        public ListSortDirection SortDirection { get; set; }
        public string SortPropertyName { get; set; }

        public int Compare(object x, object y)
        {
            var strX = x?.GetType().GetProperty(SortPropertyName)?.GetValue(x)?.ToString();
            var strY = y?.GetType().GetProperty(SortPropertyName)?.GetValue(y)?.ToString();
            return SortDirection == ListSortDirection.Ascending
                ? AlphanumComparator.New.Compare(strX, strY)
                : AlphanumComparator.New.Compare(strY, strX);
        }
    }
}