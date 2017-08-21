﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetLib.Comparers;

namespace NetLib.WPF.Behaviors
{
	public class AlphaNumSorter : ICustomSorter
	{
		public ListSortDirection SortDirection { get; set; }

		public int Compare(object x, object y)
		{
			var strX = x?.ToString();
			var strY = y?.ToString();
			if (SortDirection == ListSortDirection.Ascending)
			{
				return AlphanumComparator.New.Compare(strX, strY);
			}
			return AlphanumComparator.New.Compare(strY, strX);
		}
	}
}
