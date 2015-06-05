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
        public Timer damageTimer;

        public Health(ContentManager Content)
        {
            damage = 0;
            damageTimer = new Timer(PlayerHealth.DamageDelay);
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
            damageTimer.Reset();
            ResetFillSprites();
            return false;
        }

        public void AddHealth(HPUnit health)
        {
            if (damage > health)
            {
                damage -= health;
                health = 0;
            }
            else if (damage > 0)
            {
                health -= damage;
                damage = 0;
            }
            currentHealth = Math.Min(maxHealth, currentHealth + health);
            ResetFillSprites();
        }

        private void ResetFillSprites()
        {
            healthFillSprite.SetPercentageWidth((float)(currentHealth - damage) / (float)maxHealth);
            damageFillSprite.SetPercentageWidth((float)currentHealth / (float)maxHealth);
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
