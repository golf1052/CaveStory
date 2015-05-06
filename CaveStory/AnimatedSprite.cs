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
        FrameUnit numberOfFrames;
        FrameUnit currentFrame;
        Timer frameTimer;

        public AnimatedSprite(ContentManager Content,
            string fileName,
            PixelUnit sourceX, PixelUnit sourceY,
            PixelUnit width, PixelUnit height,
            int fps, FrameUnit numberOfFrames) : base(Content, fileName, sourceX, sourceY, width, height)
        {
            frameTimer = new Timer(TimeSpan.FromMilliseconds(1000 / fps));
            this.numberOfFrames = numberOfFrames;
            currentFrame = 0;
        }

        public override void Update()
        {
            if (frameTimer.Expired)
            {
                currentFrame++;
                frameTimer.Reset();
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
