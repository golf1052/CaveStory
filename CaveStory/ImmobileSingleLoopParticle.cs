using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CaveStory
{
    public class ImmobileSingleLoopParticle : IParticle
    {
        GameUnit x;
        GameUnit y;
        AnimatedSprite sprite;

        public ImmobileSingleLoopParticle(ContentManager Content, string spriteName,
            PixelUnit sourceX, PixelUnit sourceY,
            PixelUnit sourceWidth, PixelUnit sourceHeight,
            int fps, FrameUnit numFrames,
            GameUnit x, GameUnit y)
        {
            this.x = x;
            this.y = y;
            sprite = new AnimatedSprite(Content, spriteName,
                sourceX, sourceY,
                sourceWidth, sourceHeight,
                fps, numFrames);
        }

        public bool Update(GameTime gameTime)
        {
            sprite.Update();
            return sprite.NumCompletedLoops == 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, x, y);
        }
    }
}
