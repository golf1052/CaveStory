using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CaveStory
{
    public class PowerDoritoPickup : MapCollidable, IPickup
    {
        public enum SizeType
        {
            Small,
            Medium,
            Large
        }

        static GunExperienceUnit[] Values { get { return new GunExperienceUnit[] { 1, 5, 20 }; } }
        const string SpriteName = "Npc\\NpcSym";
        static TileUnit SourceX { get { return 0; } }
        static TileUnit[] SourceYs { get { return new TileUnit[] { 1, 2, 3 }; } }
        static TileUnit SourceWidth = 1;
        static TileUnit SourceHeight = 1;
        const int Fps = 14;
        static FrameUnit NumFrames { get { return 6; } }
        static TimeSpan LifeTime { get { return TimeSpan.FromMilliseconds(8000); } }
        static TimeSpan FlashTime { get { return TimeSpan.FromMilliseconds(7000); } }
        static TimeSpan FlashPeriod { get { return TimeSpan.FromMilliseconds(50); } }
        static VelocityUnit BounceSpeed { get { return 0.225f; } }
        static FrictionAccelerator Friction { get { return new FrictionAccelerator(0.00002f); } }
        static SimpleCollisionRectangle[] CollisionRectangles
        {
            get
            {
                return new SimpleCollisionRectangle[]
                {
                    new SimpleCollisionRectangle(new Rectangle(8, 8, 16, 16)),
                    new SimpleCollisionRectangle(new Rectangle(4, 4, 24, 24)),
                    new SimpleCollisionRectangle(new Rectangle(0, 0, 32, 32))
                };
            }
        }

        public Pickup.PickupType Type { get { return Pickup.PickupType.Experience; } }

        public int Value
        {
            get
            {
                return Values[(int)size];
            }
        }

        public Rectangle CollisionRectangle
        {
            get
            {
                return new Rectangle((int)Math.Round(kinematicsX.position) + CollisionRectangles[(int)size].BoundingBox.Left,
                    (int)Math.Round(kinematicsY.position) + CollisionRectangles[(int)size].BoundingBox.Top,
                    CollisionRectangles[(int)size].BoundingBox.Width,
                    CollisionRectangles[(int)size].BoundingBox.Height);
            }
        }

        Kinematics kinematicsX;
        Kinematics kinematicsY;
        AnimatedSprite sprite;
        SizeType size;
        Timer timer;

        public PowerDoritoPickup(ContentManager Content, GameUnit centerX, GameUnit centerY, SizeType size)
        {
            kinematicsX = new Kinematics(centerX - Units.HalfTile, (Game1.Random.Next(0, 11) - 5) * 0.025f);
            kinematicsY = new Kinematics(centerY - Units.HalfTile, (Game1.Random.Next(0, 11) - 5) * 0.025f);
            sprite = new AnimatedSprite(Content, SpriteName,
                Units.TileToPixel(SourceX), Units.TileToPixel(SourceYs[(int)size]),
                Units.TileToPixel(SourceWidth), Units.TileToPixel(SourceHeight),
                Fps, NumFrames);
            this.size = size;
            timer = new Timer(LifeTime, true);
        }

        

        public bool Update(GameTime gameTime, Map map)
        {
            sprite.Update();

            UpdateY(CollisionRectangles[(int)size], ConstantAccelerator.Gravity,
                kinematicsX, kinematicsY, gameTime, map);
            UpdateX(CollisionRectangles[(int)size], Friction,
                kinematicsX, kinematicsY, gameTime, map);
            return timer.Active;
        }

        protected override void OnCollision(TileInfo.SideType side, bool isDeltaDirection)
        {
            if (side == TileInfo.SideType.TopSide)
            {
                kinematicsY.velocity = 0;
            }
            else if (side == TileInfo.SideType.BottomSide)
            {
                kinematicsY.velocity = -BounceSpeed;
            }
            else
            {
                kinematicsX.velocity *= -1;
            }
        }

        protected override void OnDelta(TileInfo.SideType side)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (timer.CurrentTime < FlashTime || timer.CurrentTime.Ticks / FlashPeriod.Ticks % 2 == 0)
            {
                sprite.Draw(spriteBatch, kinematicsX.position, kinematicsY.position);
            }   
        }
    }
}
