namespace NetLib.Locks
{
    using System;
    using JetBrains.Annotations;

    [PublicAPI]
    public interface ILockItem : IDisposable
    {
        LockInfo Info { get; }
        bool IsLockSuccess { get; }
        string GetMessage();
    }
}
