using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace WpfApp1
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Color color1;
        private Brush colorBrush;

        public event PropertyChangedEventHandler PropertyChanged;

        public Color Color
        {
            get => color1;
            set { color1 = value; OnPropertyChanged(); }
        }

        public Brush ColorBrush
        {
            get => colorBrush;
            set { colorBrush = value; OnPropertyChanged(); }
        }

        public List<ColorNCS> ColorsNCS { get; set; }
        public RelayCommand SelectColor { get; set; }

        public MainViewModel()
        {
            ColorsNCS = GetColorsNCS();
            SelectColor = new RelayCommand(SelectColorExec);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static List<ColorNCS> GetColorsNCS()
        {
            var r = new Random();
            var colors = new List<ColorNCS>();
            for (var i = 0; i < 100; i++)
            {
                var color = new ColorNCS
                {
                    Color = Color.FromRgb((byte)r.Next(0, 255), (byte)r.Next(0, 255), (byte)r.Next(0, 255)),
                    Article = i.ToString(),
                    NCS = $"NCS-{i}"
                };
                color.ColorBrush = new SolidColorBrush(color.Color);
                colors.Add(color);
            }
            return colors;
        }

        private void SelectColorExec(object obj)
        {
            var colorNcs = (ColorNCS)obj;
            ColorBrush = colorNcs.ColorBrush;
        }
    }
}