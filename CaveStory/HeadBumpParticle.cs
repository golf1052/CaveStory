using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public class HeadBumpParticle
    {
        Sprite sprite;
        GameUnit centerX;
        GameUnit centerY;
        Timer timer;

        static GameUnit SourceX { get { return 116; } }
        static GameUnit SourceY { get { return 54; } }
        static GameUnit Width { get { return 6; } }
        static GameUnit Height { get { return 6; } }
        static TimeSpan LifeTime { get { return TimeSpan.FromMilliseconds(600); } }

        public HeadBumpParticle(ContentManager Content,
            GameUnit centerX, GameUnit centerY)
        {
            this.centerX = centerX;
            this.centerY = centerY;
            sprite = new Sprite(Content, "Caret",
                Units.GameToPixel(SourceX), Units.GameToPixel(SourceY),
                Units.GameToPixel(Width), Units.GameToPixel(Height));
            timer = new Timer(LifeTime);
        }

        public bool Update(GameTime gameTime)
        {
            return !timer.Expired;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, centerX, centerY);
        }
    }
}
