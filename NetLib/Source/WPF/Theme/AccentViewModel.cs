using MahApps.Metro;
using ReactiveUI;

namespace NetLib.WPF.Theme
{
    public class AccentViewModel : ReactiveObject
    {
        public AccentViewModel(Accent accent)
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
