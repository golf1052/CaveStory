using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public class DamageText
    {
        static VelocityUnit Velocity { get { return -Units.HalfTile / 250; } }
        static TimeSpan DamageTime { get { return TimeSpan.FromMilliseconds(2000); } }

        ContentManager Content;
        GameUnit offsetY;
        bool shouldRise;

        private HPUnit damage;
        public HPUnit Damage
        {
            set
            {
                shouldRise = damage == 0;
                damage += value;
                if (shouldRise)
                {
                    offsetY = 0;
                }
                timer.Reset();
            }
        }

        Timer timer;

        public DamageText(ContentManager Content)
        {
            this.Content = Content;
            shouldRise = true;
            timer = new Timer(DamageTime);
            offsetY = 0;
            damage = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (timer.Expired)
            {
                damage = 0;
            }
            else if (shouldRise)
            {
                offsetY = (float)Math.Max(-Units.TileToGame(1), offsetY + Velocity * gameTime.ElapsedGameTime.TotalMilliseconds);
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameUnit centerX, GameUnit centerY)
        {
            if (timer.Expired)
            {
                return;
            }
            NumberSprite number = NumberSprite.DamageNumber(Content, damage);
            number.LoadNumber();
            number.DrawCentered(spriteBatch, centerX, centerY + offsetY);
        }
    }
}
