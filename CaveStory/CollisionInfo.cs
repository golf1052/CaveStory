using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public struct CollisionInfo
    {
        public bool collided;
        public int row;
        public int col;

        public CollisionInfo(bool collided, int row, int col)
        {
            this.collided = collided;
            this.row = row;
            this.col = col;
        }
    }
}
