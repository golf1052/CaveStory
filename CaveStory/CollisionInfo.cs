using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public struct CollisionInfo
    {
        public bool collided;
        public TileUnit row;
        public TileUnit col;

        public CollisionInfo(bool collided, TileUnit row, TileUnit col)
        {
            this.collided = collided;
            this.row = row;
            this.col = col;
        }
    }
}
