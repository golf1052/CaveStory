using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public struct TestCollisionInfo
    {
        public bool isColliding;
        public GameUnit position;

        public TestCollisionInfo(bool isColliding, GameUnit position)
        {
            this.isColliding = isColliding;
            this.position = position;
        }
    }
}
