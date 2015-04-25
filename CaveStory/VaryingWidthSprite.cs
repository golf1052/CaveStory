using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace CaveStory
{
    public class VaryingWidthSprite : Sprite
    {
        public PixelUnit Width { set { sourceRect.Width = value; } }
        public VaryingWidthSprite(ContentManager Content, string fileName,
            PixelUnit sourceX, PixelUnit sourceY,
            PixelUnit initialWidth, PixelUnit initialHeight) : base(Content, fileName, sourceX, sourceY, initialWidth, initialHeight)
        {
        }
    }
}
