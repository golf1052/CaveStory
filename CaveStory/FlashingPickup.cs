using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CaveStory
{
    public class FlashingPickup : IPickup
    {
        const string SpriteName = "Npc\\NpcSym";
        static TimeSpan LifeTime { get { return TimeSpan.FromMilliseconds(8000); } }
        static TimeSpan StartPeriod { get { return TimeSpan.FromMilliseconds(400); } }
        static TimeSpan EndPeriod { get { return TimeSpan.FromMilliseconds(75 * 3); } }
        static TimeSpan FlickerPeriod { get { return TimeSpan.FromMilliseconds(75); } }
        static TimeSpan FlickerTime { get { return LifeTime - TimeSpan.FromMilliseconds(1000); } }
        static TimeSpan DissipateTime { get { return LifeTime - TimeSpan.FromMilliseconds(25); } }
        static float FlashInterpolation { get { return ((float)EndPeriod.Ticks - (float)StartPeriod.Ticks) / (float)FlickerTime.Ticks; } }

        static TileUnit DissipatingSourceX { get { return 1; } }
        static TileUnit DissipatingSourceY { get { return 0; } }

        static TileUnit HeartSourceX { get { return 2; } }
        static TileUnit HeartSourceY { get { return 5; } }
        static Rectangle HeartRectangle { get { return new Rectangle(5, 8, 21, 19); } }
        static HPUnit HeartValue { get { return 2; } }

        static TileUnit MultiHeartSourceX { get { return 4; } }
        static TileUnit MultiHeartSourceY { get { return 5; } }
        static Rectangle MultiHeartRectangle { get { return new Rectangle(6, 7, 26, 25); } }
        static HPUnit MultiHeartValue { get { return 6; } }

        GameUnit x;
        GameUnit y;
        Timer timer;
        TimeSpan flashPeriod;

        Sprite sprite;
        Sprite flashSprite;
        Sprite dissipatingSprite;
        Rectangle rectangle;
        int value;
        Pickup.PickupType type;

        public int Value { get { return value; } }

        public Pickup.PickupType Type { get { return type; } }

        public Rectangle CollisionRectangle
        {
            get
            {
                return new Rectangle((int)Math.Round(x) + rectangle.Left,
                    (int)Math.Round(y) + rectangle.Top,
                    rectangle.Width,
                    rectangle.Height);
            }
        }

        public static IPickup HeartPickup(ContentManager Content, GameUnit centerX, GameUnit centerY)
        {
            return new FlashingPickup(Content,
                centerX, centerY,
                HeartSourceX, HeartSourceY,
                HeartRectangle,
                HeartValue, Pickup.PickupType.Health);
        }

        public static IPickup MultiHeartPickup(ContentManager Content, GameUnit centerX, GameUnit centerY)
        {
            return new FlashingPickup(Content,
                centerX, centerY,
                MultiHeartSourceX, MultiHeartSourceY,
                MultiHeartRectangle,
                MultiHeartValue, Pickup.PickupType.Health);
        }

        private FlashingPickup(ContentManager Content,
            GameUnit centerX, GameUnit centerY,
            TileUnit sourceX, TileUnit sourceY,
            Rectangle rectangle,
            int value, Pickup.PickupType type)
        {
            sprite = new Sprite(Content, SpriteName,
                Units.TileToPixel(sourceX), Units.TileToPixel(sourceY),
                Units.TileToPixel(1), Units.TileToPixel(1));
            flashSprite = new Sprite(Content, SpriteName,
                Units.TileToPixel(sourceX + 1), Units.TileToPixel(sourceY),
                Units.TileToPixel(1), Units.TileToPixel(1));
            dissipatingSprite = new Sprite(Content, SpriteName,
                Units.TileToPixel(DissipatingSourceX), Units.TileToPixel(DissipatingSourceY),
                Units.TileToPixel(1), Units.TileToPixel(1));
            x = centerX - Units.HalfTile;
            y = centerY - Units.HalfTile;
            timer = new Timer(LifeTime, true);
            flashPeriod = StartPeriod;
            this.rectangle = rectangle;
            this.value = value;
            this.type = type;
        }

        public bool Update(GameTime gameTime, Map map)
        {
            flashPeriod = timer.CurrentTime < FlickerTime ?
                TimeSpan.FromMilliseconds(FlashInterpolation * timer.CurrentTime.TotalMilliseconds + StartPeriod.TotalMilliseconds) :
                FlickerPeriod;
            return timer.Active;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (timer.CurrentTime > DissipateTime)
            {
                dissipatingSprite.Draw(spriteBatch, x, y);
            }
            else if (timer.CurrentTime > FlickerTime)
            {
                if (timer.CurrentTime.Ticks / flashPeriod.Ticks % 3 == 0)
                {
                    sprite.Draw(spriteBatch, x, y);
                }
                else if (timer.CurrentTime.Ticks / flashPeriod.Ticks % 3 == 1)
                {
                    flashSprite.Draw(spriteBatch, x, y);
                }
            }
            else
            {
                if (timer.CurrentTime.Ticks / flashPeriod.Ticks % 2 == 0)
                {
                    sprite.Draw(spriteBatch, x, y);
                }
                else
                {
                    flashSprite.Draw(spriteBatch, x, y);
                }
            }
        }
    }
}
