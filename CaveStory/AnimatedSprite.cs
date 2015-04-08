using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CaveStory
{
    public class AnimatedSprite : Sprite
    {
        TimeSpan frameTime;
        int numberOfFrames;
        int currentFrame;
        TimeSpan elapsedTime; // Elapsed time sinced the last frame change

        public AnimatedSprite(Texture2D loadedTex,
            int sourceX, int sourceY,
            int width, int height,
            int fps, int numberOfFrames) : base(loadedTex, sourceX, sourceY, width, height)
        {
            frameTime = TimeSpan.FromMilliseconds(1000 / fps);
            this.numberOfFrames = numberOfFrames;
            currentFrame = 0;
            elapsedTime = TimeSpan.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;
            if (elapsedTime > frameTime)
            {
                currentFrame++;
                elapsedTime = TimeSpan.Zero;
                if (currentFrame < numberOfFrames)
                {
                    sourceRect.X += Game1.TileSize;
                }
                else
                {
                    sourceRect.X -= Game1.TileSize * (numberOfFrames - 1);
                    currentFrame = 0;
                }
            }
        }
    }
}
