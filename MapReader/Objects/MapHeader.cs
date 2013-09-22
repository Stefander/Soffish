using System;

namespace MapReader.Objects
{
    public class MapHeader : BaseMapPortion
    {
        public const int PartitionAmount = 6;
        public String BlamIdentifier { get; private set; }
        public MapVersion Version { get; private set; }
        public MapContent Content { get; private set; }
        public uint FileSize { get; private set; }
        public String MapName { get; private set; }
        public String BuildVersion { get; private set; }
        public uint XDKVersion { get; private set; }
        public MapType Type { get; private set; }
        public HeaderEOF EOFInfo { get; private set; }
        public String ScenarioName { get; private set; }
        public HeaderObjectInfo TagInfo { get; private set; }
        public HeaderObjectInfo StringInfo { get; private set; }
        public XboxPartition[] Partitions { get; private set; }
        public HeaderAssetInfo AssetInfo { get; private set; }
        public HeaderLocaleInfo LocaleInfo { get; private set; }
        public HeaderVirtualInfo VirtualInfo { get; private set; }
        public HeaderTagTableInfo TagTableInfo { get; private set; }
        public uint MapMagic { get; private set; }
        public uint MetaDataOffset { get; private set; }
        public uint IndexOffset { get; private set; }
        public uint IndexOffsetMagic { get; private set; }

        public MapHeader()
        {
            _chunkSize = 1196;
            _chunkOffset = 0;
        }

        public override void Load(byte[] chunkData)
        {
            base.Load(chunkData);
            BlamIdentifier = HaloMap.ReadString(_chunkData, 0, 4);
            
            // Determine the endianness of the map file (should be big endian for Halo 4)
            if (BlamIdentifier.Equals("head"))
            {
                HaloMap.ReadType = EndianType.Big;
            }
            else if (BlamIdentifier.Equals("daeh"))
            {
                HaloMap.ReadType = EndianType.Little;
            }
            else
            {
                throw new Exception("Not a Halo map file!");
            }
             
            Version = (MapVersion)ReadUint32(0x4);

            // Only Halo 4 map files :)
            if (Version != MapVersion.Halo4)
            {
                throw new Exception("Not a Halo 4 map file!");
            }

            FileSize = ReadUint32(0x8);
            BuildVersion = HaloMap.ReadString(_chunkData, 0x11C, 0x20);
            Type = (MapType)ReadUint32(0x13C);
            Content = new MapContent(ReadUint32(0x140));

            StringInfo = new HeaderObjectInfo(ReadUint32(0x158), ReadUint32(0x15C), ReadUint32(0x160), ReadUint32(0x164));
            TagInfo = new HeaderObjectInfo(ReadUint32(0x2B4), ReadUint32(0x2BC), ReadUint32(0x2C0), ReadUint32(0x2B8));
            VirtualInfo = new HeaderVirtualInfo(ReadUint32(0x2F8), ReadUint32(0x18));
            EOFInfo = new HeaderEOF(ReadUint32(0x14), ReadUint32(0x49C));
            
            MapName = HaloMap.ReadString(_chunkData, 0x18C, 0x24);
            ScenarioName = HaloMap.ReadString(_chunkData, 0x1B0, 0x104);

            XDKVersion = ReadUint32(0x2FC);

            Partitions = new XboxPartition[PartitionAmount];
            for (int i = 0; i < PartitionAmount; i++)
            {
                int addressOffset = 0x300 + i * 0x8;
                Partitions[i].Address = ReadUint32((uint)addressOffset);
                Partitions[i].Size = ReadUint32((uint)addressOffset+4);
            }

            AssetInfo = new HeaderAssetInfo(ReadUint32(0x480), ReadUint32(0x490), ReadUint32(0x498));
            LocaleInfo = new HeaderLocaleInfo(ReadUint32(0x488), ReadUint32(0x4A4), ReadUint32(0x4A8));
            TagTableInfo = new HeaderTagTableInfo(ReadUint32(0x10), ReadUint32(0x4A0), ReadUint32(0x48C));

            // Calculate magic offsets from header data
            MetaDataOffset = AssetInfo.FileOffset+AssetInfo.Size;
            MapMagic = VirtualInfo.BaseAddress - MetaDataOffset;

            // Adjust magic and metadata offset if the partition 0 address is 0
            if (Partitions[0].Address == 0x0)
            {
                MetaDataOffset -= 0x795000;
                MapMagic -= 0x795000;
            }

            IndexOffset = TagTableInfo.HeaderAddress - MapMagic;
            IndexOffsetMagic = LocaleInfo.AddressMask;
        }
    }
}
