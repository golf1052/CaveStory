using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CaveStory
{
    public struct Velocity2D
    {
        public Vector2 value;

        public Velocity2D(Vector2 value)
        {
            this.value = value;
        }

        public static implicit operator Vector2(Velocity2D tileUnit)
        {
            return tileUnit.value;
        }

        public static implicit operator Velocity2D(Vector2 v)
        {
            return new Velocity2D(v);
        }
    }
}
