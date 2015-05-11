using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public struct BatSpriteState
    {
        private Tuple<SpriteState.HorizontalFacing> tuple;
        public SpriteState.HorizontalFacing HorizontalFacing { get { return tuple.Item1; } }
        public BatSpriteState(Tuple<SpriteState.HorizontalFacing> tuple)
        {
            this.tuple = tuple;
        }
    }

    public class FirstCaveBat : Damageable
    {
        public GameUnit x;
        public GameUnit y;
        static FrameUnit NumFlyFrames { get { return 3; } }
        static int FlyFps { get { return 13; } }
        DegreesUnit flightAngle;
        static AngularVelocityUnit AngularVelocity { get { return 120.0f / 1000.0f; } }
        SpriteState.HorizontalFacing facing;

        public BatSpriteState SpriteState { get { return new BatSpriteState(
            new Tuple<SpriteState.HorizontalFacing>(facing)); } }

        static GameUnit FlightAmplitude { get { return 5 * Units.HalfTile; } }

        public HPUnit ContactDamage { get { return 1; } }

        Dictionary<BatSpriteState, Sprite> sprites;
        DamageText damageText;
        public DamageText DamageText { get { return damageText; } }

        GameUnit flightCenterY;

        bool alive;

        public GameUnit CenterX { get { return x + Units.HalfTile; } }
        public GameUnit CenterY { get { return y + Units.HalfTile; } }

        public Rectangle DamageRectangle
        {
            get
            {
                return new Rectangle((int)Math.Round(x + Units.HalfTile),
                    (int)Math.Round(y + Units.HalfTile),
                    0, 0);
            }
        }

        public Rectangle CollisionRectangle
        {
            get
            {
                return new Rectangle((int)Math.Round(x), (int)Math.Round(y),
                    (int)Units.TileToGame(1), (int)Units.TileToGame(1));
            }
        }

        public FirstCaveBat(ContentManager Content, GameUnit x, GameUnit y)
        {
            sprites = new Dictionary<BatSpriteState, Sprite>();
            this.x = x;
            this.y = y;
            flightCenterY = y;
            alive = true;
            flightAngle = 0.0f;
            facing = CaveStory.SpriteState.HorizontalFacing.Right;
            damageText = new DamageText(Content);
            InitializeSprites(Content);
        }

        public void InitializeSprites(ContentManager Content)
        {
            for (SpriteState.HorizontalFacing facing = CaveStory.SpriteState.HorizontalFacing.FirstHorizontalFacing;
                facing < CaveStory.SpriteState.HorizontalFacing.LastHorizontalFacing; facing++)
            {
                InitializeSprite(Content, new BatSpriteState(new Tuple<SpriteState.HorizontalFacing>(facing)));
            }
        }

        public void InitializeSprite(ContentManager Content, BatSpriteState spriteState)
        {
            TileUnit tileY = spriteState.HorizontalFacing == CaveStory.SpriteState.HorizontalFacing.Right ? (uint)3 : (uint)2;
            sprites[spriteState] = new AnimatedSprite(Content, "Npc\\NpcCemet",
                Units.TileToPixel(2), Units.TileToPixel(tileY),
                Units.TileToPixel(1), Units.TileToPixel(1),
                FlyFps, NumFlyFrames);
        }

        public bool Update(GameTime gameTime, GameUnit playerX)
        {
            flightAngle += AngularVelocity *
                (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            facing = x + Units.HalfTile > playerX ?
                CaveStory.SpriteState.HorizontalFacing.Left : CaveStory.SpriteState.HorizontalFacing.Right;
            y = flightCenterY + FlightAmplitude * (float)Math.Sin(MathHelper.ToRadians(flightAngle));
            sprites[SpriteState].Update();
            return alive;
        }

        public void TakeDamage(HPUnit damage)
        {
            damageText.Damage = damage;
            alive = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprites[SpriteState].Draw(spriteBatch, x, y);
        }
    }
}
