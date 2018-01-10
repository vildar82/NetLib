﻿using JetBrains.Annotations;
using MahApps.Metro;
using ReactiveUI;

namespace NetLib.WPF.Theme
{
    public class ThemeViewModel : ReactiveObject
    {
        public ThemeViewModel([NotNull] AppTheme theme)
        {
            Theme = theme;
            Name = theme.Name;
        }

        public AppTheme Theme { get; set; }
        public string Name { get; set; }
    }
}
