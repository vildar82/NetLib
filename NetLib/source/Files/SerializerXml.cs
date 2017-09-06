using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace NetLib
{
    public class SerializerXml
    {
        private readonly string _settingsFile;

        public SerializerXml (string settingsFile)
        {
            _settingsFile = settingsFile;
        }

        public void SerializeList<T> (T settings)
        {
            using (FileStream fs = new FileStream(_settingsFile, FileMode.Create, FileAccess.Write))
            {
                XmlSerializer ser = new XmlSerializer(typeof(T));
                ser.Serialize(fs, settings);
            }
        }

        public void SerializeList<T>(T settings, params Type[] types)
        {
            using (FileStream fs = new FileStream(_settingsFile, FileMode.Create, FileAccess.Write))
            {
                XmlSerializer ser = new XmlSerializer(typeof(T), types);
                ser.Serialize(fs, settings);
            }
        }

        public T DeserializeXmlFile<T> ()
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            using (XmlReader reader = XmlReader.Create(_settingsFile))
            {
                return (T)ser.Deserialize(reader);
            }
        }

        public T DeserializeXmlFile<T>(params Type[] types)
        {
            XmlSerializer ser = new XmlSerializer(typeof(T), types);
            using (XmlReader reader = XmlReader.Create(_settingsFile))
            {
                return (T)ser.Deserialize(reader);
            }
        }

        /// <summary>
        /// Считывание объекта из файла
        /// </summary>
        /// <typeparam name="T">Тип считываемого объекта></typeparam>
        /// <param name="file">Файл xml</param>
        /// <returns>Объект T или null</returns>
        public static T Load<T>(string file) where T : class, new()
        {
            var ser = new SerializerXml(file);
            T res = null;
            res = ser.DeserializeXmlFile<T>();
            return res;
        }

        public static T Load<T>(string file, params Type[] types) where T : class, new()
        {
            var ser = new SerializerXml(file);
            T res = null;
            res = ser.DeserializeXmlFile<T>(types);
            return res;
        }

        /// <summary>
        /// Сохранение объекта в файл.
        /// При ошибке записывается лог.
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="file">Файл</param>
        /// <param name="obj">Объект</param>
        public static void Save<T> (string file, T obj)
        {
            var ser = new SerializerXml(file);
            ser.SerializeList(obj);
        }

        public static void Save<T>(string file, T obj, params Type[] types)
        {
            var ser = new SerializerXml(file);
            ser.SerializeList(obj, types);
        }
    }
}