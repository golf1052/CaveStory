using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace CaveStory
{
    public class AnimatedSprite : Sprite
    {
        TimeSpan frameTime;
        FrameUnit numberOfFrames;
        FrameUnit currentFrame;
        TimeSpan elapsedTime; // Elapsed time sinced the last frame change

        public AnimatedSprite(ContentManager Content,
            string fileName,
            PixelUnit sourceX, PixelUnit sourceY,
            PixelUnit width, PixelUnit height,
            int fps, FrameUnit numberOfFrames) : base(Content, fileName, sourceX, sourceY, width, height)
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
                    sourceRect.X += sourceRect.Width;
                }
                else
                {
                    sourceRect.X -= sourceRect.Width * (numberOfFrames - 1);
                    currentFrame = 0;
                }
            }
        }
    }
}
