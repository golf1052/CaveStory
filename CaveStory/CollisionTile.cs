using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public class CollisionTile
    {
        public TileUnit row;
        public TileUnit col;
        public Tile.TileType tileType;

        public CollisionTile(TileUnit row, TileUnit col, Tile.TileType tileType)
        {
            this.row = row;
            this.col = col;
            this.tileType = tileType;
        }
    }
}
