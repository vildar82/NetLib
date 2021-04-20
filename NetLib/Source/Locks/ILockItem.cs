namespace NetLib.Locks
{
    using System;

    public interface ILockItem : IDisposable
    {
        LockInfo Info { get; }
        bool IsLockSuccess { get; }
        string GetMessage();
    }
}
