using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    /// <summary>
    /// GameUnit. Basically positional stuff.
    /// Float for extra precision.
    /// </summary>
    public struct GameUnit
    {
        public float Value { get; set; }

        public GameUnit()
        {
            Value = 0.0f;
        }

        public GameUnit(float value)
        {
            Value = value;
        }

        public static implicit operator float(GameUnit gameUnit)
        {
            return gameUnit.Value;
        }

        public static implicit operator GameUnit(float f)
        {
            return new GameUnit(f);
        }
    }
}
