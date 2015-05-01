using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    /// <summary>
    /// GameUnit / MSUnit
    /// </summary>
    public struct VelocityUnit
    {
        public float Value { get; set; }

        public VelocityUnit(float value)
        {
            Value = value;
        }

        public static implicit operator float (VelocityUnit velocityUnit)
        {
            return velocityUnit.Value;
        }

        public static implicit operator VelocityUnit(float f)
        {
            return new VelocityUnit(f);
        }
    }
}
