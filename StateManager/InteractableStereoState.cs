using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadsafeBlock;

namespace StateManager
{
    public class InteractableStereoState : BarricadeState
    {

        public BarricadeDrop Drop { get; private set; }

        public Guid Track { get; set; }

        public byte CompressedVolume { get; set; }

        public InteractableStereoState(BarricadeDrop drop) : base(drop.GetServersideData().barricade.state)
        {
            Drop = drop;
            Replicate(drop.GetServersideData().barricade.state);
        }

        public override void Apply()
        {
            _stateBuffer = new byte[Block.BUFFER_SIZE];
            var block = new BlockV2(_stateBuffer);

            block.writeGUID(Track);
            block.writeByte(CompressedVolume);

            State = block.getBytes(out int size);
            BarricadeManager.updateReplicatedState(Drop.model, State, size);
        }

        public override void Replicate(byte[] state)
        {
            BarricadeData data = Drop.GetServersideData();
            _stateBuffer = new byte[Block.BUFFER_SIZE];
            var block = new BlockV2(_stateBuffer, data.barricade.state);

            Track = block.readGUID();
            CompressedVolume = block.readByte();

            State = block.getBytes(out int _);
        }
    }
}
