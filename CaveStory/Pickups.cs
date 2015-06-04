using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CaveStory
{
    public class Pickups
    {
        HashSet<IPickup> pickups;

        public Pickups()
        {
            pickups = new HashSet<IPickup>();
        }

        public void Add(IPickup pickup)
        {
            pickups.Add(pickup);
        }

        public void Update(GameTime gameTime, Map map)
        {
            for (int i = 0; i < pickups.Count;)
            {
                if (pickups.ElementAt(i).Update(gameTime, map))
                {
                    i++;
                }
                else
                {
                    pickups.Remove(pickups.ElementAt(i));
                }
            }
        }

        public void HandleCollisions(Player player)
        {
            for (int i = 0; i < pickups.Count;)
            {
                if (player.DamageRectangle.Intersects(pickups.ElementAt(i).CollisionRectangle))
                {
                    player.CollectPickup(pickups.ElementAt(i));
                    pickups.Remove(pickups.ElementAt(i));
                }
                else
                {
                    i++;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (IPickup pickup in pickups)
            {
                pickup.Draw(spriteBatch);
            }
        }
    }
}
