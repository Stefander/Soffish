using System;
using System.Collections.Generic;

namespace MapReader.Objects
{
    public class ObjectManager
    {
        private HaloMap _map;
        public List<ObjectListEntry> ObjectList = new List<ObjectListEntry>();

        public ObjectManager(HaloMap map)
        {
            _map = map;
            _map.LoadListEntries(ObjectList, _map.Tables.ObjectList, LoadEntry, 8);
        }

        private void LoadEntry(ObjectListEntry obj, byte[] entryData)
        {
            obj.TagId = HaloMap.ReadUint16(entryData, 0);
            obj.TagIndex = HaloMap.ReadUint16(entryData, 2);
            obj.RawOffset = HaloMap.ReadUint32(entryData, 4);
            obj.Offset = obj.RawOffset - _map.Header.MapMagic;
            obj.IsEmpty = (obj.RawOffset == 0x0);

            if (!obj.IsEmpty)
            {
                obj.ObjectTag = _map.Tables.Tags.TagList[(int)obj.TagId];
            }
        }
    }
}
