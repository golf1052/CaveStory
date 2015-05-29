using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CaveStory
{
    public class ZeroAccelerator : IAccelerator
    {
        public static ZeroAccelerator Zero = new ZeroAccelerator();

        public void UpdateVelocity(Kinematics kinematics, GameTime gameTime)
        {
        }
    }
}
