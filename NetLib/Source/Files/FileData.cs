namespace NetLib
{
    using System;
    using System.IO;
    using JetBrains.Annotations;
    using NLog;

    /// <summary>
    /// Данные хранимые в файле xml/json на сервере, с локальным кэшем
    /// </summary>
    [PublicAPI]
    public class FileData<T> where T : class, new()
    {
        public readonly string LocalFile;
        public readonly string ServerFile;
        private readonly bool isXmlOrJson;

        public T Data { get; set; }

        private static ILogger Logger { get; } = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileData{T}"/> class.
        /// Данные хранимые в файле на сервере, с локальным кэшем
        /// </summary>
        /// <param name="localFile"></param>
        /// <param name="isXmlOrJson">true - xml, false - json</param>
        /// <param name="serverFile"></param>
        public FileData([NotNull] string serverFile, [NotNull] string localFile, bool isXmlOrJson)
        {
            ServerFile = serverFile;
            LocalFile = localFile;
            this.isXmlOrJson = isXmlOrJson;
        }

        /// <summary>
        ///
        /// </summary>
        public T Load()
        {
            Copy();
            Data = Deserialize();
            return Data;
        }

        public void Save()
        {
            var serverDir = Path.GetDirectoryName(ServerFile);
            if (!Directory.Exists(serverDir))
            {
                Directory.CreateDirectory(serverDir ?? throw new InvalidOperationException());
            }
            Serialize();
            Copy();
        }

        public T TryLoad()
        {
            try
            {
                return Load();
            }
            catch
            {
                //
                return default;
            }
        }

        public T TryLoad(Func<T> onError)
        {
            try
            {
                return Load();
            }
            catch
            {
                Data = onError();
                return Data;
            }
        }

        public void TrySave()
        {
            try
            {
                Save();
            }
            catch
            {
                //
            }
        }

        public void Update()
        {
            try
            {
                if (!IO.Path.IsEqualsDateFile(ServerFile, LocalFile))
                {
                    Load();
                }
            }
            catch
            {
                // Нет доступа к файлу
            }
        }

        private void Copy()
        {
            if (!File.Exists(ServerFile)) return;
            try
            {
                File.Copy(ServerFile, LocalFile, true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Копирование файла с сервера локально - {ServerFile}.");
            }
        }

        private T Deserialize()
        {
            return isXmlOrJson ? SerializerXml.Load<T>(LocalFile) : LocalFile.Deserialize<T>();
        }

        private void Serialize()
        {
            if (isXmlOrJson) SerializerXml.Save(ServerFile, Data);
            else Data.Serialize(ServerFile);
        }
    }
}