using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLib
{
    public class ActionUsage : IDisposable
    {
        private readonly Action after;

        public ActionUsage(Action before,Action after)
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
