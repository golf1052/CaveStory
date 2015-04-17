using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public struct FrameUnit
    {
        public int Value { get; set; }

        public FrameUnit()
        {
            Value = 0;
        }

        public FrameUnit(int value)
        {
            Value = value;
        }

        public static implicit operator int (FrameUnit frameUnit)
        {
            return frameUnit.Value;
        }

        public static implicit operator FrameUnit(int i)
        {
            return new FrameUnit(i);
        }
    }
}
