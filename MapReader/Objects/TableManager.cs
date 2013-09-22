using System;
using System.Collections.Generic;
using System.Text;

namespace MapReader.Objects
{
    public class TableManager : BaseMapPortion
    {
        private MapHeader _header;
        private HaloMap _map;
        public ObjectManager Objects { get; private set; }
        public TagManager Tags { get; private set; }
        private List<GlobalTagEntry> GlobalTags = new List<GlobalTagEntry>();

        public Reflexive TagList { get; private set; }
        public Reflexive ObjectList { get; private set; }
        public Reflexive GlobalTagList { get; private set; }
        public Reflexive Unknown { get; private set; }

        public TableManager(MapHeader header, HaloMap map)
        {
            _header = header;
            _map = map;
            _chunkSize = 32;
            _chunkOffset = _map.Magic(_header.TagTableInfo.HeaderAddress);
        }

        public override void Load(byte[] chunkData)
        {
            base.Load(chunkData);
            TagList = new Reflexive(_map.Magic(ReadUint32(4)),ReadUint32(0));
            ObjectList = new Reflexive(_map.Magic(ReadUint32(12)),ReadUint32(8));
            GlobalTagList = new Reflexive(_map.Magic(ReadUint32(20)),ReadUint32(16));
            Unknown = new Reflexive(_map.Magic(ReadUint32(28)), ReadUint32(24));

            Tags = new TagManager(_map);
            Objects = new ObjectManager(_map);

            _map.LoadListEntries(GlobalTags,GlobalTagList,LoadObjectListEntry,8);
        }

        private void LoadObjectListEntry(GlobalTagEntry obj, byte[] entryData)
        {
            String tagName = HaloMap.ReadString(entryData, 0, 4);
            obj.Tag = Tags.GetTag(tagName);
            obj.Unknown1 = HaloMap.ReadUint16(entryData, 4);
            obj.Unknown2 = HaloMap.ReadUint16(entryData, 6);
        }
    }
}
