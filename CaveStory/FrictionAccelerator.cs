using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CaveStory
{
    public class FrictionAccelerator : IAccelerator
    {
        AccelerationUnit friction;

        public FrictionAccelerator(AccelerationUnit friction)
        {
            this.friction = friction;
        }

        public void UpdateVelocity(Kinematics kinematics, GameTime gameTime)
        {
            kinematics.velocity = kinematics.velocity > 0.0f ?
                (float)Math.Max(0.0f, kinematics.velocity - friction * gameTime.ElapsedGameTime.TotalMilliseconds) :
                (float)Math.Min(0.0f, kinematics.velocity + friction * gameTime.ElapsedGameTime.TotalMilliseconds);
        }
    }
}
