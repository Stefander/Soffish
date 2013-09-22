using System;

namespace MapReader.Objects
{
    public class ObjectListEntry
    {
        public TagListEntry ObjectTag;
        public uint TagId;
        public uint TagIndex;
        public bool IsEmpty;
        public uint Offset;
        public uint RawOffset;

        public ObjectListEntry()
        {
        }
    }
}
