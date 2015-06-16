using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CaveStory
{
    public static class RectangleExtensions
    {
        public static Rectangle NewRectangle(Position2D position, Dimensions2D dimensions)
        {
            return new Rectangle((int)Math.Round(position.value.X), (int)Math.Round(position.value.Y),
                (int)Math.Round(dimensions.value.X), (int)Math.Round(dimensions.value.Y));
        }

        public static int Side(this Rectangle rectangle, TileInfo.SideType side)
        {
            if (side == TileInfo.SideType.LeftSide)
            {
                return rectangle.Left;
            }
            if (side == TileInfo.SideType.RightSide)
            {
                return rectangle.Right;
            }
            if (side == TileInfo.SideType.TopSide)
            {
                return rectangle.Top;
            }
            return rectangle.Bottom;
        }
    }
}
