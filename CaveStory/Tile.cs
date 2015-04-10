using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public class Tile
    {
        public enum TileType
        {
            AirTile,
            WallTile
        }

        public TileType tileType;
        public Sprite sprite;

        public Tile(TileType tileType = Tile.TileType.AirTile,
            Sprite sprite = null)
        {
            this.tileType = tileType;
            this.sprite = sprite;
        }
    }
}
