using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public interface IProjectile
    {
        Rectangle CollisionRectangle { get; }

        HPUnit ContactDamage { get; }

        void CollideWithEnemy();
    }
}
