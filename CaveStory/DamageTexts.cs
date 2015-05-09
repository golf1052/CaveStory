using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public class DamageTexts
    {
        Dictionary<DamageText, Damageable> damageTextDict;

        public DamageTexts()
        {
            damageTextDict = new Dictionary<DamageText, Damageable>();
        }

        public void AddDamageable(Damageable damageable)
        {
            damageTextDict[damageable.DamageText] = damageable;
        }

        public void Update(GameTime gameTime)
        {
            foreach (KeyValuePair<DamageText, Damageable> pair in damageTextDict)
            {
                if (pair.Value != null)
                {
                    Damageable damageable = pair.Value;
                    pair.Key.SetPosition(damageable.CenterX, damageable.CenterY);
                }
                pair.Key.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (KeyValuePair<DamageText, Damageable> pair in damageTextDict)
            {
                pair.Key.Draw(spriteBatch);
            }
        }
    }
}
