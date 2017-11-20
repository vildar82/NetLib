using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using MahApps.Metro;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace NetLib.WPF.Theme
{
    public class StyleSettingsViewModel : BaseViewModel
    {
        public StyleSettingsViewModel(IBaseViewModel parent) : base(parent)
        {
            Themes = StyleSettings.GetThemes().Select(s => new ThemeViewModel(s)).ToList();
            Accents = StyleSettings.Getaccents().Select(s => new AccentViewModel(s)).ToList();
            var windowTheme = StyleSettings.GetWindowTheme(parent?.Window);
            SelectedTheme = Themes.FirstOrDefault(t => t.Theme == windowTheme.Theme);
            SelectedAccent = Accents.FirstOrDefault(t => t.Accent == windowTheme.Accent);
            IsOnlyThisWindow = windowTheme.FindWindowTheme;
            
            this.WhenAnyValue(w => w.SelectedTheme, w => w.SelectedAccent).Skip(1).Subscribe(s =>
            {
                StyleSettings.SaveWindowTheme(Parent.Window, s.Item1.Theme, s.Item2.Accent, IsOnlyThisWindow);
                ThemeManager.ChangeAppStyle(Window.Resources, s.Item2.Accent, s.Item1.Theme);
                if (parent?.Window != null)
                {
                    ThemeManager.ChangeAppStyle(parent.Window.Resources, s.Item2.Accent, s.Item1.Theme);
                    parent.Window.OnChangeTheme();
                }
            });
        }

        public override void OnInitialize()
        {
            try
            {
                ThemeManager.ChangeAppStyle(Window.Resources, SelectedAccent.Accent, SelectedTheme.Theme);
            }
            catch
            {
                //
            }
        }

        public List<ThemeViewModel> Themes { get; set; }
        [Reactive] public ThemeViewModel SelectedTheme { get; set; }
        public List<AccentViewModel> Accents { get; set; }
        [Reactive] public AccentViewModel SelectedAccent { get; set; }
        [Reactive] public bool IsOnlyThisWindow { get; set; }

        public override void OnClosed()
        {
            StyleSettings.SaveWindowTheme(Parent.Window, SelectedTheme.Theme, SelectedAccent.Accent, IsOnlyThisWindow);
        }
    }
}
