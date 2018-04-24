using JetBrains.Annotations;
using MahApps.Metro;

namespace NetLib.WPF.Theme
{
    public class AccentViewModel : BaseModel
    {
        public AccentViewModel([NotNull] Accent accent)
        {
            Accent = accent;
            Name = accent.Name;
            Color = accent.Resources["AccentColorBrush"];
        }

        public Accent Accent { get; set; }
        public string Name { get; set; }
        public object Color { get; set; }
    }
}
