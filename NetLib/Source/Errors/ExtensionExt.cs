namespace NetLib.Errors
{
    using System;
    using System.Linq;

    /// <summary>
    /// Расширения для исключений
    /// </summary>
    public static class ExceptionExt
    {
        /// <summary>
        /// Сообщение об ошибке включая вложенные исключения и аггркгатные.
        /// </summary>
        public static string GetMessageWithInner(this Exception ex)
        {
            var msg = ex.Message;
            var inner = ex.InnerException;
            while (inner != null)
            {
                msg = AppendMessage(msg, inner.Message);
                inner = inner.InnerException;
            }

            if (ex is AggregateException ag)
            {
                msg = ag.InnerExceptions.Aggregate(msg,
                    (current, agInner) => AppendMessage(current, GetMessageWithInner(agInner)));
            }

            return msg;
        }

        private static string AppendMessage(string message, string append)
        {
            return message.Contains(append) ? message : $"{message}{(message.EndsWith(".") ? "" : ".")} {append}";
        }
    }
}