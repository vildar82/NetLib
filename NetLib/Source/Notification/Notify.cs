using System;
using System.Windows;
using System.Windows.Threading;
using JetBrains.Annotations;
using ToastNotifications;
using ToastNotifications.Core;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace NetLib.Notification
{
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
        public NotifyOptions(TimeSpan lifeTime = default, Window parent = null,
            NotifyCorner corner = NotifyCorner.TopRight, double offsetX = 50, double offsetY = 50, int maxCount = 5)
        {
            LifeTime = lifeTime == default ? TimeSpan.FromSeconds(5) : lifeTime;
            Parent = parent;
            Corner = corner;
            OffsetX = offsetX;
            OffsetY = offsetY;
            MaxCount = maxCount;
        }

        public TimeSpan LifeTime { get; set; } = default;
        public Window Parent { get; set; } = null;
        public NotifyCorner Corner { get; set; } = NotifyCorner.TopRight;
        public double OffsetX { get; set; } = 50;
        public double OffsetY { get; set; } = 150;
        public int MaxCount { get; set; } = 5;
    }

    public class NotifyMessageOptions
    {
        public double? FontSize { get; set; }
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
    [PublicAPI]
    public class Notify
    {
        private static readonly Notify notifyScreen;
        private readonly Notifier notifier;
        private static readonly Dispatcher dispatcher;

        static Notify()
        {
            dispatcher = Dispatcher.CurrentDispatcher;
            notifyScreen = new Notify(new NotifyOptions());
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
        public static void ShowScreenNotify(string message, NotifyType type = NotifyType.Information,
            [CanBeNull] NotifyMessageOptions msgOpt = null)
        {
            Show(message, notifyScreen, msgOpt, type);
        }

        /// <summary>
        /// Показать уведомление
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="type">Тип</param>
        /// <param name="msgOpt">настройки сообщения</param>
        public void ShowNotify(string message, NotifyType type = NotifyType.Information, 
            [CanBeNull] NotifyMessageOptions msgOpt = null)
        {
            Show(message, this, null, type);
        }

        private static void Show(string message, [NotNull] Notify notify, [CanBeNull] NotifyMessageOptions nMsgOpt, 
            NotifyType type)
        {
            MessageOptions msgOpt = null;
            if (nMsgOpt != null)
            {
                msgOpt = new MessageOptions
                {
                    CloseClickAction = n =>
                    {
                        n.Close();
                        nMsgOpt.CloseClickAction?.Invoke();
                    },
                    UnfreezeOnMouseLeave = nMsgOpt.UnfreezeOnMouseLeave,
                    FontSize = nMsgOpt.FontSize,
                    FreezeOnMouseEnter = nMsgOpt.FreezeOnMouseEnter,
                    ShowCloseButton = nMsgOpt.ShowCloseButton,
                    Tag = nMsgOpt.Tag,
                    NotificationClickAction = n =>
                    {
                        n.Close();
                        nMsgOpt.NotificationClickAction?.Invoke();
                    }
                };
            }
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

        [NotNull]
        private static Notifier CreateNotifier(NotifyOptions opt)
        {
            return new Notifier(cfg =>
            {
                if (opt.Parent == null)
                    cfg.PositionProvider = new PrimaryScreenPositionProvider(
                        (Corner) opt.Corner, opt.OffsetX, opt.OffsetY);
                else
                    cfg.PositionProvider = new WindowPositionProvider(
                        opt.Parent, (Corner) opt.Corner, opt.OffsetX, opt.OffsetY);
                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(opt.LifeTime,
                    MaximumNotificationCount.FromCount(opt.MaxCount));
                cfg.Dispatcher = dispatcher;
                cfg.DisplayOptions.Width = 400;
            });
        }
    }
}
