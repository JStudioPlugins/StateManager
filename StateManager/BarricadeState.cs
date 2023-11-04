using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateManager
{
    public abstract class BarricadeState(byte[] state)
    {
        public byte[] State { get; internal set; } = state;

        public abstract void Apply();

        public abstract void Replicate(byte[] state);
    }
}
