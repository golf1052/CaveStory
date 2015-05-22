using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public struct GunExperienceUnit
    {
        public int Value { get; set; }

        public GunExperienceUnit(int value)
        {
            Value = value;
        }

        public static implicit operator int(GunExperienceUnit gunExperienceUnit)
        {
            return gunExperienceUnit.Value;
        }

        public static implicit operator GunExperienceUnit(int i)
        {
            return new GunExperienceUnit(i);
        }
    }
}
