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
        /// <param name="side"></param>
        /// <param name="position"></param>
        /// <returns>
        /// Returns null if there was no collision
        /// Returns the position of the collision on the same axis of side
        /// </returns>
        public GameUnit? TestCollision(TileInfo.SideType side, GameUnit position)
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
            return null;
        }
    }
}
