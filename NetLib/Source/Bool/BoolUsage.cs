using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLib
{
    public class BoolUsage : IDisposable
    {
        private readonly bool oldValue;
        private readonly Action<bool> setValue;

        public BoolUsage(bool curValue, bool newValue, Action<bool> setValue)
        {
            oldValue = curValue;
            this.setValue = setValue;
            setValue(newValue);
        }

        public void Dispose()
        {
            setValue(oldValue);
        }
    }
}
