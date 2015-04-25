using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public class Health
    {
        public Sprite healthBarSprite;
        public VaryingWidthSprite healthFillSprite;
        public VaryingWidthSprite damageFillSprite;
        public NumberSprite healthNumberSprite;

        public HPUnit maxHealth;
        public HPUnit currentHealth;
        public HPUnit damage;
        public TimeSpan damageTime;

        public Health(ContentManager Content)
        {
            damage = 0;
            damageTime = TimeSpan.Zero;
            maxHealth = 6;
            currentHealth = 6;
        }

        /// <summary>
        /// Returns true if we've died
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        public bool TakeDamage(HPUnit damage)
        {
            this.damage = damage;
            damageTime = TimeSpan.Zero;
            healthFillSprite.Width = Units.GameToPixel(FillOffset(currentHealth - damage));
            damageFillSprite.Width = Units.GameToPixel(FillOffset(damage));
            return false;
        }

        public GameUnit FillOffset(HPUnit health)
        {
            return PlayerHealth.MaxFillWidth * health / maxHealth;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
