using System;

namespace NetLib.Locks
{
	public interface ILockItem : IDisposable
	{
		bool IsLockSuccess { get; }
		LockInfo Info { get; }
		string GetMessage();
	}
}
