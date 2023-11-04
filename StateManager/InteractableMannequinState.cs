using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.UI.GridLayoutGroup;
using ThreadsafeBlock;
using Steamworks;
using UnityEngine;
using Types = SDG.Unturned.Types;

namespace StateManager
{
    public class InteractableMannequinState : BarricadeState
    {
        private byte[] _stateBuffer;

        public BarricadeDrop Drop { get; private set; }

        public CSteamID Owner { get; set; }
        public CSteamID Group { get; set; }

        public int VisualShirt { get; set; }
        public int VisualPants { get; set; }
        public int VisualHat { get; set; }
        public int VisualBackpack { get; set; }
        public int VisualVest { get; set; }
        public int VisualMask { get; set; }
        public int VisualGlasses { get; set; }

        public ushort Shirt { get; set; }
        public byte ShirtQuality { get; set; }
        public ushort Pants { get; set; }
        public byte PantsQuality { get; set; }
        public ushort Hat { get; set; }
        public byte HatQuality { get; set; }
        public ushort Backpack { get; set; }
        public byte BackpackQuality { get; set; }
        public ushort Vest { get; set; }
        public byte VestQuality { get; set; }
        public ushort Mask { get; set; }
        public byte MaskQuality { get; set; }
        public ushort Glasses { get; set; }
        public byte GlassesQuality { get; set; }
        public byte[] ShirtState { get; set; }
        public byte[] PantsState { get; set; }
        public byte[] HatState { get; set; }
        public byte[] BackpackState { get; set; }
        public byte[] VestState { get; set; }
        public byte[] MaskState { get; set; }
        public byte[] GlassesState { get; set; }
        public byte PoseComp { get; set; }

        public InteractableMannequinState(BarricadeDrop drop) : base(drop.GetServersideData().barricade.state)
        {
            Drop = drop;
            Replicate(drop.GetServersideData().barricade.state);
        }

        public override void Apply()
        {
            _stateBuffer = new byte[Block.BUFFER_SIZE];
            var block = new BlockV2(_stateBuffer);

            block.write(Owner, Group);
            block.writeInt32(VisualShirt);
            block.writeInt32(VisualPants);
            block.writeInt32(VisualHat);
            block.writeInt32(VisualBackpack);
            block.writeInt32(VisualVest);
            block.writeInt32(VisualMask);
            block.writeInt32(VisualGlasses);
            block.writeUInt16(Shirt);
            block.writeByte(ShirtQuality);
            block.writeUInt16(Pants);
            block.writeByte(PantsQuality);
            block.writeUInt16(Hat);
            block.writeByte(HatQuality);
            block.writeUInt16(Backpack);
            block.writeByte(BackpackQuality);
            block.writeUInt16(Vest);
            block.writeByte(VestQuality);
            block.writeUInt16(Mask);
            block.writeByte(MaskQuality);
            block.writeUInt16(Glasses);
            block.writeByte(GlassesQuality);
            block.writeByteArray(ShirtState);
            block.writeByteArray(PantsState);
            block.writeByteArray(HatState);
            block.writeByteArray(BackpackState);
            block.writeByteArray(VestState);
            block.writeByteArray(MaskState);
            block.writeByteArray(GlassesState);
            block.writeByte(PoseComp);


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
            VisualShirt = block.readInt32();
            VisualPants = block.readInt32();
            VisualHat = block.readInt32();
            VisualBackpack = block.readInt32();
            VisualVest = block.readInt32();
            VisualMask = block.readInt32();
            VisualGlasses = block.readInt32();
            Shirt = block.readUInt16();
            ShirtQuality = block.readByte();
            Pants = block.readUInt16();
            PantsQuality = block.readByte();
            Hat = block.readUInt16();
            HatQuality = block.readByte();
            Backpack = block.readUInt16();
            BackpackQuality = block.readByte();
            Vest = block.readUInt16();
            VestQuality = block.readByte();
            Mask = block.readUInt16();
            MaskQuality = block.readByte();
            Glasses = block.readUInt16();
            GlassesQuality = block.readByte();
            ShirtState = block.readByteArray();
            PantsState = block.readByteArray();
            HatState = block.readByteArray();
            BackpackState = block.readByteArray();
            VestState = block.readByteArray();
            MaskState = block.readByteArray();
            GlassesState = block.readByteArray();

            State = block.getBytes(out int _);
        }
    }
}
