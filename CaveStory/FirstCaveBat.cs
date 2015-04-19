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
        public FirstCaveBat.Facing facing;
        public BatSpriteState(FirstCaveBat.Facing facing)
        {
            this.facing = facing;
        }

        public static bool operator < (BatSpriteState a, BatSpriteState b)
        {
            return a.facing < b.facing;
        }

        public static bool operator > (BatSpriteState a, BatSpriteState b)
        {
            return b < a;
        }
    }

    public class FirstCaveBat
    {
        Sprite sprite;
        public GameUnit x;
        public GameUnit y;
        static FrameUnit NumFlyFrames { get { return 3; } }
        static int FlyFps { get { return 13; } }
        DegreesUnit flightAngle;
        static AngularVelocityUnit AngularVelocity { get { return 120.0f / 1000.0f; } }
        public enum Facing
        {
            FirstFacing,
            Left = FirstFacing,
            Right,
            LastFacing
        }
        Facing facing;

        public BatSpriteState SpriteState
        {
            get
            {
                return new BatSpriteState(facing);
            }
        }

        Dictionary<BatSpriteState, Sprite> sprites;

        public FirstCaveBat(ContentManager Content, GameUnit x, GameUnit y)
        {
            sprites = new Dictionary<BatSpriteState, Sprite>();
            this.x = x;
            this.y = y;
            flightAngle = 0.0f;
            facing = Facing.Right;
            InitializeSprites(Content);
        }

        public void InitializeSprites(ContentManager Content)
        {
            for (Facing facing = Facing.FirstFacing; facing < Facing.LastFacing; facing++)
            {
                InitializeSprite(Content, new BatSpriteState(facing));
            }
        }

        public void InitializeSprite(ContentManager Content, BatSpriteState spriteState)
        {
            TileUnit tileY = spriteState.facing == Facing.Right ? (uint)3 : (uint)2;
            sprites[spriteState] = new AnimatedSprite(Content, "Npc\\NpcCemet",
                Units.TileToPixel(2), Units.TileToPixel(tileY),
                Units.TileToPixel(1), Units.TileToPixel(1),
                FlyFps, NumFlyFrames);
        }

        public void Update(GameTime gameTime, GameUnit playerX)
        {
            flightAngle += AngularVelocity *
                (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            facing = x + Units.TileToGame(1) / 2.0f > playerX ?
                Facing.Left : Facing.Right;
            sprites[SpriteState].Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            GameUnit modifiedY = y + Units.TileToGame(5) / 2.0f * (float)Math.Sin(MathHelper.ToRadians(flightAngle));
            sprites[SpriteState].Draw(spriteBatch, x, modifiedY);
        }
    }
}
