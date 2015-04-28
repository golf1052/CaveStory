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
        public enum ColorType
        {
            Red,
            White
        }

        public enum OperatorType
        {
            Plus,
            Minus,
            None
        }
        const string SpritePath = "TextBox";
        static GameUnit SourceWhiteY { get { return 7 * Units.HalfTile; } }
        static GameUnit SourceRedY { get { return 8 * Units.HalfTile; } }

        static GameUnit PlusSourceX { get { return 4 * Units.HalfTile; } }
        static GameUnit MinusSourceX { get { return 5 * Units.HalfTile; } }
        static GameUnit OpSourceY { get { return 6 * Units.HalfTile; } }
        
        static GameUnit SourceWidth { get { return Units.HalfTile; } }
        static GameUnit SourceHeight { get { return Units.HalfTile; } }
        const int Radix = 10;

        List<Sprite> reversedGlyphs;
        GameUnit padding;

        private GameUnit Width { get { return Units.HalfTile * reversedGlyphs.Count; } }
        private GameUnit Height { get { return Units.HalfTile; } }

        ContentManager Content;
        public int number;
        int numDigits;
        ColorType color;
        OperatorType op;

        public static NumberSprite HudNumber(ContentManager Content, int number, int numDigits = 0)
        {
            return new NumberSprite(Content, number, numDigits, ColorType.White, OperatorType.None);
        }

        public static NumberSprite DamageNumber(ContentManager Content, int number)
        {
            return new NumberSprite(Content, number, 0, ColorType.Red, OperatorType.Minus);
        }

        public static NumberSprite ExperienceNumber(ContentManager Content, int number)
        {
            return new NumberSprite(Content, number, 0, ColorType.White, OperatorType.Plus);
        }

        // if numDigits = 0, don't care how much space it takes up
        private NumberSprite(ContentManager Content, int number, int numDigits, ColorType color, OperatorType op)
        {
            this.Content = Content;
            this.number = number;
            this.numDigits = numDigits;
            this.color = color;
            this.op = op;
        }

        public void LoadNumber()
        {
            reversedGlyphs = new List<Sprite>();
            GameUnit sourceY = color == ColorType.Red ? SourceRedY : SourceWhiteY;
            padding = 0;
            int digitCount = 0;
            do
            {
                int digit = number % Radix;
                reversedGlyphs.Add(new Sprite(Content, SpritePath,
                    Units.GameToPixel(digit * Units.HalfTile), Units.GameToPixel(sourceY),
                    Units.GameToPixel(SourceWidth), Units.GameToPixel(SourceHeight)));
                number /= Radix;
                digitCount++;
            }
            while (number != 0);

            padding = numDigits == 0 ? 0 : Units.HalfTile * (numDigits - digitCount);
            switch (op)
            {
                case OperatorType.Plus:
                    reversedGlyphs.Add(new Sprite(Content, SpritePath,
                        Units.GameToPixel(PlusSourceX), Units.GameToPixel(OpSourceY),
                        Units.GameToPixel(SourceWidth), Units.GameToPixel(SourceHeight)));
                    break;
                case OperatorType.Minus:
                    reversedGlyphs.Add(new Sprite(Content, SpritePath,
                        Units.GameToPixel(MinusSourceX), Units.GameToPixel(OpSourceY),
                        Units.GameToPixel(SourceWidth), Units.GameToPixel(SourceHeight)));
                    break;
                case OperatorType.None:
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameUnit x, GameUnit y)
        {
            for (int i = 0; i < reversedGlyphs.Count; i++)
            {
                GameUnit offset = Units.HalfTile * (reversedGlyphs.Count - 1 - i);
                reversedGlyphs[i].Draw(spriteBatch, x + offset + padding, y);
            }
        }

        public void DrawCentered(SpriteBatch spriteBatch, GameUnit x, GameUnit y)
        {
            Draw(spriteBatch, x - Width / 2, y - Height / 2);
        }
    }
}
