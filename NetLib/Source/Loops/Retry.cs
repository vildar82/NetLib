namespace NetLib
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// Повторение операции, за заданное время и количество попыток.
    /// </summary>
    public static class Retry
    {
        /// <summary>
        /// Попытки выполнить операцию
        /// </summary>
        /// <param name="action">Операция</param>
        /// <param name="retryInterval">Интервал повторения</param>
        /// <param name="retryCount">Кол попыток</param>
        public static void Do(Action action, TimeSpan retryInterval, int retryCount = 3)
        {
            Do<object>(
                () =>
            {
                action();
                return null;
            }, retryInterval,
                retryCount);
        }

        /// <summary>
        /// Попытка выполнить операцию
        /// </summary>
        /// <typeparam name="T">Возвращаемое значение</typeparam>
        /// <param name="action">Операция</param>
        /// <param name="retryInterval">Интервал повторения</param>
        /// <param name="retryCount">Кол попыток</param>
        /// <returns>Результат операции</returns>
        public static T Do<T>(Func<T> action, TimeSpan retryInterval, int retryCount = 3)
        {
            var exceptions = new List<Exception>();
            for (var retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    if (retry > 0)
                        Thread.Sleep(retryInterval);

                    return action();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            throw new AggregateException(exceptions);
        }
    }
}