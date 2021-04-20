namespace NetLib.WPF
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Windows.Controls;
    using System.Windows.Media;
    using NLog;
    using Pik.Metro;
    using Theme;
    using Path = IO.Path;

    public static class StyleSettings
    {
        private static Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        private static Pik.Metro.Theme theme;
        private static ApplicationThemes applicationTheme;
        private static readonly string applicationName;

        public static event EventHandler Change;

        static StyleSettings()
        {
            applicationName = GetAppName();
            var windowsThemes = LoadWindowsThemes();
            if (windowsThemes.Applications == null)
                windowsThemes.Applications = new List<ApplicationThemes>();

            applicationTheme = FindApplication(windowsThemes, applicationName);
            theme = GetTheme(applicationTheme?.Theme);
        }

        public static void ApplyWindowTheme(BaseWindow window)
        {
            var windowTheme = GetWindowTheme(window);
            ThemeManager.ChangeTheme(window.Resources, windowTheme.theme);
        }

        public static void ApplyWindowTheme(UserControl control)
        {
            var windowTheme = GetWindowTheme(control.GetType().FullName);
            ThemeManager.ChangeTheme(control.Resources, windowTheme.theme);
        }

        /// <summary>
        /// Determining Ideal Text Color Based on Specified Background Color
        /// http://www.codeproject.com/KB/GDI-plus/IdealTextColor.aspx
        /// </summary>
        /// <param name = "color">The bg.</param>
        public static Color IdealTextColor(Color color)
        {
            const int nThreshold = 105;
            var bgDelta = Convert.ToInt32(color.R * 0.299 + color.G * 0.587 + color.B * 0.114);
            var foreColor = 255 - bgDelta < nThreshold ? Colors.Black : Colors.White;
            return foreColor;
        }

        public static SolidColorBrush GetSolidColorBrush(Color color, double opacity = 1d)
        {
            var brush = new SolidColorBrush(color) { Opacity = opacity };
            brush.Freeze();
            return brush;
        }

        internal static void SaveWindowTheme(BaseWindow? window, Pik.Metro.Theme wTheme, bool isOnlyThisWindow)
        {
            try
            {
                var windowsThemes = LoadWindowsThemes();
                applicationTheme = FindApplication(windowsThemes, applicationName);
                if (applicationTheme == null)
                {
                    applicationTheme = new ApplicationThemes { Name = applicationName };
                    windowsThemes.Applications.Add(applicationTheme);
                }

                if (isOnlyThisWindow && window != null)
                {
                    var windowTheme = FindWindowTheme(window);
                    if (windowTheme == null)
                    {
                        windowTheme = new WindowTheme { Window = GetWindowName(window) };
                        applicationTheme.Windows.Add(windowTheme);
                        AddApplication(windowsThemes, applicationTheme);
                    }

                    windowTheme.Theme = wTheme.Name;
                }
                else
                {
                    var windowTheme = FindWindowTheme(window);
                    if (windowTheme != null)
                    {
                        applicationTheme.Windows.Remove(windowTheme);
                    }

                    theme = wTheme;
                    applicationTheme.Theme = theme.Name;
                    Change?.Invoke(null, EventArgs.Empty);
                }

                var file = GetWindowsThemesFile();
                Save(file, windowsThemes);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "SaveWindowTheme");
            }
        }

        private static void AddApplication(WindowsThemes windowsThemes, ApplicationThemes applicationThemes)
        {
            if (windowsThemes.Applications.All(a => a.Name != applicationThemes.Name))
            {
                windowsThemes.Applications.Add(applicationThemes);
            }
        }

        private static void Save(string file, WindowsThemes data)
        {
            try
            {
                data.Serialize(file);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        internal static (Pik.Metro.Theme theme, bool find) GetWindowTheme(BaseWindow window)
        {
            return GetWindowTheme(GetWindowName(window));
        }

        internal static (Pik.Metro.Theme theme, bool find) GetWindowTheme(string windowName)
        {
            Pik.Metro.Theme wTheme;

            var windowTheme = FindWindowTheme(windowName);
            bool findWindowTheme;
            if (windowTheme != null)
            {
                wTheme = GetTheme(windowTheme.Theme);
                findWindowTheme = true;
            }
            else
            {
                findWindowTheme = false;
                wTheme = theme;
            }

            return (wTheme, findWindowTheme);
        }

        private static WindowTheme? FindWindowTheme(string? wName)
        {
            if (wName == null)
                return null;
            return applicationTheme?.Windows?.FirstOrDefault(f => f.Window.Equals(wName));
        }

        private static WindowTheme? FindWindowTheme(BaseWindow? window)
        {
            if (window == null)
                return null;
            var wName = GetWindowName(window);
            return applicationTheme?.Windows?.FirstOrDefault(f => f.Window.Equals(wName));
        }

        private static string? GetWindowName(BaseWindow? window)
        {
            return window?.GetType().FullName;
        }

        private static Pik.Metro.Theme GetTheme(string? themeName)
        {
            try
            {
                return ThemeManager.GetTheme(themeName) ??
                       ThemeManager.GetTheme("Light.Cyan") ??
                       ThemeManager.Themes.First();
            }
            catch
            {
                return ThemeManager.Themes.First();
            }
        }

        private static WindowsThemes LoadWindowsThemes()
        {
            try
            {
                var file = GetWindowsThemesFile();
                if (File.Exists(file))
                {
                    return file.Deserialize<WindowsThemes>();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return new WindowsThemes();
        }

        private static string GetWindowsThemesFile()
        {
            return Path.GetUserPluginFile("AutoCAD\\Theme", "WindowsThemes.json");
        }

        public static IEnumerable<Pik.Metro.Theme> GetThemes()
        {
            return ThemeManager.Themes;
        }

        private static ApplicationThemes FindApplication(WindowsThemes windowsThemes, string appName)
        {
            return windowsThemes.Applications.FirstOrDefault(a => a.Name.Equals(appName));
        }

        private static string GetAppName()
        {
            try
            {
                return Process.GetCurrentProcess().ProcessName;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return "default";
        }
    }
}
