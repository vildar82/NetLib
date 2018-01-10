using JetBrains.Annotations;
using System;

namespace NetLib
{
    public class BoolUsage : IDisposable
    {
        private readonly bool oldValue;
        private readonly Action<bool> setValue;

        public BoolUsage(bool curValue, bool newValue, [NotNull] Action<bool> setValue)
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
