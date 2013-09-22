using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapReader.Objects
{
    public class Reflexive
    {
        public uint Offset;
        public uint Count;
        public uint Size;

        public Reflexive(uint offset, uint count, uint size = 0)
        {
            Offset = offset;
            Count = count;
            Size = size;
        }
    }
}
