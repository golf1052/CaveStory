using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public class HeadBumpParticle : IParticle
    {
        Sprite sprite;
        GameUnit centerX;
        GameUnit centerY;
        Timer timer;
        PolarVector particleA;
        GameUnit maxOffsetA;
        PolarVector particleB;
        GameUnit maxOffsetB;

        static GameUnit SourceX { get { return 116; } }
        static GameUnit SourceY { get { return 54; } }
        static GameUnit Width { get { return 6; } }
        static GameUnit Height { get { return 6; } }
        static TimeSpan LifeTime { get { return TimeSpan.FromMilliseconds(600); } }
        static TimeSpan FlashPeriod { get { return TimeSpan.FromMilliseconds(25); } }
        static VelocityUnit Speed { get { return 0.12f; } }

        public HeadBumpParticle(ContentManager Content,
            GameUnit centerX, GameUnit centerY)
        {
            this.centerX = centerX;
            this.centerY = centerY;
            sprite = new Sprite(Content, "Caret",
                Units.GameToPixel(SourceX), Units.GameToPixel(SourceY),
                Units.GameToPixel(Width), Units.GameToPixel(Height));
            timer = new Timer(LifeTime, true);
            particleA = new PolarVector(0, Game1.Random.Next(0, 360));
            maxOffsetA = 4 + Game1.Random.Next(0, 16);
            particleB = new PolarVector(0, Game1.Random.Next(0, 360));
            maxOffsetB = 4 + Game1.Random.Next(0, 16);
        }

        public bool Update(GameTime gameTime)
        {
            particleA.magnitude = Math.Min(particleA.magnitude + Speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds,
                maxOffsetA);
            particleB.magnitude = Math.Min(particleB.magnitude + Speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds,
                maxOffsetB);
            return timer.Active;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (timer.CurrentTime.Ticks / FlashPeriod.Ticks % 2 == 0)
            {
                sprite.Draw(spriteBatch, centerX + particleA.X, centerY + particleA.Y);
                sprite.Draw(spriteBatch, centerX + particleB.X, centerY + particleB.Y);
            }
        }
    }
}
