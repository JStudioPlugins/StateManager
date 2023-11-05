using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadsafeBlock;

namespace StateManager
{
    public class UseableGunState : UsableState
    {
        public Item Item { get; private set; }

        public ushort Sight { get; set; }
        public ushort Tactical { get; set; }
        public ushort Grip { get; set; }
        public ushort Barrel { get; set; }
        public ushort Magazine { get; set; }
        public byte AmmoAmount { get; set; }
        
        public EFiremode Firemode { get; set; }
        public byte TacticalSetting { get; set; }
        public byte SightQuality { get; set; }
        public byte TacticalQuality { get; set; }
        public byte GripQuality { get; set; }
        public byte BarrelQuality { get; set; }
        public byte MagazineQuality { get; set; }

        public UseableGunState(Item item) : base(item.state)
        {
            Item = item;
            Replicate(item.state);
        }

        public override void Get(out byte[] state, out int size)
        {
            _stateBuffer = new byte[Block.BUFFER_SIZE];
            var block = new BlockV2(_stateBuffer);

            block.writeUInt16(Sight);
            block.writeUInt16(Tactical);
            block.writeUInt16(Grip);
            block.writeUInt16(Barrel);
            block.writeUInt16(Magazine);
            block.writeByte(AmmoAmount);
            block.writeByte((byte)Firemode);
            block.writeByte(TacticalSetting);
            block.writeByte(SightQuality);
            block.writeByte(TacticalQuality);
            block.writeByte(GripQuality);
            block.writeByte(BarrelQuality);
            block.writeByte(MagazineQuality);

            State = block.getBytes(out size);
            state = State;
        }

        public override void Replicate(byte[] state)
        {
            _stateBuffer = new byte[Block.BUFFER_SIZE];
            var block = new BlockV2(_stateBuffer, Item.state);

            Sight = block.readUInt16();
            Tactical = block.readUInt16();
            Grip = block.readUInt16();
            Barrel = block.readUInt16();
            Magazine = block.readUInt16();
            AmmoAmount = block.readByte();
            Firemode = (EFiremode)block.readByte();
            TacticalSetting = block.readByte();
            SightQuality = block.readByte();
            TacticalQuality = block.readByte();
            GripQuality = block.readByte();
            BarrelQuality = block.readByte();
            MagazineQuality = block.readByte();

            State = block.getBytes(out int _);
        }
    }
}
