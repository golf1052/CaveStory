using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public class CollisionTile
    {
        private TileUnit row;
        private TileUnit col;
        private TileInfo.TileType tileType;

        public CollisionTile(TileUnit row, TileUnit col, TileInfo.TileType tileType)
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
            if (tileType == TileInfo.TileType.WallTile)
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
