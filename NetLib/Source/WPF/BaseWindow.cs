using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.IconPacks;
using MicroMvvm;
using NetLib.WPF.Theme;

namespace NetLib.WPF
{
    public class BaseWindow : MetroWindow
    {
        /// <summary>
        /// DataContext
        /// </summary>
        public BaseViewModel Model { get; set; }
        /// <summary>
        /// Изменилась тема оформления окна
        /// </summary>
        public event EventHandler ChangeTheme;

        [Obsolete("Лучше не использовать. Не забудь присвоить Model и Model.Window.")]
        public BaseWindow() : this(null)
        {

        }

        public BaseWindow(BaseViewModel model)
        {
            Model = model;
            if (Model != null) Model.Window = this;
            // Скрытие окна
            var hideBinding = new Binding("Hide");
            SetBinding(VisibilityHelper.IsHiddenProperty, hideBinding);
            // Регистрация окна в MahApps
            var dialogRegBinding = new Binding { Source = model };
            SetBinding(DialogParticipation.RegisterProperty, dialogRegBinding);
            // DialogResult
            var dialogResultBinding = new Binding("DialogResult");
            SetBinding(DialogCloser.DialogResultProperty, dialogResultBinding);

            WindowStartupLocation = model?.Parent?.Window == null
                ? WindowStartupLocation.CenterScreen
                : WindowStartupLocation.CenterOwner;
            Dispatcher.UnhandledException += Dispatcher_UnhandledException;

        }

        private void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            if (!(e.Exception is OperationCanceledException))
            {
                Model?.ShowMessage(e.Exception.Message);
            }
            e.Handled = true;
        }

        protected override void OnInitialized(EventArgs e)
        {
            DataContext = Model;
            AddStyleResouse();
            // Применение темы оформления
            ApplyTheme();
            // При изменении темы
            StyleSettings.Change += (s, a) =>
            {
                ApplyTheme();
                OnChangeTheme();
            };
            SaveWindowPosition = true;
            GlowBrush = Resources["AccentColorBrush"] as Brush;

            // Кнопка настроек темы оформления
            if (!(Model is StyleSettingsViewModel))
            {
                var buttonTheme = new Button
                {
                    Content = new PackIconOcticons { Kind = PackIconOcticonsKind.Paintcan },
                    ToolTip = "Настройка тем оформления окон"
                };
                buttonTheme.Click += ButtonTheme_Click;
                if (RightWindowCommands == null)
                {
                    RightWindowCommands = new WindowCommands();
                }
                RightWindowCommands.Items.Add(buttonTheme);
            }
            base.OnInitialized(e);
            Model?.OnInitialize();
        }

        internal void OnChangeTheme()
        {
            ChangeTheme?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void ApplyTheme()
        {
            StyleSettings.ApplyWindowTheme(this);
        }

        private void ButtonTheme_Click(object sender, RoutedEventArgs e)
        {
            var styleSettingsVM = new StyleSettingsViewModel(Model);
            var styleSettingsView = new StyleSettingsView(styleSettingsVM);
            styleSettingsView.ShowDialog();
        }

        protected override void OnClosed(EventArgs e)
        {
            Model?.OnClosed();
            base.OnClosed(e);
        }

        private void AddStyleResouse()
        {
            Resources.MergedDictionaries.Add(new ResourceDictionary()
            {
                Source = new Uri("pack://application:,,,/AcadLib;component/Model/WPF/Style.xaml")
            });
        }
    }
}
