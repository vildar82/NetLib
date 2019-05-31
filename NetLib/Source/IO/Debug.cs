namespace NetLib.IO
{
    using System;

    public static class DebugEx
    {
        public static void WriteLine(this string msg)
        {
            System.Diagnostics.Debug.WriteLine($"{DateTime.Now:HH:mm:ss.fff}: {msg}");
        }
    }
}
