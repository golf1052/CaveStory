using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public struct CollisionInfo
    {
        public GameUnit position;
        public Tile2D tilePosition;
        public BitArray tileType;

        public CollisionInfo(GameUnit position, Tile2D tilePosition, BitArray tileType)
        {
            this.position = position;
            this.tilePosition = tilePosition;
            this.tileType = tileType;
        }
    }
}
