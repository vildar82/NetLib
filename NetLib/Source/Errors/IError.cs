﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLib.Errors
{
    public interface IError
    {
        /// <summary>
        /// Группировка ошибок
        /// </summary>
        string Group { get; set; }
        /// <summary>
        /// Сообщение
        /// </summary>
        string Message { get; set; }
        /// <summary>
        /// Тип ошибки
        /// </summary>
        ErrorLevel Level { get; set; }
        /// <summary>
        /// Действие при даблулике.
        /// </summary>
        Action<IError> DoubleClickAction { get; set; }
    }
}
