namespace NetLib
{
    using System.Collections.Generic;

    /// <summary>
    /// Нумерация осей
    /// </summary>
    public static class AlhpabeticAxis
    {
        private static List<char> _letters = new List<char>
        {
            'А',
            'Б',
            'В',
            'Г',
            'Д',
            'Е',
            'Ж',
            'З',
            'И',
            'К',
            'Л',
            'М',
            'Н',
            'О',
            'П',
            'Р',
            'С',
            'Т',
            'У',
            'Ф',
            'Х',
            'Ц',
            'Ч',
            'Ш',
            'Э',
            'Ю',
            'Я',
        };

        /// <summary>
        /// Название оси по индексу
        /// </summary>
        /// <param name="index">Индекс с 0</param>
        private static string GetName(int index)
        {
            if (index < _letters.Count)
                return _letters[index].ToString();

            var multiply = GetName((index / _letters.Count) - 1);
            var sign = GetName(index % _letters.Count);

            return multiply + sign;
        }
    }
}