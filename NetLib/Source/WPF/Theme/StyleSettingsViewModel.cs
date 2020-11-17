namespace NetLib.WPF.Theme
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using Pik.Metro;
    using ReactiveUI;

    public class StyleSettingsViewModel : BaseViewModel
    {
        public StyleSettingsViewModel(IBaseViewModel? parent)
            : base(parent)
        {
            Themes = StyleSettings.GetThemes().OrderBy(o => o.Name).ToList();
            var (theme, find) = StyleSettings.GetWindowTheme(parent?.Window);
            SelectedTheme = Themes.FirstOrDefault(t => t == theme) ?? Themes.First();
            IsOnlyThisWindow = find;

            this.WhenAnyValue(w => w.SelectedTheme).Skip(1).Subscribe(s =>
            {
                StyleSettings.SaveWindowTheme(Parent.Window, s, IsOnlyThisWindow);
                ThemeManager.ChangeTheme(Window.Resources, s);
                if (parent?.Window != null)
                {
                    ThemeManager.ChangeTheme(parent.Window.Resources, s);
                    parent.Window.OnChangeTheme();
                }
            });
        }

        public List<Theme> Themes { get; set; }

        public Theme SelectedTheme { get; set; }

        public bool IsOnlyThisWindow { get; set; }

        public override void OnInitialize()
        {
            try
            {
                ThemeManager.ChangeTheme(Window.Resources, SelectedTheme);
            }
            catch
            {
                //
            }
        }

        public override void OnClosed()
        {
            StyleSettings.SaveWindowTheme(Parent.Window, SelectedTheme, IsOnlyThisWindow);
        }
    }
}