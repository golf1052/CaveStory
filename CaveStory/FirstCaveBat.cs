using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public class FirstCaveBat
    {
        Sprite sprite;
        public GameUnit x;
        public GameUnit y;
        static FrameUnit NumFlyFrames { get { return 3; } }
        static int FlyFps { get { return 13; } }
        DegreesUnit flightAngle;
        static AngularVelocityUnit AngularVelocity { get { return 120.0f / 1000.0f; } }

        public FirstCaveBat(ContentManager Content, GameUnit x, GameUnit y)
        {
            this.x = x;
            this.y = y;
            flightAngle = 0.0f;
            sprite = new AnimatedSprite(Content, "Npc\\NpcCemet",
                Units.TileToPixel(2), Units.TileToPixel(2),
                Units.TileToPixel(1), Units.TileToPixel(1),
                FlyFps, NumFlyFrames);
        }

        public void Update(GameTime gameTime)
        {
            flightAngle += AngularVelocity *
                (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            GameUnit modifiedY = y + Units.TileToGame(5) / 2.0f * (float)Math.Sin(MathHelper.ToRadians(flightAngle));
            sprite.Draw(spriteBatch, x, modifiedY);
        }
    }
}
