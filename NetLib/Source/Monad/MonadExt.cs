using System;
using JetBrains.Annotations;
using NLog;

namespace NetLib.Monad
{
    [PublicAPI]
    public static class MonadExt
    {
        private static ILogger Logger { get; } = LogManager.GetCurrentClassLogger();

        public static T To<T>(this object obj)
        {
            return (T) obj;
        }

        [CanBeNull]
        public static TRes With<T,TRes>([CanBeNull] this T obj, Func<T,TRes> func)
        {
            return obj == null ? default : func(obj);
        }

        public static void Try(this Action action, [CanBeNull] Action<Exception> exception = null)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (exception == null) Logger.Error(ex);
                else exception(ex);
            }
        }

        public static T Try<T>(this Func<T> action, Func<Exception, T> exception)
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                return exception(ex);
            }
        }

        /// <summary>
        /// Попытка работы с объектом, возвращается сам объект
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="obj">Объект</param>
        /// <param name="action">Работа</param>
        /// <param name="exception">обработка исключения при работе</param>
        /// <returns>Объект</returns>
        public static T Try<T>(this T obj, Action<T> action, [CanBeNull] Action<T, Exception> exception = null)
        {
            try
            {
                action(obj);
            }
            catch (Exception ex)
            {
                if (exception == null) Logger.Error(ex);
                else exception(obj, ex);
            }
            return obj;
        }

        /// <summary>
        /// Попытка работы с объектом, возвращается результат
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <typeparam name="TRes">Тип результата</typeparam>
        /// <param name="obj">Объект</param>
        /// <param name="func">Работа</param>
        /// <param name="exception">Обработка исключения</param>
        /// <returns></returns>
        public static TRes Try<T,TRes>(this T obj, Func<T, TRes> func, [CanBeNull] Func<Exception, TRes> exception=null)
        {
            try
            {
                return func(obj);
            }
            catch (Exception ex)
            {
                return exception == null ? default : exception(ex);
            }
        }
    }
}
