using MahApps.Metro;
using NetLib.IO;
using NetLib.WPF.Theme;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media;

namespace NetLib.WPF
{
    public static class StyleSettings
    {
        private static Logger Logger { get; } = LogManager.GetCurrentClassLogger();
        private static Accent accent;
        private static AppTheme theme;
        private static readonly WindowsThemes windowsThemes;
        private static readonly ApplicationThemes applicationTheme;

        static StyleSettings()
        {
            LoadThemesAndColors();
            windowsThemes = LoadWindowsThemes();
            if (windowsThemes.Applications == null)
            {
                windowsThemes.Applications = new List<ApplicationThemes>();
            }
            var applicationName = GetAppName();
            Debug.WriteLine($"applicationName={applicationName}");
            // перенос старой настройки тем автакада в настройки приложения
            if (applicationName.EqualsIgnoreCase("acad") && 
                !windowsThemes.Applications.Any(a=>a.Name.EqualsIgnoreCase("acad")))
            {
                applicationTheme = new ApplicationThemes
                {
                    Name = "acad",
#pragma warning disable 612
                    Accent = windowsThemes.Accent ?? "Blue",
                    Theme = windowsThemes.Theme ?? "BaseLight",
                    Windows = windowsThemes.Windows
#pragma warning restore 612
                };
                windowsThemes.Applications.Add(applicationTheme);
            }
            else
            {
                applicationTheme = windowsThemes.Applications.FirstOrDefault(a => a.Name.Equals(applicationName));
                if (applicationTheme == null)
                {
                    applicationTheme = new ApplicationThemes { Name = applicationName };
                    windowsThemes.Applications.Add(applicationTheme);
                }
            }
            accent = GetAccent(applicationTheme.Accent);
            theme = GetTheme(applicationTheme.Theme);
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

        public static event EventHandler Change;

        public static void ApplyWindowTheme(BaseWindow window)
        {
            var windowTheme = GetWindowTheme(window);
            ThemeManager.ChangeAppStyle(window.Resources, windowTheme.Accent, windowTheme.Theme);
        }

        internal static void SaveWindowTheme(BaseWindow window, AppTheme wTheme, Accent wAccent, bool isOnlyThisWindow)
        {
            try
            {
                if (isOnlyThisWindow && window != null)
                {
                    var windowTheme = FindWindowTheme(window);
                    if (windowTheme == null)
                    {
                        windowTheme = new WindowTheme {Window = GetWindowName(window)};
                        applicationTheme.Windows.Add(windowTheme);
                    }
                    windowTheme.Theme = wTheme.Name;
                    windowTheme.Accent = wAccent.Name;
                }
                else
                {
                    var windowTheme = FindWindowTheme(window);
                    if (windowTheme != null)
                    {
                        applicationTheme.Windows.Remove(windowTheme);
                    }
                    accent = wAccent;
                    theme = wTheme;
                    applicationTheme.Accent = accent.Name;
                    applicationTheme.Theme = theme.Name;
                    Change?.Invoke(null, EventArgs.Empty);
                }
                var file = GetWindowsThemesFile();
                windowsThemes.Serialize(file);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "SaveWindowTheme");
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
            return applicationTheme.Windows.FirstOrDefault(f => f.Window.Equals(wName));
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

        private static void LoadThemesAndColors()
        {
            try
            {
                ThemeManager.AddAppTheme("DarkBlue",new Uri("pack://application:,,,/NetLib;component/Source/WPF/Theme/DarkBlue.xaml"));
                ThemeManager.AddAppTheme("Gray",new Uri("pack://application:,,,/NetLib;component/Source/WPF/Theme/Gray.xaml"));
                ThemeManager.AddAccent("Gray",new Uri("pack://application:,,,/NetLib;component/Source/WPF/Theme/Accents/GrayAccent.xaml"));
                ThemeManager.AddAccent("mdAmber",new Uri("pack://application:,,,/NetLib;component/Source/WPF/Theme/Accents/mdAmber.xaml"));
                ThemeManager.AddAccent("mdBlue",new Uri("pack://application:,,,/NetLib;component/Source/WPF/Theme/Accents/mdBlue.xaml"));
                ThemeManager.AddAccent("mdBlueGrey",new Uri("pack://application:,,,/NetLib;component/Source/WPF/Theme/Accents/mdBlueGrey.xaml"));
                ThemeManager.AddAccent("mdBrown",new Uri("pack://application:,,,/NetLib;component/Source/WPF/Theme/Accents/mdBrown.xaml"));
                ThemeManager.AddAccent("mdCyan",new Uri("pack://application:,,,/NetLib;component/Source/WPF/Theme/Accents/mdCyan.xaml"));
                ThemeManager.AddAccent("mdDeepOrange",new Uri("pack://application:,,,/NetLib;component/Source/WPF/Theme/Accents/mdDeepOrange.xaml"));
                ThemeManager.AddAccent("mdDeepPurple",new Uri("pack://application:,,,/NetLib;component/Source/WPF/Theme/Accents/mdDeepPurple.xaml"));
                ThemeManager.AddAccent("mdGreen",new Uri("pack://application:,,,/NetLib;component/Source/WPF/Theme/Accents/mdGreen.xaml"));
                ThemeManager.AddAccent("mdGrey",new Uri("pack://application:,,,/NetLib;component/Source/WPF/Theme/Accents/mdGrey.xaml"));
                ThemeManager.AddAccent("mdIndigo",new Uri("pack://application:,,,/NetLib;component/Source/WPF/Theme/Accents/mdIndigo.xaml"));
                ThemeManager.AddAccent("mdLightBlue",new Uri("pack://application:,,,/NetLib;component/Source/WPF/Theme/Accents/mdLightBlue.xaml"));
                ThemeManager.AddAccent("mdLightGreen",new Uri("pack://application:,,,/NetLib;component/Source/WPF/Theme/Accents/mdLightGreen.xaml"));
                ThemeManager.AddAccent("mdLime",new Uri("pack://application:,,,/NetLib;component/Source/WPF/Theme/Accents/mdLime.xaml"));
                ThemeManager.AddAccent("mdOrange",new Uri("pack://application:,,,/NetLib;component/Source/WPF/Theme/Accents/mdOrange.xaml"));
                ThemeManager.AddAccent("mdPink",new Uri("pack://application:,,,/NetLib;component/Source/WPF/Theme/Accents/mdPink.xaml"));
                ThemeManager.AddAccent("mdPurple",new Uri("pack://application:,,,/NetLib;component/Source/WPF/Theme/Accents/mdPurple.xaml"));
                ThemeManager.AddAccent("mdRed",new Uri("pack://application:,,,/NetLib;component/Source/WPF/Theme/Accents/mdRed.xaml"));
                ThemeManager.AddAccent("mdTeal",new Uri("pack://application:,,,/NetLib;component/Source/WPF/Theme/Accents/mdTeal.xaml"));
                ThemeManager.AddAccent("mdYellow",new Uri("pack://application:,,,/NetLib;component/Source/WPF/Theme/Accents/mdYellow.xaml"));
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "LoadThemesAndColors");
            }
        }

        /// <summary>
        /// Determining Ideal Text Color Based on Specified Background Color
        /// http://www.codeproject.com/KB/GDI-plus/IdealTextColor.aspx
        /// </summary>
        /// <param name = "color">The bg.</param>
        /// <returns></returns>
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
    }
}
