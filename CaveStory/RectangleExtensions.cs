using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CaveStory
{
    public static class RectangleExtensions
    {
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
