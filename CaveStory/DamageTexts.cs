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
        public Dictionary<DamageText, Damageable> damageTextDict;

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
            for (int i = 0; i < damageTextDict.Count;)
            {
                if (damageTextDict.ElementAt(i).Value != null)
                {
                    Damageable damageable = damageTextDict.ElementAt(i).Value;
                    damageTextDict.ElementAt(i).Key.SetPosition(damageable.CenterX, damageable.CenterY);
                }
                if (damageTextDict.ElementAt(i).Key.Update(gameTime) ||
                    damageTextDict.ElementAt(i).Value != null)
                {
                    i++;
                }
                else
                {
                    damageTextDict.Remove(damageTextDict.ElementAt(i++).Key);
                }
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
