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
        private static ILogger Logger { get; } = LogManager.GetCurrentClassLogger();
        private DateTime fileLastWrite;
            
        public readonly string LocalFile;
        private readonly bool isXmlOrJson;
        [NotNull]
        public T Data { get; set; }

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

        public bool HasChanges()
        {
            return File.GetLastWriteTime(LocalFile) > fileLastWrite;
        }

        /// <summary>
        ///
        /// </summary>
        public void Load()
        {
            Data = Deserialize();
            fileLastWrite = File.GetLastWriteTime(LocalFile);
        }

        public void Save()
        {
            Serialize();
        }

        public void TryLoad(Func<T> onError)
        {
            try
            {
                Load();
            }
            catch (Exception ex)
            {
                Data = onError();
                Logger.Error(ex);
            }
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