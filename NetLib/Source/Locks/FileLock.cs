﻿using JetBrains.Annotations;
using System;
using System.IO;

namespace NetLib.Locks
{
    [PublicAPI]
    public class FileLock : ILockItem
    {
        private readonly string file;
        private FileStream stream;
        /// <summary>
        /// Информация из файла блокировки
        /// </summary>
        public LockInfo Info { get; private set; }

        public bool IsLockSuccess { get; }

        public FileLock(string file)
        {
            this.file = file;
            try
            {
                File.Delete(file);
                IsLockSuccess = true;
                WriteLockInfo();
            }
            catch
            {
                // Заблокировано
                try
                {
                    Info = file.Deserialize<LockInfo>();
                }
                catch
                {
                    Info = new LockInfo { Login = "Unknown" };
                }
                IsLockSuccess = false;
            }
        }

        public void Dispose()
        {
            try
            {
                stream?.Dispose();
                File.Delete(file);
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// строка для сообщения о блокировке - Пользователь, дата
        /// </summary>
        [NotNull]
        public string GetMessage()
        {
            return $"Пользователь - {Info.Login}, дата блокировки {Info.Date}";
        }

        private void WriteLockInfo()
        {
            Info = new LockInfo { Login = Environment.UserName, Date = DateTime.Now };
            Info.Serialize(file);
            stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
        }
    }
}