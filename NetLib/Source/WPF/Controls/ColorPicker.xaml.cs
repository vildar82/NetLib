namespace NetLib.WPF.Controls
{
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Forms;

    public partial class ColorPicker : INotifyPropertyChanged
    {
        /// <summary>
        /// Цвет (AutoCAD)
        /// </summary>
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register(
                "Color",
                typeof(Color),
                typeof(ColorPicker),
                new FrameworkPropertyMetadata(Color.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        private bool canClearColor;

        public ColorPicker()
        {
            InitializeComponent();
            CanClearColor = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool CanClearColor
        {
            get => canClearColor;
            set
            {
                if (value == canClearColor)
                    return;
                canClearColor = value;
                OnPropertyChanged();
            }
        }
        
        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set
            {
                SetValue(ColorProperty, value);
                CanClearColor = Color != null;
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            Color = Color.Empty;
        }

        private void SelectColor(object sender, RoutedEventArgs e)
        {
            var colorDlg = new ColorDialog { Color = Color };
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                Color = colorDlg.Color;
            }
        }
    }
}
