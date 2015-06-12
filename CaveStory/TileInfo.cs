using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public class TileInfo
    {
        public enum SideType
        {
            TopSide,
            BottomSide,
            LeftSide,
            RightSide
        }

        public enum TileFlag
        {
            Empty,
            Wall,
            Slope,
            LeftSlope,
            RightSlope,
            TopSlope,
            BottomSlope,
            TallSlope,
            ShortSlope,
            LastTileFlag
        }

        public static BitArray CreateTileType()
        {
            return new BitArray((int)TileFlag.LastTileFlag);
        }

        public static SideType OppositeSide(SideType side)
        {
            if (side == SideType.TopSide)
            {
                return SideType.BottomSide;
            }
            if (side == SideType.BottomSide)
            {
                return SideType.TopSide;
            }
            if (side == SideType.LeftSide)
            {
                return SideType.RightSide;
            }
            return SideType.LeftSide;
        }

        public static bool Vertical(SideType side)
        {
            return side == SideType.TopSide || side == SideType.BottomSide;
        }

        public static bool Horizontal(SideType side)
        {
            return !Vertical(side);
        }

        public static SideType FromFacing(SpriteState.HorizontalFacing horizontalFacing,
            SpriteState.VerticalFacing verticalFacing)
        {
            if (verticalFacing == SpriteState.VerticalFacing.Up)
            {
                return SideType.TopSide;
            }
            if (verticalFacing == SpriteState.VerticalFacing.Down)
            {
                return SideType.BottomSide;
            }
            if (horizontalFacing == SpriteState.HorizontalFacing.Left)
            {
                return SideType.LeftSide;
            }
            return SideType.RightSide;
        }

        public static TileFlag SlopeFlagFromSide(SideType side)
        {
            if (side == SideType.LeftSide)
            {
                return TileFlag.LeftSlope;
            }
            if (side == SideType.RightSide)
            {
                return TileFlag.RightSlope;
            }
            if (side == SideType.TopSide)
            {
                return TileFlag.TopSlope;
            }
            return TileFlag.BottomSlope;
        }

        public static bool IsMin(SideType side)
        {
            return !IsMax(side);
        }

        public static bool IsMax(SideType side)
        {
            return side == SideType.RightSide || side == SideType.BottomSide;
        }
    }
}
