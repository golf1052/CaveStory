using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CaveStory
{
    public class Kinematics2D
    {
        Position2D Position { get { return new Position2D(new Vector2(x.position, y.position)); } }
        Velocity2D Velocity { get { return new Velocity2D(new Vector2(x.velocity, y.velocity)); } }
        Kinematics x;
        Kinematics y;

        public Kinematics2D(Kinematics x, Kinematics y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
