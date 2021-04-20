namespace NetLib.Notification
{
    using System;
    using System.Windows;
    using System.Windows.Threading;
    using ToastNotifications;
    using ToastNotifications.Core;
    using ToastNotifications.Lifetime;
    using ToastNotifications.Messages;
    using ToastNotifications.Position;

    public enum NotifyType
    {
        Information,
        Success,
        Warning,
        Error
    }

    public enum NotifyCorner
    {
        TopRight = 0,
        TopLeft = 1,
        BottomRight = 2,
        BottomLeft = 3,
        BottomCenter = 4
    }

    public class NotifyOptions
    {
        /// <summary>
        /// Настройки уведомления
        /// </summary>
        /// <param name="lifeTime">Время показа уведомления</param>
        /// <param name="parent">Родительское окно. Если null - то основной экран</param>
        /// <param name="corner">Расположение</param>
        /// <param name="offsetX">Отступ X</param>
        /// <param name="offsetY">Отступ Y</param>
        /// <param name="maxCount">Максимальное кол-во уведомлений</param>
        /// <param name="with">Ширина уведомления</param>
        public NotifyOptions(TimeSpan lifeTime = default, Window parent = null,
            NotifyCorner corner = NotifyCorner.TopRight, double offsetX = 50, double offsetY = 50, int maxCount = 3,
            double with = 250)
        {
            LifeTime = lifeTime;
            Parent = parent;
            Corner = corner;
            OffsetX = offsetX;
            OffsetY = offsetY;
            MaxCount = maxCount;
            With = with;
        }

        public TimeSpan LifeTime { get; set; }
        public Window? Parent { get; set; }
        public NotifyCorner Corner { get; set; }
        public double OffsetX { get; set; }
        public double OffsetY { get; set; }
        public int MaxCount { get; set; }
        public double With { get; set; }
    }

    public class NotifyMessageOptions
    {
        public double? FontSize { get; set; } = 14;
        public bool ShowCloseButton { get; set; } = true;
        public object Tag { get; set; }
        public bool FreezeOnMouseEnter { get; set; } = true;
        public Action NotificationClickAction { get; set; }
        public Action CloseClickAction { get; set; }
        public bool UnfreezeOnMouseLeave { get; set; } = true;
    }

    /// <summary>
    /// Уведомления - toast
    /// </summary>
    public class Notify
    {
        private static Notify notifyScreen;
        private readonly Notifier notifier;
        private static readonly Dispatcher dispatcher;

        static Notify()
        {
            dispatcher = Dispatcher.CurrentDispatcher;
            notifyScreen = new Notify(new NotifyOptions());
        }

        /// <summary>
        /// Настройка уведомлений на основном экране (без окна).
        /// </summary>
        /// <param name="opt">Настройки уведомлений</param>
        public static void SetScreenSettings(NotifyOptions opt)
        {
            notifyScreen = new Notify(opt);
        }

        /// <summary>
        /// Создание уведомлений по настройкам
        /// </summary>
        /// <param name="opt"></param>
        public Notify(NotifyOptions opt)
        {
            notifier = CreateNotifier(opt);
        }

        /// <summary>
        /// Показать уведомление на основном экране с дефолтными настройками
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="type">Тип</param>
        /// <param name="msgOpt">настройки сообщения</param>
        public static void ShowOnScreen(string message, NotifyType type = NotifyType.Information, NotifyMessageOptions? msgOpt = null)
        {
            Show(message, notifyScreen, msgOpt, type);
        }

        /// <summary>
        /// Показать уведомление
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="type">Тип</param>
        /// <param name="msgOpt">настройки сообщения</param>
        public void Show(string message, NotifyType type = NotifyType.Information, NotifyMessageOptions? msgOpt = null)
        {
            Show(message, this, msgOpt, type);
        }

        private static void Show(string message, Notify notify, NotifyMessageOptions? nMsgOpt, NotifyType type)
        {
            if (nMsgOpt == null) nMsgOpt = new NotifyMessageOptions();
            var msgOpt = new MessageOptions
            {
                CloseClickAction = n => { nMsgOpt.CloseClickAction?.Invoke(); },
                UnfreezeOnMouseLeave = nMsgOpt.UnfreezeOnMouseLeave,
                FontSize = nMsgOpt.FontSize,
                FreezeOnMouseEnter = nMsgOpt.FreezeOnMouseEnter,
                ShowCloseButton = nMsgOpt.ShowCloseButton,
                Tag = nMsgOpt.Tag,
                NotificationClickAction = n =>
                {
                    n.CanClose = true;
                    n.Close();
                    nMsgOpt.NotificationClickAction?.Invoke();
                },
            };
            switch (type)
            {
                case NotifyType.Information:
                    notify.notifier.ShowInformation(message, msgOpt);
                    break;
                case NotifyType.Success:
                    notify.notifier.ShowSuccess(message, msgOpt);
                    break;
                case NotifyType.Warning:
                    notify.notifier.ShowWarning(message, msgOpt);
                    break;
                case NotifyType.Error:
                    notify.notifier.ShowError(message, msgOpt);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private static Notifier CreateNotifier(NotifyOptions opt)
        {
            return new Notifier(cfg =>
            {
                if (opt.Parent == null)
                {
                    cfg.PositionProvider = new PrimaryScreenPositionProvider(
                        (Corner)opt.Corner, opt.OffsetX, opt.OffsetY);
                }
                else
                {
                    cfg.PositionProvider = new WindowPositionProvider(
                        opt.Parent, (Corner)opt.Corner, opt.OffsetX, opt.OffsetY);
                }

                if (opt.LifeTime.TotalMilliseconds < 100)
                {
                    cfg.LifetimeSupervisor = new CountBasedLifetimeSupervisor(MaximumNotificationCount.FromCount(opt.MaxCount));
                }
                else
                {
                    cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                        opt.LifeTime,
                        MaximumNotificationCount.FromCount(opt.MaxCount));
                }

                cfg.Dispatcher = dispatcher;
                cfg.DisplayOptions.Width = opt.With;
            });
        }
    }
}
