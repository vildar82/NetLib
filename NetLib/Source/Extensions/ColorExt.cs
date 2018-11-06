namespace NetLib.Extensions
{
    using System.Drawing;
    using JetBrains.Annotations;

    [PublicAPI]
    public static class ColorExt
    {
        /// <summary>
        /// Соединение цветов
        /// </summary>
        /// <param name="from">Цвет 1</param>
        /// <param name="to">Цвет 2</param>
        /// <param name="percent">Процент смешивания</param>
        /// <returns>Смешанный цвет</returns>
        public static Color Mix(this Color from, Color to, double percent)
        {
            var amountFrom = 1 - percent;
            return Color.FromArgb(
                (int)(from.A * amountFrom + to.A * percent),
                (int)(from.R * amountFrom + to.R * percent),
                (int)(from.G * amountFrom + to.G * percent),
                (int)(from.B * amountFrom + to.B * percent));
        }
    }
}