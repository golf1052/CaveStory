using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public class Tile
    {
        public BitArray tileType;
        public Sprite sprite;

        public Tile(BitArray tileType = null,
            Sprite sprite = null)
        {
            if (tileType == null)
            {
                tileType = TileInfo.CreateTileType();
                tileType.Set((int)TileInfo.TileFlag.Empty, true);
            }
            this.tileType = tileType;
            this.sprite = sprite;
        }
    }
}
