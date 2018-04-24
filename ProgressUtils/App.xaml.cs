using System.Windows;
using System.Windows.Media;

namespace ProgressUtils
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var accent = new SolidColorBrush(Color.FromRgb(75,75,45));
            Resources.Add("AccentColorBrush", accent);
        }
    }
}
