using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    /// <summary>
    /// Also discrete, but non-negative.
    /// </summary>
    public struct TileUnit
    {
        public uint Value { get; set; }

        public TileUnit()
        {
            Value = 0;
        }

        public TileUnit(uint value)
        {
            Value = value;
        }

        public static implicit operator uint(TileUnit tileUnit)
        {
            return tileUnit.Value;
        }

        public static implicit operator TileUnit(uint u)
        {
            return new TileUnit(u);
        }
    }
}
