namespace NetLib.WPF
{
    using System;
    using System.Windows.Controls;
    using JetBrains.Annotations;

    [PublicAPI]
    public class BaseUserControl : UserControl
    {
        private readonly bool applyTheme;
        private IBaseModel model;

        public BaseUserControl()
            : this(null, true)
        {
        }

        public BaseUserControl(IBaseModel baseModel)
            : this(baseModel, true)
        {
        }

        public BaseUserControl(IBaseModel baseModel, bool applyTheme)
        {
            this.applyTheme = applyTheme;
            DataContext = baseModel;
            Model = baseModel;
            BaseWindow.AddStyleResouse(Resources);
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
