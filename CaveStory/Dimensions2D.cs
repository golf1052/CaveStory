using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CaveStory
{
    public struct Dimensions2D
    {
        public Vector2 value;

        public Dimensions2D(Vector2 value)
        {
            this.value = value;
        }

        public static implicit operator Vector2(Dimensions2D tileUnit)
        {
            return tileUnit.value;
        }

        public static implicit operator Dimensions2D(Vector2 v)
        {
            return new Dimensions2D(v);
        }
    }
}
