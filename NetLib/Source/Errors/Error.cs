using System;

namespace NetLib.Errors
{
    /// <summary>
    /// Описание ошибки
    /// </summary>
    public class Error : IError
    {
        public string Group { get; set; }
        public string Message { get; set; }
        public ErrorLevel Level { get; set; }
        public Action<IError> DoubleClickAction { get; set; }
    }
}
