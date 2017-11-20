using System;
using System.Collections.Generic;
using System.Linq;
using MahApps.Metro;
using NetLib.IO;
using NetLib.WPF.Theme;

namespace NetLib.WPF
{
    public static class StyleSettings
    {
        private static Accent accent;
        private static AppTheme theme;

        static StyleSettings()
        {
            accent = GetAccent(Properties.Settings.Default.Accent);
            theme = GetTheme(Properties.Settings.Default.Theme);
            WindowsThemes = LoadWindowsThemes();
        }

        public static WindowsThemes WindowsThemes { get; set; }
        public static event EventHandler Change;

        public static void ApplyWindowTheme(BaseWindow window)
        {
            var windowTheme = GetWindowTheme(window);
            ThemeManager.ChangeAppStyle(window.Resources, windowTheme.Accent, windowTheme.Theme);
        }

        internal static void SaveWindowTheme(BaseWindow window, AppTheme wTheme, Accent wAccent, bool isOnlyThisWindow)
        {
            if (isOnlyThisWindow && window != null)
            {
                var windowTheme = FindWindowTheme(window);
                if (windowTheme == null)
                {
                    windowTheme = new WindowTheme { Window = GetWindowName(window) };
                    WindowsThemes.Windows.Add(windowTheme);
                }
                windowTheme.Theme = wTheme.Name;
                windowTheme.Accent = wAccent.Name;
                var file = GetWindowsThemesFile();
                    WindowsThemes.Serialize(file);
            }
            else
            {
                var windowTheme = FindWindowTheme(window);
                if (windowTheme != null)
                {
                    WindowsThemes.Windows.Remove(windowTheme);
                }
                accent = wAccent;
                theme = wTheme;
                Properties.Settings.Default.Accent = accent.Name;
                Properties.Settings.Default.Theme = theme.Name;
                Properties.Settings.Default.Save();
                Change?.Invoke(null, EventArgs.Empty);
            }
        }

        internal static (AppTheme Theme, Accent Accent, bool FindWindowTheme)
            GetWindowTheme(BaseWindow window)
        {
            AppTheme wTheme;
            Accent wAccent;
            var windowTheme = FindWindowTheme(window);
            bool findWindowTheme;
            if (windowTheme != null)
            {
                wTheme = GetTheme(windowTheme.Theme);
                wAccent = GetAccent(windowTheme.Accent);
                findWindowTheme = true;
            }
            else
            {
                findWindowTheme = false;
                wTheme = theme;
                wAccent = accent;
            }
            return (wTheme, wAccent, findWindowTheme);
        }

        private static WindowTheme FindWindowTheme(BaseWindow window)
        {
            if (window == null) return null;
            var wName = GetWindowName(window);
            return WindowsThemes.Windows.FirstOrDefault(f => f.Window.Equals(wName));
        }

        private static string GetWindowName(BaseWindow window)
        {
            return window?.GetType().FullName;
        }

        private static AppTheme GetTheme(string themeName)
        {
            try
            {
                var fTheme = ThemeManager.GetAppTheme(themeName);
                return fTheme ?? ThemeManager.GetAppTheme("BaseLight");
            }
            catch
            {
                return ThemeManager.AppThemes.First();
            }
        }

        private static Accent GetAccent(string accentName)
        {
            try
            {
                var fAccent = ThemeManager.GetAccent(accentName);
                return fAccent ?? ThemeManager.GetAccent("Blue");
            }
            catch
            {
                return ThemeManager.Accents.First();
            }
        }

        private static WindowsThemes LoadWindowsThemes()
        {
            try
            {
                var file = GetWindowsThemesFile();
                return file.Deserialize<WindowsThemes>();
            }
            catch
            {
                return new WindowsThemes();
            }
        }

        private static string GetWindowsThemesFile()
        {
            return Path.GetUserPluginFile("AutoCAD\\Theme", "WindowsThemes.json");
        }

        public static IEnumerable<AppTheme> GetThemes()
        {
            return ThemeManager.AppThemes;
        }

        public static IEnumerable<Accent> Getaccents()
        {
            return ThemeManager.Accents;
        }
    }
}
