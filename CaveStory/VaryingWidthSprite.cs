using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace CaveStory
{
    public class VaryingWidthSprite : Sprite
    {
        private PixelUnit maxWidth;

        public VaryingWidthSprite(ContentManager Content, string fileName,
            PixelUnit sourceX, PixelUnit sourceY,
            PixelUnit maxWidth, PixelUnit initialWidth,
            PixelUnit initialHeight) : base(Content, fileName, sourceX, sourceY, initialWidth, initialHeight)
        {
            this.maxWidth = maxWidth;
        }

        public void SetPercentageWidth(float percentage)
        {
            sourceRect.Width = (int)(percentage * maxWidth);
        }
    }
}
