namespace NetLib
{
    using System;

    public static class XmlExt
    {
        /// <summary>
        /// Серилизовать объект в Xml
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="obj">Объект</param>
        /// <param name="file">Файл</param>
        public static void ToXml<T>(this T obj, string file)
        {
            SerializerXml.Save(file, obj);
        }

        public static void ToXml<T>(this T obj, string file, params Type[] types)
        {
            SerializerXml.Save(file, obj, types);
        }

        /// <summary>
        /// Десирилизовать объект из файла xml
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="file">Файл xml</param>
        public static T FromXml<T>(this string file)
            where T : class, new()
        {
            return SerializerXml.Load<T>(file);
        }

        public static T FromXml<T>(this string file, params Type[] types)
            where T : class, new()
        {
            return SerializerXml.Load<T>(file, types);
        }
    }
}
