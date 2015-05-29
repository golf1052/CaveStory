using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CaveStory
{
    public class ConstantAccelerator : IAccelerator
    {
        // Fall Motion
        // (pixels / millisecond) / millisecond
        static AccelerationUnit GravityAcceleration { get { return 0.00078125f; } }
        // pixels / millisecond
        public static VelocityUnit TerminalSpeed { get { return 0.2998046875f; } }

        AccelerationUnit acceleration;
        VelocityUnit maxVelocity;
        public static ConstantAccelerator Gravity = new ConstantAccelerator(GravityAcceleration, TerminalSpeed);

        public ConstantAccelerator(AccelerationUnit acceleration, VelocityUnit maxVelocity)
        {
            this.acceleration = acceleration;
            this.maxVelocity = maxVelocity;
        }

        public void UpdateVelocity(Kinematics kinematics, GameTime gameTime)
        {
            if (acceleration < 0)
            {
                kinematics.velocity = Math.Max(kinematics.velocity + acceleration * (float)gameTime.ElapsedGameTime.TotalMilliseconds, maxVelocity);
            }
            else
            {
                kinematics.velocity = Math.Min(kinematics.velocity + acceleration * (float)gameTime.ElapsedGameTime.TotalMilliseconds, maxVelocity);
            }
        }
    }
}
