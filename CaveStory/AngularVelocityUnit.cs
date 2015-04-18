using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    /// <summary>
    /// Degrees / MS
    /// </summary>
    public struct AngularVelocityUnit
    {
        public float Value { get; set; }

        public AngularVelocityUnit()
        {
            Value = 0.0f;
        }

        public AngularVelocityUnit(float value)
        {
            Value = value;
        }

        public static implicit operator float (AngularVelocityUnit angularVelocityUnit)
        {
            return angularVelocityUnit.Value;
        }

        public static implicit operator AngularVelocityUnit(float f)
        {
            return new AngularVelocityUnit(f);
        }
    }
}
