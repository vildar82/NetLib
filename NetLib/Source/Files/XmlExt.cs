using JetBrains.Annotations;

namespace NetLib
{
    [PublicAPI]
    public static class XmlExt
    {
        /// <summary>
        /// Серилизовать объект в Xml
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="obj">Объект</param>
        /// <param name="file">Файл</param>
        public static void ToXml<T>([NotNull] this T obj, string file)
        {
            SerializerXml.Save(file, obj);
        }

        /// <summary>
        /// Десирилизовать объект из файла xml
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="file">Файл xml</param>
        public static T FromXml<T>([NotNull] this string file) where T : class, new()
        {
            return SerializerXml.Load<T>(file);
        }
    }
}
