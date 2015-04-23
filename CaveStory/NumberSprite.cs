using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public class NumberSprite
    {
        Sprite sprite;

        public NumberSprite(ContentManager Content, int number)
        {
            sprite = new Sprite(Content, "TextBox",
                Units.GameToPixel(number * Units.HalfTile), Units.GameToPixel(7 * Units.HalfTile),
                Units.GameToPixel(Units.HalfTile), Units.GameToPixel(Units.HalfTile));
        }

        public void Draw(SpriteBatch spriteBatch, GameUnit x, GameUnit y)
        {
            sprite.Draw(spriteBatch, x, y);
        }
    }
}
