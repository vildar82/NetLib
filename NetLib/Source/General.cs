namespace NetLib
{
    using System;

    /// <summary>
    /// Основные значения
    /// </summary>
    public static class General
    {
        internal static readonly Random random = new Random();

        /// <summary>
        /// Название компании на англ.
        /// </summary>
        public static string CompanyNameShortEng { get; set; } = "PIK";
    }
}
