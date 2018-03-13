using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NLog;

namespace NetLib.Monad
{
    public static class MonadExt
    {
        private static ILogger Logger { get; } = LogManager.GetCurrentClassLogger();

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

        public static TRes Try<T,TRes>(this T obj, Func<T, TRes> func, Func<Exception, TRes> exception)
        {
            try
            {
                return func(obj);
            }
            catch (Exception ex)
            {
                return exception(ex);
            }
        }
    }
}
