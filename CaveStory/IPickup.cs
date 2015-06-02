using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CaveStory
{
    public interface IPickup
    {

        int Value { get; }
        Pickup.PickupType Type { get; }

        Rectangle CollisionRectangle();
        void Draw(SpriteBatch spriteBatch);
        bool Update(GameTime gameTime, Map map);
    }

    public class Pickup
    {
        public enum PickupType
        {
            Health,
            Missiles,
            Experience
        }
    }
}
