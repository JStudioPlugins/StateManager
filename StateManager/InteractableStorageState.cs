using Rocket.Core.Assets;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadsafeBlock;
using UnityEngine;
using Types = SDG.Unturned.Types;
using Item = SDG.Unturned.Item;

namespace StateManager
{
    public class InteractableStorageState : BarricadeState
    {
        private byte[] _stateBuffer;

        public BarricadeDrop Drop { get; private set; }

        public CSteamID Owner { get; set; }
        public CSteamID Group { get; set; }

        public Items Items { get; set; }

        public ushort DisplaySkin { get; set; }
        public ushort DisplayMythic { get; set; }
        public string DisplayTags { get; set; }
        public string DisplayDynamicProps { get; set; }
        public byte Rotation { get; set; }

        public InteractableStorageState(BarricadeDrop drop) : base(drop.GetServersideData().barricade.state)
        {
            Drop = drop;
            Replicate(drop.GetServersideData().barricade.state);
        }

        public override void Apply()
        {
            _stateBuffer = new byte[Block.BUFFER_SIZE];
            var block = new BlockV2(_stateBuffer);
            if (Items != null)
            {
                block.write(Owner, Group, Items.getItemCount());
                for (byte b = 0; b < Items.getItemCount(); b++)
                {
                    ItemJar item = Items.getItem(b);
                    block.write(item.x, item.y, item.rot, item.item.id, item.item.amount, item.item.quality, item.item.state);
                }
                if (((ItemStorageAsset)Drop.asset).isDisplay)
                {
                    block.write(DisplaySkin);
                    block.write(DisplayMythic);
                    block.write(string.IsNullOrEmpty(DisplayTags) ? string.Empty : DisplayTags);
                    block.write(string.IsNullOrEmpty(DisplayDynamicProps) ? string.Empty : DisplayDynamicProps);
                    block.write(Rotation);
                }
            }
            else
            {
                block.write(Owner, Group, ((byte)0));
                if (((ItemStorageAsset)Drop.asset).isDisplay)
                {
                    block.write(DisplaySkin);
                    block.write(DisplayMythic);
                    block.write(string.IsNullOrEmpty(DisplayTags) ? string.Empty : DisplayTags);
                    block.write(string.IsNullOrEmpty(DisplayDynamicProps) ? string.Empty : DisplayDynamicProps);
                    block.write(Rotation);
                }
            }
            State = block.getBytes(out int size);
            BarricadeManager.updateReplicatedState(Drop.model, State, size);
        }

        public override void Replicate(byte[] state)
        {
            BarricadeData data = Drop.GetServersideData();
            _stateBuffer = new byte[Block.BUFFER_SIZE];
            var block = new BlockV2(_stateBuffer, data.barricade.state);

            Owner = (CSteamID)block.read(Types.STEAM_ID_TYPE);
            Group = (CSteamID)block.read(Types.STEAM_ID_TYPE);
            Items = new(PlayerInventory.STORAGE);
            Items.resize(((ItemStorageAsset)data.barricade.asset).storage_x, ((ItemStorageAsset)data.barricade.asset).storage_y);
            byte b = block.readByte();
            for (byte b2 = 0; b2 < b; b2++)
            {
                if (BarricadeManager.version > 7)
                {
                    object[] array = block.read(Types.BYTE_TYPE, Types.BYTE_TYPE, Types.BYTE_TYPE, Types.UINT16_TYPE, Types.BYTE_TYPE, Types.BYTE_TYPE, Types.BYTE_ARRAY_TYPE);
                    if (Assets.find(EAssetType.ITEM, (ushort)array[3]) is ItemAsset)
                    {
                        Items.loadItem((byte)array[0], (byte)array[1], (byte)array[2], new Item((ushort)array[3], (byte)array[4], (byte)array[5], (byte[])array[6]));
                    }
                }
                else
                {
                    object[] array2 = block.read(Types.BYTE_TYPE, Types.BYTE_TYPE, Types.UINT16_TYPE, Types.BYTE_TYPE, Types.BYTE_TYPE, Types.BYTE_ARRAY_TYPE);
                    if (Assets.find(EAssetType.ITEM, (ushort)array2[2]) is ItemAsset)
                    {
                        Items.loadItem((byte)array2[0], (byte)array2[1], 0, new Item((ushort)array2[2], (byte)array2[3], (byte)array2[4], (byte[])array2[5]));
                    }
                }
            }
            if (((ItemStorageAsset)Drop.asset).isDisplay)
            {
                DisplaySkin = block.readUInt16();
                DisplayMythic = block.readUInt16();
                if (BarricadeManager.version > 12)
                {
                    DisplayTags = block.readString();
                    DisplayDynamicProps = block.readString();
                }
                else
                {
                    DisplayTags = string.Empty;
                    DisplayDynamicProps = string.Empty;
                }
                if (BarricadeManager.version > 8)
                {
                    Rotation = block.readByte();
                }
                else
                {
                    Rotation = 0;
                }
            }
            State = block.getBytes(out int _);
        }
    }
}
