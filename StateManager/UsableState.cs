using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateManager
{
    public abstract class UsableState(byte[] state)
    {
        protected byte[] _stateBuffer;

        public byte[] State { get; internal set; } = state;

        public abstract void Get(out byte[] state, out int size);

        public abstract void Replicate(byte[] state);
    }
}
