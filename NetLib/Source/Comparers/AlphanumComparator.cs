﻿namespace NetLib.Comparers
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;

    /// <summary>
    /// Сортировка строк с числами.
    /// http://www.dotnetperls.com/alphanumeric-sorting
    /// </summary>
    public class AlphanumComparator : IComparer<string>
    {
        public static AlphanumComparator New { get; } = new AlphanumComparator();

        public int Compare(string s1, string s2)
        {
            var null1 = string.IsNullOrEmpty(s1);
            var null2 = string.IsNullOrEmpty(s2);
            if (null1)
            {
                return null2 ? 0 : -1;
            }
            if (null2)
            {
                return 1;
            }

            var len1 = s1.Length;
            var len2 = s2.Length;
            var marker1 = 0;
            var marker2 = 0;

            // Walk through two the strings with two markers.
            while (marker1 < len1 && marker2 < len2)
            {
                var ch1 = s1[marker1];
                var ch2 = s2[marker2];

                // Some buffers we can build up characters in for each chunk.
                var space1 = new char[len1];
                var loc1 = 0;
                var space2 = new char[len2];
                var loc2 = 0;

                // Walk through all following characters that are digits or
                // characters in BOTH strings starting at the appropriate marker.
                // Collect char arrays.
                do
                {
                    space1[loc1++] = ch1;
                    marker1++;

                    if (marker1 < len1)
                    {
                        ch1 = s1[marker1];
                    }
                    else
                    {
                        break;
                    }
                } while (char.IsDigit(ch1) == char.IsDigit(space1[0]));

                do
                {
                    space2[loc2++] = ch2;
                    marker2++;

                    if (marker2 < len2)
                    {
                        ch2 = s2[marker2];
                    }
                    else
                    {
                        break;
                    }
                } while (char.IsDigit(ch2) == char.IsDigit(space2[0]));

                // If we have collected numbers, compare them numerically.
                // Otherwise, if we have strings, compare them alphabetically.
                var str1 = new string(space1).Trim('\0');
                var str2 = new string(space2).Trim('\0');
                int result;

                if (char.IsDigit(space1[0]) && char.IsDigit(space2[0]))
                {
                    try
                    {
                        var thisNumericChunk = int.Parse(str1);
                        var thatNumericChunk = int.Parse(str2);
                        result = thisNumericChunk.CompareTo(thatNumericChunk);
                    }
                    catch
                    {
                        result = string.Compare(str1, str2, StringComparison.OrdinalIgnoreCase);
                    }
                }
                else
                {
                    result = string.Compare(str1, str2, StringComparison.OrdinalIgnoreCase);
                }
                if (result != 0)
                {
                    return result;
                }
            }
            return len1 - len2;
        }

        [PublicAPI]
        public int GetHashCode([CanBeNull] string obj)
        {
            return obj == null ? 0 : obj.GetHashCode();
        }
    }
}