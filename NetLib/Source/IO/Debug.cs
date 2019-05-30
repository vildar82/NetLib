namespace NetLib.IO
{
    using System;

    public static class Debug
    {
        public static void WriteLine(this string msg)
        {
            System.Diagnostics.Debug.WriteLine($"{DateTime.Now}: {msg}");
        }
    }
}
