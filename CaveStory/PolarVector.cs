using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public class PolarVector
    {
        public GameUnit magnitude;
        public DegreesUnit angle;

        public GameUnit X { get { return magnitude * (float)Math.Cos(MathHelper.ToRadians(angle)); } }
        public GameUnit Y { get { return magnitude * (float)Math.Sin(MathHelper.ToRadians(angle)); } }

        public PolarVector(GameUnit magnitude, DegreesUnit angle)
        {
            this.magnitude = magnitude;
            this.angle = angle;
        }
    }
}
