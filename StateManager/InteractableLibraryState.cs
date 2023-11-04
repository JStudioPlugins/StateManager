using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadsafeBlock;

namespace StateManager
{
    public class InteractableLibraryState : BarricadeState
    {
        private byte[] _stateBuffer;

        public BarricadeDrop Drop { get; private set; }

        public CSteamID Owner { get; set; }
        public CSteamID Group { get; set; }

        public uint Amount { get; set; }

        public InteractableLibraryState(BarricadeDrop drop) : base(drop.GetServersideData().barricade.state)
        {
            Drop = drop;
            Replicate(drop.GetServersideData().barricade.state);
        }

        public override void Apply()
        {
            _stateBuffer = new byte[Block.BUFFER_SIZE];
            var block = new BlockV2(_stateBuffer);

            block.write(Owner, Group);
            block.writeUInt32(Amount);


            State = block.getBytes(out int size);
            BarricadeManager.updateReplicatedState(Drop.model, State, size);
        }

        public override void Replicate(byte[] state)
        {
            BarricadeData data = Drop.GetServersideData();
            _stateBuffer = new byte[Block.BUFFER_SIZE];
            var block = new BlockV2(_stateBuffer, data.barricade.state);

            Owner = new CSteamID((ulong)block.read(Types.UINT64_TYPE));
            Group = new CSteamID((ulong)block.read(Types.UINT64_TYPE));
            Amount = block.readUInt32();

            State = block.getBytes(out int _);
        }
    }
}
