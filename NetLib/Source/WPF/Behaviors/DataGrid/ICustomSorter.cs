using System.Collections;
using System.ComponentModel;

namespace NetLib.WPF.Behaviors
{
	public interface ICustomSorter : IComparer
	{
		ListSortDirection SortDirection { get; set; }
		string SortPropertyName { get; set; } 
	}
}