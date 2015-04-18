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
        static int FlyFps { get { return 10; } }

        public FirstCaveBat(ContentManager Content, GameUnit x, GameUnit y)
        {
            this.x = x;
            this.y = y;
            sprite = new AnimatedSprite(Content, "Npc\\NpcCemet",
                Units.TileToPixel(2), Units.TileToPixel(2),
                Units.TileToPixel(1), Units.TileToPixel(1),
                FlyFps, NumFlyFrames);
        }

        public void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, x, y);
        }
    }
}
