using System;
using System.Collections.Generic;

namespace NetLib.WPF.Controls.Select
{
	public static class SelectList
	{
		public static T Select<T>(List<SelectListItem<T>> items, string title, string name=null)
		{
			var selVM = new SelectListVM<T>(items, title, name);
			var selView = new SelectListView {DataContext = selVM};
			if (selView.ShowDialog() == true)
			{
				return selVM.Selected.Object;
			}
			throw new OperationCanceledException("Отменено пользователем");
		}
	}
}
