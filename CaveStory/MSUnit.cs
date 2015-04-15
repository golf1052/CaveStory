using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    /// <summary>
    /// Discrete milliseconds.
    /// </summary>
    public struct MSUnit
    {
        public uint Value { get; set; }

        public MSUnit()
        {
            Value = 0;
        }

        public MSUnit(uint value)
        {
            Value = value;
        }

        public static implicit operator uint (MSUnit msUnit)
        {
            return msUnit.Value;
        }

        public static implicit operator MSUnit(uint u)
        {
            return new MSUnit(u);
        }
    }
}
