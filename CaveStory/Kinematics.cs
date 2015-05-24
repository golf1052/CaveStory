using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CaveStory
{
    public class Kinematics
    {
        public GameUnit position;
        public VelocityUnit velocity;

        public Kinematics(GameUnit position, VelocityUnit velocity)
        {
            this.position = position;
            this.velocity = velocity;
        }

        public GameUnit Delta(GameTime gameTime)
        {
            return velocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }
    }
}
