using System;
using System.Collections.Generic;

namespace MapReader.Objects
{
    public class TagManager
    {
        private HaloMap _map;
        public List<TagListEntry> TagList = new List<TagListEntry>();

        public TagManager(HaloMap map)
        {
            _map = map;
            _map.LoadListEntries(TagList, _map.Tables.TagList, LoadTagListEntry, 16);
        }

        private void LoadTagListEntry(TagListEntry obj, byte[] entryData)
        {
            obj.Tags = new Tag[3];
            for (uint j = 0; j < 3; j++)
            {
                obj.Tags[j] = new Tag(HaloMap.ReadString(entryData, j * 4, 4));
            }

            obj.Unknown = HaloMap.ReadUint32(entryData, 12);
        }

        public TagListEntry GetTag(String tagName)
        {
            foreach (TagListEntry entry in TagList)
            {
                if (tagName.Equals(entry.Tags[0].TagName))
                {
                    return entry;
                }
            }

            Console.WriteLine("Couldn't find tag '"+tagName+"'!");
            return null;
        }
    }
}
