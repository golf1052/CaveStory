using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CaveStory
{
    public class FixedBackdrop : Backdrop
    {
        Texture2D tex;
        static TileUnit BackgroundSize { get { return 4; } }

        public FixedBackdrop(string filePath, ContentManager Content)
        {
            tex = Content.Load<Texture2D>(filePath);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (TileUnit x = 0; x < Game1.ScreenWidth; x += BackgroundSize)
            {
                for (TileUnit y = 0; y < Game1.ScreenHeight; y += BackgroundSize)
                {
                    spriteBatch.Draw(tex, new Vector2(Units.TileToPixel(x), Units.TileToPixel(y)), Color.White);
                }
            }
        }
    }
}
