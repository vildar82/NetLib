using System;
using System.IO;

namespace NetLib.Locks
{
	internal class FileLock : ILockItem
	{
		private FileStream stream;
		private readonly string file;

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
					Info = new LockInfo { Login = "Unknown"};
				}
				IsLockSuccess = false;
			}
		}

		public bool IsLockSuccess { get; }
		public LockInfo Info { get; private set; }
		
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
	}
}
