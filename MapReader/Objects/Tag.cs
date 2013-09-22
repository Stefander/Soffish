using System;

namespace MapReader.Objects
{
    public class Tag : IComparable
    {
        public String TagName { get; private set; }
        
        public Tag(String tagName)
        {
            TagName = tagName;
        }

        public int CompareTo(object obj)
        {
            Tag otherTag = (Tag)obj;
            return TagName.CompareTo(otherTag.TagName);
        }
    }
}
