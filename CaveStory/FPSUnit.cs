using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    /// <summary>
    /// Frame per second (Hz or 1 / Second)
    /// </summary>
    public struct FPSUnit
    {
        public uint Value { get; set; }

        public FPSUnit(uint value)
        {
            Value = value;
        }

        public static implicit operator uint (FPSUnit fpsUnit)
        {
            return fpsUnit.Value;
        }

        public static implicit operator FPSUnit(uint u)
        {
            return new FPSUnit(u);
        }
    }
}
