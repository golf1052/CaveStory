using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CaveStory
{
    public struct Position2D
    {
        public Vector2 value;

        public Position2D(Vector2 value)
        {
            this.value = value;
        }

        public static implicit operator Vector2(Position2D tileUnit)
        {
            return tileUnit.value;
        }

        public static implicit operator Position2D(Vector2 v)
        {
            return new Position2D(v);
        }
    }
}
