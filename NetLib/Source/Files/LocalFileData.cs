using JetBrains.Annotations;
using NLog;
using System;
using System.IO;

namespace NetLib
{
    /// <summary>
    /// Данные хранимые в файле json на сервере, с локальным кэшем
    /// </summary>
    [PublicAPI]
    public class LocalFileData<T> where T : class, new()
    {
        public readonly string LocalFile;
        private readonly bool isXmlOrJson;
        public T Data { get; set; }

        // ReSharper disable once StaticMemberInGenericType
        private static ILogger Logger { get; } = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Данные хранимые в файле json на сервере, с локальным кэшем
        /// </summary>
        /// /// <param name="localFile"></param>
        /// <param name="isXmlOrJson">true - xml, false - json</param>
        public LocalFileData([NotNull] string localFile, bool isXmlOrJson)
        {
            LocalFile = localFile;
            this.isXmlOrJson = isXmlOrJson;
        }

        /// <summary>
        ///
        /// </summary>
        public void Load()
        {
            Data = !File.Exists(LocalFile) ? default : Deserialize();
        }

        public void Save()
        {
            Serialize();
        }

        public void TryLoad()
        {
            try
            {
                Load();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public void TrySave()
        {
            try
            {
                Save();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private T Deserialize()
        {
            return isXmlOrJson ? SerializerXml.Load<T>(LocalFile) : LocalFile.Deserialize<T>();
        }

        private void Serialize()
        {
            if (isXmlOrJson) SerializerXml.Save(LocalFile, Data);
            else Data.Serialize(LocalFile);
        }
    }
}