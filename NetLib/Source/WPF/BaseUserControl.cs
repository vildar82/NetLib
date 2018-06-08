using System;
using System.Windows.Controls;
using JetBrains.Annotations;

namespace NetLib.WPF
{
    [PublicAPI]
    public class BaseUserControl : UserControl
    {
        private readonly bool applyTheme;
        private IBaseModel model;

        public BaseUserControl()
        {
            
        }

        public BaseUserControl(IBaseModel baseModel) : this(baseModel, true)
        {
            
        }

        public BaseUserControl(IBaseModel baseModel, bool applyTheme)
        {
            this.applyTheme = applyTheme;
            DataContext = baseModel;
            Model = baseModel;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            // При изменении темы
            if (applyTheme)
            {
                StyleSettings.Change += (s, a) => { ApplyTheme(); };
                BaseWindow.AddStyleResouse(Resources);
                ApplyTheme();
            }
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
