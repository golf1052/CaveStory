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
        const string SpritePath = "TextBox";
        static GameUnit SourceY { get { return 7 * Units.HalfTile; } }
        static GameUnit SourceWidth { get { return Units.HalfTile; } }
        static GameUnit SourceHeight { get { return Units.HalfTile; } }

        List<Sprite> reverseDigits;
        GameUnit padding;

        ContentManager Content;
        public int number;
        int numDigits;

        // if numDigits = 0, don't care how much space it takes up
        public NumberSprite(ContentManager Content, int number, int numDigits = 0)
        {
            this.Content = Content;
            this.number = number;
            this.numDigits = numDigits;
        }

        void LoadNumber()
        {
            reverseDigits = new List<Sprite>();
            padding = 0;
            int digitCount = 0;
            do
            {
                int digit = number % 10;
                reverseDigits.Add(new Sprite(Content, SpritePath,
                Units.GameToPixel(digit * Units.HalfTile), Units.GameToPixel(SourceY),
                Units.GameToPixel(SourceWidth), Units.GameToPixel(SourceHeight)));
                number /= 10;
                digitCount++;
            }
            while (number != 0);

            padding = numDigits == 0 ? 0 : Units.HalfTile * (numDigits - digitCount);
        }

        public void Draw(SpriteBatch spriteBatch, GameUnit x, GameUnit y)
        {
            LoadNumber();
            for (int i = 0; i < reverseDigits.Count; i++)
            {
                GameUnit offset = Units.HalfTile * (reverseDigits.Count - 1 - i);
                reverseDigits[i].Draw(spriteBatch, x + offset + padding, y);
            }
        }
    }
}
