using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public struct DegreesUnit
    {
        public float Value { get; set; }

        public DegreesUnit(float value)
        {
            Value = value;
        }

        public static implicit operator float (DegreesUnit degreesUnit)
        {
            return degreesUnit.Value;
        }

        public static implicit operator DegreesUnit(float f)
        {
            return new DegreesUnit(f);
        }
    }
}
