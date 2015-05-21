using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public struct GunLevelUnit
    {
        public uint Value { get; set; }

        public GunLevelUnit(uint value)
        {
            Value = value;
        }

        public static implicit operator uint (GunLevelUnit gunLevelUnit)
        {
            return gunLevelUnit.Value;
        }

        public static implicit operator GunLevelUnit(uint u)
        {
            return new GunLevelUnit(u);
        }
    }
}
