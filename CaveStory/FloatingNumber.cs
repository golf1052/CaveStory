using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public class FloatingNumber
    {
        public enum NumberType
        {
            Damage,
            Experience
        }

        static VelocityUnit Velocity { get { return -Units.HalfTile / 250; } }
        static TimeSpan DamageTime { get { return TimeSpan.FromMilliseconds(2000); } }

        ContentManager Content;
        GameUnit offsetY;
        bool shouldRise;
        GameUnit centerX;
        GameUnit centerY;

        private HPUnit val;
        public HPUnit Value
        {
            set
            {
                shouldRise = val == 0;
                val += value;
                if (shouldRise)
                {
                    offsetY = 0;
                }
                timer.Reset();
            }
        }

        Timer timer;
        NumberType type;

        public FloatingNumber(ContentManager Content, NumberType type)
        {
            this.Content = Content;
            shouldRise = true;
            timer = new Timer(DamageTime);
            this.type = type;
            offsetY = 0;
            val = 0;
            centerX = 0;
            centerY = 0;
        }

        public bool Update(GameTime gameTime)
        {
            if (timer.Expired)
            {
                val = 0;
            }
            else if (shouldRise)
            {
                offsetY = (float)Math.Max(-Units.TileToGame(1), offsetY + Velocity * gameTime.ElapsedGameTime.TotalMilliseconds);
            }
            return !timer.Expired;
        }

        public void SetPosition(GameUnit centerX, GameUnit centerY)
        {
            this.centerX = centerX;
            this.centerY = centerY;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (timer.Expired)
            {
                return;
            }
            if (type == NumberType.Damage)
            {
                NumberSprite number = NumberSprite.DamageNumber(Content, val);
                number.LoadNumber();
                number.DrawCentered(spriteBatch, centerX, centerY + offsetY);
            }
            else
            {
                NumberSprite number = NumberSprite.ExperienceNumber(Content, val);
                number.LoadNumber();
                number.DrawCentered(spriteBatch, centerX, centerY + offsetY);
            }
        }
    }
}
