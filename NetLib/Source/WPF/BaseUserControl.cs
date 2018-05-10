using System;
using System.Windows.Controls;
using JetBrains.Annotations;

namespace NetLib.WPF
{
    [PublicAPI]
    public class BaseUserControl : UserControl
    {
        private IBaseModel model;

        public BaseUserControl()
        {
            
        }

        public BaseUserControl(IBaseModel baseModel)
        {
            DataContext = baseModel;
            Model = baseModel;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            // При изменении темы
            StyleSettings.Change += (s, a) =>
            {
                ApplyTheme();
            };
            BaseWindow.AddStyleResouse(Resources);
            ApplyTheme();
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
