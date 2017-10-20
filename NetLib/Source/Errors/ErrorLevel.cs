using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLib.Errors
{
    public enum ErrorLevel
    {
        /// <summary>
        /// Ошибка
        /// </summary>
        Error,
        /// <summary>
        /// Информация
        /// </summary>
        Info,
        /// <summary>
        /// Восклицание (неожиданное, непредвиденная ситуация)
        /// </summary>
        Exclamation
    }
}
