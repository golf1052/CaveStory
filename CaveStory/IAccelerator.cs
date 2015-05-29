using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CaveStory
{
    public interface IAccelerator
    {
        void UpdateVelocity(Kinematics kinematics, GameTime gameTime);
    }
}
