using JetBrains.Annotations;
using System;

namespace NetLib
{
    public class ActionUsage : IDisposable
    {
        private readonly Action after;

        public ActionUsage([NotNull] Action before, Action after)
        {
            before();
            this.after = after;
        }

        public void Dispose()
        {
            after();
        }
    }
}
