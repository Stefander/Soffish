using System;
using MapReader.Objects;

namespace MapReader
{
    public enum MapVersion 
    {
        Halo2 = 0x8,
        Halo4 = 0xC
    }

    public enum MapType
    {
        Map=0xFFFF,
        Campaign=0x40002,
        MainMenu=0x20000,
        Shared=0x30001
    }

    public enum EndianType
    {
        Little = 0,
        Big
    }

    public struct XboxPartition
    {
        public uint Address;
        public uint Size;

        public XboxPartition(uint address, uint size)
        {
            Address = address;
            Size = size;
        }
    }

    public struct HeaderEOF
    {
        public uint Index;
        public uint IndexOffset;

        public HeaderEOF(uint index, uint indexoffset)
        {
            Index = index;
            IndexOffset = indexoffset;
        }
    }

    public struct HeaderVirtualInfo
    {
        public uint BaseAddress;
        public uint Size;

        public HeaderVirtualInfo(uint baseaddress, uint size)
        {
            BaseAddress = baseaddress;
            Size = size;
        }
    }

    public struct HeaderObjectInfo
    {
        public uint Count;
        public uint Size;
        public uint OffsetTablePointer;
        public uint DataPointer;

        public HeaderObjectInfo(uint count, uint size, uint offsettablepointer, uint datapointer)
        {
            Count = count;
            Size = size;
            OffsetTablePointer = offsettablepointer;
            DataPointer = datapointer;
        }
    }

    public struct HeaderAssetInfo
    {
        public uint FileOffset;
        public uint Offset;
        public uint Size;

        public HeaderAssetInfo(uint fileoffset, uint offset, uint size)
        {
            FileOffset = fileoffset;
            Offset = offset;
            Size = size;
        }
    }

    public struct HeaderTagTableInfo
    {
        public uint HeaderAddress;
        public uint MetaDataSize;
        public uint AddressMask;

        public HeaderTagTableInfo(uint headeraddress, uint metadatasize, uint addressmask)
        {
            HeaderAddress = headeraddress;
            MetaDataSize = metadatasize;
            AddressMask = addressmask;
        }
    }

    public struct HeaderLocaleInfo
    {
        public uint AddressMask;
        public uint TablePointer;
        public uint DataSize;

        public HeaderLocaleInfo(uint addressmask, uint tablepointer, uint datasize)
        {
            AddressMask = addressmask;
            TablePointer = tablepointer;
            DataSize = datasize;
        }
    }
}
