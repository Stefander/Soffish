using System;

namespace MapReader.Objects
{
    public class MapContent
    {
        private uint _contentData;

        public bool HasBSP
        {
            get
            {
                return !((_contentData & 0x1000000) == 0);
            }
        }

        public bool HasAssets
        {
            get
            {
                return !((_contentData & 0x10000) == 0);
            }
        }

        public bool HasGameplay
        {
            get
            {
                return !((_contentData & 0x100) == 0);
            }
        }


        public MapContent(uint contentData)
        {
            _contentData = contentData;
        }
    }
}
