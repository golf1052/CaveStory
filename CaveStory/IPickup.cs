using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CaveStory
{
    public abstract class IPickup
    {
        public enum PickupType
        {
            Health,
            Missiles,
            Experience
        }

        public abstract int Value { get; }
        public abstract PickupType Type { get; }

        public abstract Rectangle CollisionRectangle();
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract bool Update(GameTime gameTime, Map map);
    }
}
