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
        const int BackgroundSize = Game1.TileSize * 4;

        public FixedBackdrop(string filePath, ContentManager Content)
        {
            tex = Content.Load<Texture2D>(filePath);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < Game1.ScreenWidth; x += BackgroundSize)
            {
                for (int y = 0; y < Game1.ScreenHeight; y += BackgroundSize)
                {
                    spriteBatch.Draw(tex, new Vector2(x, y), Color.White);
                }
            }
        }
    }
}
