﻿using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.IconPacks;
using NetLib.WPF.Theme;
using NLog;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace NetLib.WPF
{
    public class BaseWindow : MetroWindow
    {
        private static readonly FieldInfo _showingAsDialogField = typeof(Window)
            .GetField("_showingAsDialog", BindingFlags.Instance | BindingFlags.NonPublic);

        protected static Logger Logger { get; } = LogManager.GetCurrentClassLogger();
        protected bool isDialog;
        private IBaseViewModel model;

        /// <summary>
        /// DataContext
        /// </summary>
        public IBaseViewModel Model
        {
            get => model;
            set
            {
                model = value;
                DataContext = model;
            }
        }

        /// <summary>
        /// Изменилась тема оформления окна
        /// </summary>
        public event EventHandler ChangeTheme;

        /// <summary>
        /// Закрытие окна по нажатия Enter или Space (пробел) - DialogResult true
        /// </summary>
        public bool CloseWindowByEnterOrSpace { get; set; } = true;
        /// <summary>
        /// Вызывать ли закрытие окна или нет. (Если сохранять в памяти и показывать снова)
        /// </summary>
        public bool IsUnclosing { get; set; }

        /// <summary>
        /// Дествие при нажатии OK/Space
        /// </summary>
        public Action OnEnterOrSpace { get; set; }

        [Obsolete("Лучше не использовать. Не забудь присвоить Model и Model.Window.")]
        public BaseWindow() : this(null)
        {

        }

        public BaseWindow(IBaseViewModel model)
        {
            AddStyleResouse();
            Model = model;
            if (Model != null)
            {
                Model.Window = this;
            }
            // Скрытие окна
            var hideBinding = new Binding("Hide");
            SetBinding(VisibilityHelper.IsHiddenProperty, hideBinding);
            // Регистрация окна в MahApps
            var dialogRegBinding = new Binding {Source = model};
            SetBinding(DialogParticipation.RegisterProperty, dialogRegBinding);
            // DialogResult
            var dialogResultBinding = new Binding("DialogResult");
            SetBinding(DialogCloser.DialogResultProperty, dialogResultBinding);

            if (model?.Parent?.Window != null)
            {
                Owner = model.Parent.Window;
                WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
            else
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
            Dispatcher.UnhandledException += Dispatcher_UnhandledException;
            // Скрыть кнопки свернуть/минимизировать
            ShowMinButton = false;
            ShowMaxRestoreButton = false;
            SaveWindowPosition = true;
            PreviewKeyDown += BaseWindow_PreviewKeyDown;
            MouseDown += BaseWindow_MouseDown;
            Activated += (s, a) => isDialog = (bool) _showingAsDialogField.GetValue(this);
        }

        private void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            if (!(e.Exception is OperationCanceledException))
            {
                Logger.Fatal(e.Exception, "UnhandledException");
                Model?.ShowMessage(e.Exception.Message);
            }
            e.Handled = true;
        }

        protected override void OnInitialized(EventArgs e)
        {
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
                    Content = new PackIconOcticons {Kind = PackIconOcticonsKind.Paintcan},
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

        protected override void OnClosing(CancelEventArgs e)
        {
            if (IsUnclosing)
            {
                e.Cancel = true;
                Hide();
            }
            else
            {
                base.OnClosing(e);
            }
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
                Source = new Uri("pack://application:,,,/NetLib;component/Source/Style.xaml")
            });
        }

        private void BaseWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var scope = FocusManager.GetFocusScope(this); // elem is the UIElement to unfocus
            FocusManager.SetFocusedElement(scope, null); // remove logical focus
            Keyboard.ClearFocus(); // remove keyboard focus
        }

        private void BaseWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (isDialog)
                {
                    DialogResult = false;
                }
                else
                {
                    Close();
                }
                e.Handled = true;
            }
            else if (CloseWindowByEnterOrSpace && 
                (e.Key == Key.Enter || FocusManager.GetFocusedElement(this) == null && e.Key == Key.Space))
            {
                OnEnterOrSpace?.Invoke();
                if (isDialog)
                {
                    DialogResult = true;
                }
                else
                {
                    Close();
                }
                e.Handled = true;
            }
        }
    }
}