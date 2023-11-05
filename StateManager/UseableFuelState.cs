using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadsafeBlock;

namespace StateManager
{
    public class UseableFuelState : UsableState
    {
        public Item Item { get; private set; }

        public ushort Fuel { get; set; }

        public UseableFuelState(Item item) : base(item.state)
        {
            Item = item;
            Replicate(item.state);
        }

        public override void Get(out byte[] state, out int size)
        {
            _stateBuffer = new byte[Block.BUFFER_SIZE];
            var block = new BlockV2(_stateBuffer);

            block.writeUInt16(Fuel);

            State = block.getBytes(out size);
            state = State;
        }

        public override void Replicate(byte[] state)
        {
            _stateBuffer = new byte[Block.BUFFER_SIZE];
            var block = new BlockV2(_stateBuffer, Item.state);

            Fuel = block.readUInt16();

            State = block.getBytes(out int _);
        }
    }
}
