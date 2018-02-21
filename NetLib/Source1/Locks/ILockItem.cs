using System;
using JetBrains.Annotations;

namespace NetLib.Locks
{
    [PublicAPI]
    public interface ILockItem : IDisposable
    {
        LockInfo Info { get; }
        bool IsLockSuccess { get; }

        string GetMessage();
    }
}