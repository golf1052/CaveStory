using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    /// <summary>
    /// GameUnit / MSUnit / MSUnit
    /// </summary>
    public struct AccelerationUnit
    {
        public float Value { get; set; }

        public AccelerationUnit()
        {
            Value = 0.0f;
        }

        public AccelerationUnit(float value)
        {
            Value = value;
        }

        public static implicit operator float (AccelerationUnit accelerationUnit)
        {
            return accelerationUnit.Value;
        }

        public static implicit operator AccelerationUnit(float f)
        {
            return new AccelerationUnit(f);
        }
    }
}
