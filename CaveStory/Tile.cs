using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public class Tile
    {
        public TileInfo.TileType tileType;
        public Sprite sprite;

        public Tile(TileInfo.TileType tileType = TileInfo.TileType.AirTile,
            Sprite sprite = null)
        {
            this.tileType = tileType;
            this.sprite = sprite;
        }
    }
}
