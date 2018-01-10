using JetBrains.Annotations;
using MahApps.Metro.IconPacks;
using NetLib.WPF;
using System.Windows.Controls;
using System.Windows.Media;

namespace NetLib.Errors.UI.ViewModel
{
    public class ErrorViewModel : BaseViewModel
    {
        public ErrorViewModel([NotNull] IError error)
        {
            Message = error.Message;
            Icon = GetIcon(error);
        }

        public string Message { get; set; }
        public Control Icon { get; set; }

        [CanBeNull]
        private static Control GetIcon([NotNull] IError error)
        {
            switch (error.Level)
            {
                case ErrorLevel.Error:
                    return new PackIconOcticons
                    {
                        Kind = PackIconOcticonsKind.Stop,
                        Background = Brushes.Red
                    };
                case ErrorLevel.Info:
                    return new PackIconOcticons
                    {
                        Kind = PackIconOcticonsKind.Info,
                        Background = Brushes.DeepSkyBlue
                    };
                case ErrorLevel.Exclamation:
                    return new PackIconOcticons
                    {
                        Kind = PackIconOcticonsKind.Alert,
                        Background = Brushes.Yellow
                    };
                default: return null;
            }
        }
    }
}
