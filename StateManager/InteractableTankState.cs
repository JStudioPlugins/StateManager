﻿using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadsafeBlock;

namespace StateManager
{
    public class InteractableTankState : BarricadeState
    {

        public BarricadeDrop Drop { get; private set; }

        public ushort Capacity { get; set; }

        public InteractableTankState(BarricadeDrop drop) : base(drop.GetServersideData().barricade.state)
        {
            Drop = drop;
            Replicate(drop.GetServersideData().barricade.state);
        }

        public override void Apply()
        {
            _stateBuffer = new byte[Block.BUFFER_SIZE];
            var block = new BlockV2(_stateBuffer);

            block.writeUInt16(Capacity);

            State = block.getBytes(out int size);
            BarricadeManager.updateReplicatedState(Drop.model, State, size);
        }

        public override void Replicate(byte[] state)
        {
            BarricadeData data = Drop.GetServersideData();
            _stateBuffer = new byte[Block.BUFFER_SIZE];
            var block = new BlockV2(_stateBuffer, data.barricade.state);

            Capacity = block.readUInt16();

            State = block.getBytes(out int _);
        }
    }
}
