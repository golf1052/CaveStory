using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public class Sprite
    {
        Texture2D tex;
        protected Rectangle sourceRect;
        const bool blackIsTransparent = true;

        public Sprite(ContentManager Content,
            string fileName,
            int sourceX, int sourceY,
            int width, int height)
        {
            tex = Content.Load<Texture2D>(fileName);
            //tex = Game1.LoadImage(Content, fileName, blackIsTransparent);
            sourceRect = new Rectangle(sourceX, sourceY, width, height);
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            Rectangle destinationRect = new Rectangle(x, y, sourceRect.Width, sourceRect.Height);
            spriteBatch.Draw(tex, destinationRect, sourceRect, Color.White);
        }
    }
}
