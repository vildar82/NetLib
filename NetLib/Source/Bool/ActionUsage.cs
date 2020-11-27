namespace NetLib
{
    using System;

    public class ActionUsage : IDisposable
    {
        private readonly Action _after;
        private readonly Action<object>? _afterObj;
        private readonly object _obj;

        public ActionUsage(Action before, Action after)
        {
            before();
            _after = after;
        }

        public ActionUsage(object obj, Action preset, Action<object> after)
        {
            _obj = obj;
            preset();
            _afterObj = after;
        }

        public void Dispose()
        {
            if (_afterObj != null)
            {
                _afterObj(_obj);
            }
            else
            {
                _after();
            }
        }
    }
}
