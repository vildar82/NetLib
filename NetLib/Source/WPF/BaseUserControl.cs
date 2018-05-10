using System;
using System.Windows.Controls;
using JetBrains.Annotations;

namespace NetLib.WPF
{
    [PublicAPI]
    public class BaseUserControl : UserControl
    {
        private IBaseModel model;

        protected BaseUserControl(IBaseModel baseModel)
        {
            DataContext = baseModel;
            Model = baseModel;
            // При изменении темы
            StyleSettings.Change += (s, a) =>
            {
                ApplyTheme();
            };
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            BaseWindow.AddStyleResouse(Resources);
            StyleSettings.ApplyWindowTheme(this);
        }

        public IBaseModel Model
        {
            get => model;
            set
            {
                model = value;
                DataContext = value;
            }
        }

        protected void ApplyTheme()
        {
            StyleSettings.ApplyWindowTheme(this);
        }
    }
}
