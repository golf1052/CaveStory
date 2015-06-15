using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CaveStory
{
    public struct Tile2D
    {
        public Vector2 value;

        public Tile2D(Vector2 value)
        {
            this.value = value;
        }

        public static implicit operator Vector2(Tile2D tileUnit)
        {
            return tileUnit.value;
        }

        public static implicit operator Tile2D(Vector2 v)
        {
            return new Tile2D(v);
        }
    }
}
