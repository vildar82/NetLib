using System;
using System.Collections.Generic;

namespace NetLib.WPF.Theme
{
    public class WindowsThemes
    {
        [Obsolete]
        public List<WindowTheme> Windows { get; set; } = new List<WindowTheme>();
        [Obsolete]
        public string Accent = "Blue";
        [Obsolete]
        public string Theme = "BaseLight";
        public List<ApplicationThemes> Applications { get; set; } = new List<ApplicationThemes>();
    }

    /// <summary>
    /// Настройки тем окно в приложении
    /// </summary>
    public class ApplicationThemes
    {
        public string Name { get; set; }
        public List<WindowTheme> Windows { get; set; } = new List<WindowTheme>();
        public string Accent = "Blue";
        public string Theme = "BaseLight";
    }
}
