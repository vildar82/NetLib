namespace NetLib.WPF.Theme
{
    using Pik.Metro;

    public class ThemeViewModel : BaseModel
    {
        public ThemeViewModel(Theme theme)
        {
            Theme = theme;
            Name = theme.Name;
        }

        public Theme Theme { get; set; }
        public string Name { get; set; }
    }
}
