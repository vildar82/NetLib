using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
