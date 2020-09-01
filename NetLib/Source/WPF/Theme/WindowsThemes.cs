namespace NetLib.WPF.Theme
{
    using System.Collections.Generic;

    public class WindowsThemes
    {
        public List<ApplicationThemes> Applications { get; set; } = new List<ApplicationThemes>();
    }

    /// <summary>
    /// Настройки тем окно в приложении
    /// </summary>
    public class ApplicationThemes
    {
        public string Name { get; set; }

        public List<WindowTheme> Windows { get; set; } = new List<WindowTheme>();

        public string Theme { get; set; }
    }
}
