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
        /// <param name="percent">Процент смешивания - от 0 до 1</param>
        /// <returns>Смешанный цвет</returns>
        public static Color Mix(this Color from, Color to, double percent)
        {
            var amountFrom = 1 - percent;
            return Color.FromArgb(
                (byte)(from.A * amountFrom + to.A * percent),
                (byte)(from.R * amountFrom + to.R * percent),
                (byte)(from.G * amountFrom + to.G * percent),
                (byte)(from.B * amountFrom + to.B * percent));
        }
    }
}