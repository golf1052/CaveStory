using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public class CollisionTile
    {
        private TileUnit row;
        private TileUnit col;
        private BitArray tileType;

        public CollisionTile(TileUnit row, TileUnit col, BitArray tileType)
        {
            this.row = row;
            this.col = col;
            this.tileType = tileType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="side">The side of the tile that is being collided with</param>
        /// <param name="perpendicularPosition">The position on the tile on the opposite axis of side</param>
        /// <param name="leadingPosition">Position of the leading edge of the colliding entity</param>
        /// <param name="shouldTestSlopes">whether slopes should be considered for collision</param>
        /// <returns>
        /// Returns null if there was no collision
        /// Returns the position of the collision on the same axis of side
        /// </returns>
        public GameUnit? TestCollision(TileInfo.SideType side, GameUnit perpendicularPosition,
            GameUnit leadingPosition, bool shouldTestSlopes)
        {
            if (tileType[(int)TileInfo.TileFlag.Wall])
            {
                if (side == TileInfo.SideType.TopSide)
                {
                    return Units.TileToGame(row);
                }
                if (side == TileInfo.SideType.BottomSide)
                {
                    return Units.TileToGame(row + 1);
                }
                if (side == TileInfo.SideType.LeftSide)
                {
                    return Units.TileToGame(col);
                }
                return Units.TileToGame(col + 1);
            }
            else if (shouldTestSlopes && tileType[(int)TileInfo.TileFlag.Slope] &&
                !tileType[(int)TileInfo.SlopeFlagFromSide(side)])
            {
                GameUnit row = Units.TileToGame(this.row);
                GameUnit col = Units.TileToGame(this.col);
                float slope = GetSlope(tileType);
                GameUnit offset = GetOffset(tileType);
                GameUnit calculatedPosition = TileInfo.Vertical(side) ?
                    slope * (perpendicularPosition - col) + offset + row :
                    (perpendicularPosition - row - offset) / slope + col;

                bool isColliding = TileInfo.IsMax(side) ?
                    leadingPosition <= calculatedPosition :
                    leadingPosition >= calculatedPosition;

                if (isColliding)
                {
                    return calculatedPosition;
                }
            }
            return null;
        }

        public float GetSlope(BitArray tileType)
        {
            BitArray rightTop = TileInfo.CreateTileType();
            rightTop.Set((int)TileInfo.TileFlag.RightSlope, true);
            rightTop.Set((int)TileInfo.TileFlag.TopSlope, true);
            BitArray leftBottom = TileInfo.CreateTileType();
            leftBottom.Set((int)TileInfo.TileFlag.LeftSlope, true);
            leftBottom.Set((int)TileInfo.TileFlag.BottomSlope, true);
            bool isPositive = ((rightTop.And(tileType).IsEqual(rightTop)) ||
                (leftBottom.And(tileType).IsEqual(leftBottom)));
            return isPositive ? 0.5f : -0.5f;
        }

        public GameUnit GetOffset(BitArray tileType)
        {
            BitArray leftTopTall = TileInfo.CreateTileType();
            leftTopTall.Set((int)TileInfo.TileFlag.LeftSlope, true);
            leftTopTall.Set((int)TileInfo.TileFlag.TopSlope, true);
            leftTopTall.Set((int)TileInfo.TileFlag.TallSlope, true);
            BitArray rightBottomShort = TileInfo.CreateTileType();
            rightBottomShort.Set((int)TileInfo.TileFlag.RightSlope, true);
            rightBottomShort.Set((int)TileInfo.TileFlag.BottomSlope, true);
            rightBottomShort.Set((int)TileInfo.TileFlag.ShortSlope, true);
            if ((leftTopTall.And(tileType).IsEqual(leftTopTall)) ||
                (rightBottomShort.And(tileType).IsEqual(rightBottomShort)))
            {
                return Units.TileToGame(1);
            }

            BitArray leftBottomTall = TileInfo.CreateTileType();
            leftBottomTall.Set((int)TileInfo.TileFlag.LeftSlope, true);
            leftBottomTall.Set((int)TileInfo.TileFlag.BottomSlope, true);
            leftBottomTall.Set((int)TileInfo.TileFlag.TallSlope, true);
            BitArray rightTopShort = TileInfo.CreateTileType();
            rightTopShort.Set((int)TileInfo.TileFlag.RightSlope, true);
            rightTopShort.Set((int)TileInfo.TileFlag.TopSlope, true);
            rightTopShort.Set((int)TileInfo.TileFlag.ShortSlope, true);
            if (leftBottomTall.And(tileType).IsEqual(leftBottomTall) ||
                rightTopShort.And(tileType).IsEqual(rightTopShort))
            {
                return 0.0f;
            }

            return Units.HalfTile;
        }
    }
}
