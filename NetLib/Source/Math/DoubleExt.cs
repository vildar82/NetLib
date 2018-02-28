﻿using System;
using JetBrains.Annotations;

namespace NetLib
{
    [PublicAPI]
    public static class DoubleExt
    {
        /// <summary>
        /// Сравнение двух чисел, допустимое расхождение 0.0000001 (6 нулей)
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static bool IsEqual6(this double value1, double value2)
        {
            return Math.Abs(value1 - value2) < 0.0000001;
        }

        /// <summary>
        /// Округление числа Math.Round
        /// </summary>
        /// <param name="value">Число</param>
        /// <param name="digits">Кол знаков округления</param>
        /// <returns>Округленное число</returns>
        public static double Round(this double value, int digits = 4)
        {
            return Math.Round(value, digits);
        }
    }
}