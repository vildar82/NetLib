using JetBrains.Annotations;
using NCalc;
using NetLib.Comparers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NetLib
{
    [PublicAPI]
    public static class MathExt
    {
        /// <summary>
        /// 2 ПИ - 6.28...радиан, (360 градусов)
        /// </summary>
        public const double PI2 = Math.PI * 2;

        /// <summary>
        /// Пи/2 - 1,57...радиан, (90 градусов)
        /// </summary>
        public const double PIHalf = Math.PI * 0.5;

        /// <summary>
        /// Пи/4 - 0,78...радиан, (45 градусов)
        /// </summary>
        public const double PIQuart = Math.PI * 0.25;

        /// <summary>
        /// ПИ/180 - для преобразования градусов в радианы
        /// </summary>
        public const double RatioDegreeToRadian = Math.PI / 180;

        /// <summary>
        /// 180/Пи - для преобразования радиан в градусы
        /// </summary>
        public const double RatioRadianToDegree = 180 / Math.PI;

        public static readonly DoubleEqualityComparer AngleComparer = new DoubleEqualityComparer();

        /// <summary>
        /// Bytes to readable string.
        /// </summary>
        /// <param name="byteCount">The byte count.</param>
        /// <returns>5KB</returns>
        public static string BytesToString(this long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            var bytes = Math.Abs(byteCount);
            var place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            var num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return Math.Sign(byteCount) * num + suf[place];
        }

        /// <summary>
        /// Линейная интерполяция
        /// </summary>
        /// <param name="x0">Начальное X</param>
        /// <param name="y0">Начальное Y</param>
        /// <param name="x1">Конечное X</param>
        /// <param name="y1">Конечное Y</param>
        /// <param name="x">Промежуточное X для котрого нужно интерполировать Y</param>
        /// <returns></returns>
        public static double Interpolate(double x0, double y0, double x1, double y1, double x)
        {
            return y0 * (x - x1) / (x0 - x1) + y1 * (x - x0) / (x1 - x0);
        }

        public static object EvaluateString(string expression)
        {
            var expr = new Expression(expression);
            expr.EvaluateParameter += (s, p) =>
            {
                if (s.Equals("Pi"))
                {
                    p.Result = Math.PI;
                }
            };
            return expr.Evaluate();
        }

        /// <summary>
        /// Приведение угла к значению от 0 до 2PI (один оборот)
        /// </summary>
        /// <param name="angle">Исходный угол в радианах</param>
        /// <returns>Угол в радианах соответствующий исходному углу, но в пределах от 0 до 2pi (круг)</returns>
        public static double FixedAngle(this double angle)
        {
            if (angle < 0)
            {
                angle = PI2 + angle % PI2;
            }
            else if (angle >= PI2)
            {
                angle %= PI2;
            }
            return angle;
        }

        [Obsolete("Use StringExt.GetStartInteger()")]
        public static int GetStartInt([NotNull] this string input)
        {
            var res = input.GetStartInteger();
            if (res.Success)
            {
                return res.Value;
            }
            throw new Exception(res.Error);
        }

        /// <summary>
        /// Список чисел в строку, с групперовкой последовательных номеров
        /// int's = 1, 2, 3, 4, 5, 7, 8, 10, 15, 16, 100, 101, 102, 103, 105, 106, 107, 109
        /// res = "1-8, 10, 15, 16, 100-107, 109"
        /// </summary>
        [NotNull]
        public static string IntsToStringSequence([NotNull] int[] ints)
        {
            var uniqints = ints.Distinct().ToList();
            var res = string.Empty;
            var seq = new IntSequence(uniqints.First());
            foreach (var n in uniqints.Skip(1))
            {
                if (!seq.AddInt(n))
                {
                    SetSeq(ref res, ref seq);
                    seq = new IntSequence(n);
                }
            }
            if (!seq.IsNull())
            {
                SetSeq(ref res, ref seq);
            }
            return res;
        }

        /// <summary>
        /// Список чисел в строку, с групперовкой последовательных номеров
        /// int's = 1, 2, 3, 4, 5, 7, 8, 10, 15, 16, 100, 101, 102, 103, 105, 106, 107, 109
        /// res = "1-8,10,15,16,100-107,109"
        /// </summary>
        /// <param name="ints"></param>
        /// <returns></returns>
        [NotNull]
        public static string IntsToStringSequenceAnton([NotNull] int[] ints)
        {
            // int[] paleNumbersInt = new[] { 1, 2, 3, 4, 5, 7, 8, 10, 15, 16, 100, 101, 102, 103, 105, 106, 107, 109 };
            // res = 1-8,10,15,16,100-107,109
            var isFirstEnter = true;
            var isWas = false;
            var mark = string.Empty;
            for (var i = 0; i < ints.Length; i++)
            {
                if (i == ints.Length - 1)
                {
                    if (mark.Length == 0)
                    {
                        mark += ints[i];
                        break;
                    }
                    if (mark[mark.Length - 1] != '-') mark += ",";
                    mark += ints[i];
                    break;
                }
                if (i == 0 || isFirstEnter)
                {
                    mark += ints[i].ToString();
                    isFirstEnter = false;
                    continue;
                }
                if (ints[i + 1] - ints[i] == 1)
                {
                    if (mark[mark.Length - 1] != '-') mark += "-";// "," + int's[i] + "-";
                    isWas = true;
                    continue;
                }
                if (mark[mark.Length - 1] != '-') mark += ",";
                if (!isWas) mark += ints[i] + ",";
                else
                {
                    isWas = false;
                    mark += ints[i] + ",";
                }
                isFirstEnter = true;
            }
            mark = mark.Replace(",,", ",");
            return mark;
        }

        /// <summary>
        /// Сравнение десятичных чисел с допуском
        /// </summary>
        /// <param name="d">Первое число</param>
        /// <param name="other">Второе число</param>
        /// <param name="precision">Допуск разницы</param>
        /// <returns>true - равны, false - не равны</returns>
        public static bool IsEqual(this double d, double other, double precision = double.Epsilon)
        {
            return Math.Abs(d - other) <= precision;
        }

        /// <summary>
        /// Это четное число
        /// </summary>
        public static bool IsEven(this int value)
        {
            return value % 2 == 0;
        }

        /// <summary>
        /// Это нечетное число
        /// </summary>
        public static bool IsOdd(this int value)
        {
            return value % 2 != 0;
        }

        /// <summary>
        /// Проверка - это ортогональный угол - 0,90,180,270,360 градусов.
        /// Допуск по умолчанию 1 градус - AngleComparer
        /// </summary>
        /// <param name="angleDeg">Угол в градусах</param>
        /// <returns>True если угол ортогональный, False - если нет.</returns>
        public static bool IsOrthoAngle(this double angleDeg)
        {
            return AngleComparer.Equals(angleDeg, 0) ||
                   AngleComparer.Equals(angleDeg, 90) ||
                   AngleComparer.Equals(angleDeg, 180) ||
                   AngleComparer.Equals(angleDeg, 270) ||
                   AngleComparer.Equals(angleDeg, 360);
        }

        /// <summary>
        /// Проверка - это ортогональный угол. И на сколько его нужно повернуть до ортогональности
        /// </summary>
        /// <param name="angle">Проверяемый угол</param>
        /// <param name="angteToOrtho">На сколько его нужно повернуть до ортогональности (положительный угол - против часовой стрелки)</param>
        /// <param name="isRadians">Угол в радианах</param>
        /// <param name="tolerance">Допуск</param>
        /// <returns>Да - это ортогональный угол. Нет - не ортогональный, и угол на который необходимо повернуть</returns>
        public static bool IsOrthoAngle(this double angle, out double angteToOrtho, bool isRadians = true, double tolerance = 0.01)
        {
            if (!isRadians)
            {
                angle = angle.ToRadians();
            }
            var p = angle % PIHalf;
            if (p < tolerance)
            {
                angteToOrtho = 0;
                return true;
            }
            var np = PIHalf - p;
            angteToOrtho = p < np ? -p : np;
            return false;
        }

        /// <summary>
        /// Это целое число
        /// </summary>
        /// <param name="value">Проверяемое значение</param>
        /// <param name="tolerance">Допуск</param>
        /// <returns>Да или нет - если от заданного значения до целого числа меньше либо равно допуску</returns>
        public static bool IsWholeNumber(this double value, double tolerance = 0.001)
        {
            return Math.Abs(value.Round(0) - value) <= tolerance;
            //return Math.Abs(value % 1) <= tolerance;
        }

        /// <summary>
        /// Превращает строки с диапазоном чисел в последовательность чисел.
        /// Например "1-5, 8,9" - {1,2,3,4,5,8,9}
        /// </summary>
        [NotNull]
        public static List<int> ParseRangeNumbers([NotNull] string text)
        {
            var query =
                from x in text.Split(',')
                let y = x.Split('-')
                let b = int.Parse(y[0].Trim())
                let e = int.Parse(y[y.Length - 1].Trim())
                from n in Enumerable.Range(b, e - b + 1)
                select n;
            return query.ToList();
        }

        /// <summary>
        /// Округление до 10
        /// </summary>
        public static int RoundTo10(int i)
        {
            if (i % 10 != 0)
            {
                i = (i + 5) / 10 * 10;
            }
            return i;
        }

        /// <summary>
        /// Округление до 100
        /// </summary>
        public static int RoundTo100(int i)
        {
            if (i % 100 != 0)
            {
                i = (i + 50) / 100 * 100;
            }
            return i;
        }

        /// <summary>
        /// Округление до 5 - вверх
        /// </summary>
        public static int RoundTo5(int i)
        {
            if (i % 5 != 0)
            {
                i = (i + 5) / 5 * 5;
            }
            return i;
        }

        /// <summary>
        /// Преобразование радиан в градусы (180.0*angleDegrees/Math.PI)
        /// </summary>
        /// <param name="radian">Угол в радианах</param>
        /// <returns>Угол в градусах</returns>
        public static double ToDegrees(this double radian)
        {
            return radian % PI2 * RatioRadianToDegree;// 180.0 / Math.PI;
        }

        public static double ToDouble(this string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                return 0;
            }
            val = val.Trim();
            try
            {
                var sep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                switch (sep)
                {
                    case ",":
                        val = val.Replace('.', ',');
                        break;

                    case ".":
                        val = val.Replace(',', '.');
                        break;
                }
            }
            catch
            {
                // ignored
            }
            if (!double.TryParse(val, out var d))
            {
                // ReSharper disable once StringIndexOfIsCultureSpecific.1
                val = val.IndexOf(",") != -1 ? val.Replace(",", ".") : val.Replace(".", ",");
                d = double.Parse(val);
            }
            return d;
        }

        [NotNull]
        public static string ToHours(this int min)
        {
            return ToHours((double)min);
        }

        [NotNull]
        public static string ToHours(this double min)
        {
            var span = TimeSpan.FromMinutes(min);
            var label = $"{span.Hours}ч.{span.Minutes}м.";
            return label;
        }

        public static double ToMin(this double h)
        {
            return h * 60;
        }

        /// <summary>
        /// Преобразование градусов в радианы (Math.PI / 180.0)*angleDegrees
        /// </summary>
        /// <param name="degrees">Угол в градусах</param>
        /// <returns>Угол в радианах</returns>
        public static double ToRadians(this double degrees)
        {
            return degrees * RatioDegreeToRadian;// (Math.PI / 180.0);
        }

        private static void SetSeq([NotNull] ref string res, ref IntSequence seq)
        {
            if (res == string.Empty)
            {
                res = seq.GetSeq();
            }
            else
            {
                res += ", " + seq.GetSeq();
            }
        }

        private struct IntSequence
        {
            private readonly int start;
            private int end;
            private bool has;

            public IntSequence(int start)
            {
                this.start = start;
                end = start;
                has = true;
            }

            public bool AddInt(int next)
            {
                if (next - end == 1)
                {
                    end = next;
                    return true;
                }
                return false;
            }

            [NotNull]
            public string GetSeq()
            {
                string res;
                has = false;
                if (end == start)
                {
                    res = start.ToString();
                }
                else if (end - start == 1)
                {
                    res = start + ", " + end;
                }
                else
                {
                    res = start + "-" + end;
                }
                return res;
            }

            public bool IsNull()
            {
                return !has;
            }
        }
    }
}