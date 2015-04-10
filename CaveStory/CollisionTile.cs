using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public class CollisionTile
    {
        int row;
        int col;
        Tile.TileType tileType;

        public CollisionTile(int row, int col, Tile.TileType tileType)
        {
            this.row = row;
            this.col = col;
            this.tileType = tileType;
        }
    }
}
