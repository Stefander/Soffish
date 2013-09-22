using System;
using MapReader.Objects;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace MapReader
{
    public class HaloMap
    {
        FileStream _stream;
        public MapHeader Header { get; private set; }
        public TableManager Tables { get; private set; }
        public bool IsLoaded { get; private set; }

        public static EndianType ReadType = EndianType.Big;

        public HaloMap()
        {
            Header = null;
            IsLoaded = false;
        }

        public bool LoadMap(String fileName)
        {
            // Open the file as a new stream
            _stream = new FileStream(fileName, FileMode.Open);

            // Make sure the filesize is larger than the header chunk
            FileInfo f = new FileInfo(fileName);
            Header = new MapHeader();
            if (f.Length < Header.ChunkSize())
            {
                throw new Exception("Not a valid map file!");
            }

            // Load header and tag table header
            LoadMapPortion(Header);
            LoadMapPortion(Tables = new TableManager(Header,this));

            // Close the stream
            _stream.Close();

            IsLoaded = true;

            return true;
        }

        #region Read helpers

        public void LoadListEntries<T>(List<T> list, Reflexive target, Action<T, byte[]> method, uint entrySize) where T : new()
        {
            byte[] tagChunk = GetChunk(target.Offset, target.Count * entrySize);
            for (int i = 0; i < target.Count; i++)
            {
                byte[] entryData = new byte[entrySize];
                Buffer.BlockCopy(tagChunk, (int)(i * entrySize), entryData, 0, (int)entrySize);
                T obj = new T();
                method(obj, entryData);
                list.Add(obj);
            }
        }

        public uint Magic(uint offset)
        {
            return offset - Header.MapMagic;
        }

        public byte[] GetChunk(uint offset, uint size)
        {
            byte[] dataChunk = new byte[size];
            _stream.Seek(offset, SeekOrigin.Begin);
            _stream.Read(dataChunk, 0, (int)size);
            return dataChunk;
        }

        public void LoadMapPortion(BaseMapPortion section)
        {
            section.Load(GetChunk(section.ChunkOffset(),section.ChunkSize()));
        }

        public static uint ReadUint32(byte[] data, uint offset)
        {
            uint value = BitConverter.ToUInt32(data, (int)offset);
            if (ReadType == EndianType.Little)
            {
                return value;
            }

            var b1 = (value >> 0) & 0xff;
            var b2 = (value >> 8) & 0xff;
            var b3 = (value >> 16) & 0xff;
            var b4 = (value >> 24) & 0xff;

            return b1 << 24 | b2 << 16 | b3 << 8 | b4 << 0;
        }

        public static uint ReadUint16(byte[] data, uint offset)
        {
            uint value = BitConverter.ToUInt16(data, (int)offset);
            if (ReadType == EndianType.Little)
            {
                return value;
            }

            var b1 = (value >> 0) & 0xff;
            var b2 = (value >> 8) & 0xff;

            return b1 << 8 | b2 << 0;
        }

        public static String ReadString(byte[] data, uint offset, uint length)
        {
            char[] removeChars = { (char)0, '\n' };
            return Encoding.UTF8.GetString(data, (int)offset, (int)length).Trim(removeChars);
        }

        public static String ToHex(uint data)
        {
            return data.ToString("X");
        }

        #endregion
    }
}