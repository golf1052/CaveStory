using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public struct HPUnit
    {
        public int Value { get; set; }

        public HPUnit(int value)
        {
            Value = value;
        }

        public static implicit operator int (HPUnit tileUnit)
        {
            return tileUnit.Value;
        }

        public static implicit operator HPUnit(int i)
        {
            return new HPUnit(i);
        }
    }
}
