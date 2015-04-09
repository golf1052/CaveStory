using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public class Player
    {
        

        // Walk Motion
        const float SlowDownFactor = 0.8f;
        const float WalkingAcceleration = 0.0012f; // (pixels / millisecond) / millisecond
        const float MaxSpeedX = 0.325f; // pixels / millisecond

        // Fall Motion
        const float Gravity = 0.0012f; // (pixels / millisecond) / millisecond
        const float MaxSpeedY = 0.325f; // pixels / millisecond

        // Jump Motion
        const float JumpSpeed = 0.325f; // pixels / millisecond
        public static TimeSpan JumpTime = TimeSpan.FromMilliseconds(275);

        // Sprites
        const string SpriteFilePath = "MyChar";

        // Sprite Frames
        const int CharacterFrame = 0;

        const int WalkFrame = 0;
        const int StandFrame = 0;
        const int JumpFrame = 1;
        const int FallFrame = 2;
        const int UpFrameOffset = 3;
        const int DownFrame = 6;
        const int BackFrame = 7;

        // Walk Animation
        const int NumWalkFrames = 3;
        const int WalkFps = 15;

        int x;
        int y;
        float velocityX;
        float velocityY;
        float accelerationX;
        SpriteState.HorizontalFacing horizontalFacing;
        SpriteState.VerticalFacing verticalFacing;
        private bool onGround;
        bool OnGround
        {
            get
            {
                return onGround;
            }
        }
        private Jump jump;

        Dictionary<SpriteState, Sprite> sprites;

        private SpriteState spriteState;
        public SpriteState SpriteState
        {
            get
            {
                SpriteState.MotionType motion;
                if (OnGround)
                {
                    motion = accelerationX == 0.0f ? SpriteState.MotionType.Standing : SpriteState.MotionType.Walking;
                }
                else
                {
                    motion = velocityY < 0.0f ? SpriteState.MotionType.Jumping : SpriteState.MotionType.Falling;
                }
                return new SpriteState(motion, horizontalFacing, verticalFacing);
            }
        }

        public Player(ContentManager Content, int x, int y)
        {
            sprites = new Dictionary<SpriteState, Sprite>();
            InitializeSprites(Content);
            this.x = x;
            this.y = y;
            velocityX = 0;
            velocityY = 0;
            accelerationX = 0;
            horizontalFacing = SpriteState.HorizontalFacing.Left;
            verticalFacing = SpriteState.VerticalFacing.Horizontal;
            onGround = false;
            jump = new Jump();
        }

        public void InitializeSprites(ContentManager Content)
        {
            for (SpriteState.MotionType motionType = SpriteState.MotionType.FirstMotionType;
                motionType < SpriteState.MotionType.LastMotionType;
                ++motionType)
            {
                for (SpriteState.HorizontalFacing horizontalFacing = SpriteState.HorizontalFacing.FirstHorizontalFacing;
                    horizontalFacing < SpriteState.HorizontalFacing.LastHorizontalFacing;
                    ++horizontalFacing)
                {
                    for (SpriteState.VerticalFacing verticalFacing = SpriteState.VerticalFacing.FirstVerticalFacing;
                        verticalFacing < SpriteState.VerticalFacing.LastVerticalFacing;
                        ++verticalFacing)
                    {
                        InitializeSprite(Content, new SpriteState(motionType, horizontalFacing, verticalFacing));
                    }
                }
            }
        }

        public void InitializeSprite(ContentManager Content, SpriteState spriteState)
        {
            int sourceY = spriteState.horizontalFacing == SpriteState.HorizontalFacing.Left ?
                CharacterFrame * Game1.TileSize : (1 + CharacterFrame) * Game1.TileSize;

            int sourceX = 0;
            switch (spriteState.motionType)
            {
                case SpriteState.MotionType.Walking:
                    sourceX = WalkFrame * Game1.TileSize;
                    break;
                case SpriteState.MotionType.Standing:
                    sourceX = StandFrame * Game1.TileSize;
                    break;
                case SpriteState.MotionType.Jumping:
                    sourceX = JumpFrame * Game1.TileSize;
                    break;
                case SpriteState.MotionType.Falling:
                    sourceX = FallFrame * Game1.TileSize;
                    break;
                case SpriteState.MotionType.LastMotionType:
                    break;
            }
            sourceX = spriteState.verticalFacing == SpriteState.VerticalFacing.Up ?
                sourceX + UpFrameOffset * Game1.TileSize : sourceX; 

            if (spriteState.motionType == SpriteState.MotionType.Walking)
            {
                sprites.Add(spriteState, new AnimatedSprite(Content, SpriteFilePath, sourceX, sourceY, Game1.TileSize, Game1.TileSize, WalkFps, NumWalkFrames));
            }
            else
            {
                if (spriteState.verticalFacing == SpriteState.VerticalFacing.Down)
                {
                    sourceX = spriteState.motionType == SpriteState.MotionType.Standing ?
                        BackFrame * Game1.TileSize : DownFrame * Game1.TileSize;
                }
                sprites.Add(spriteState, new Sprite(Content, SpriteFilePath, sourceX, sourceY, Game1.TileSize, Game1.TileSize));
            }
        }

        public void Update(GameTime gameTime)
        {
            sprites[SpriteState].Update(gameTime);
            jump.Update(gameTime);

            x += (int)Math.Round(velocityX * gameTime.ElapsedGameTime.TotalMilliseconds);
            velocityX += accelerationX * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (accelerationX < 0.0f)
            {
                velocityX = Math.Max(velocityX, -MaxSpeedX);
            }
            else if (accelerationX > 0.0f)
            {
                velocityX = Math.Min(velocityX, MaxSpeedX);
            }
            else if (OnGround)
            {
                velocityX *= SlowDownFactor;
            }

            y += (int)Math.Round(velocityY * gameTime.ElapsedGameTime.TotalMilliseconds);
            if (!jump.Active)
            {
                velocityY = (float)Math.Min(velocityY + Gravity * gameTime.ElapsedGameTime.TotalMilliseconds, MaxSpeedY);
            }

            if (y >= 320)
            {
                y = 320;
                velocityY = 0;
            }

            onGround = y == 320;
        }

        public void StartMovingLeft()
        {
            accelerationX = -WalkingAcceleration;
            horizontalFacing = SpriteState.HorizontalFacing.Left;
        }

        public void StartMovingRight()
        {
            accelerationX = WalkingAcceleration;
            horizontalFacing = SpriteState.HorizontalFacing.Right;
        }

        public void StopMoving()
        {
            accelerationX = 0;
        }

        public void LookUp()
        {
            verticalFacing = SpriteState.VerticalFacing.Up;
        }

        public void LookDown()
        {
            verticalFacing = SpriteState.VerticalFacing.Down;
        }

        public void LookHorizontal()
        {
            verticalFacing = SpriteState.VerticalFacing.Horizontal;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprites[SpriteState].Draw(spriteBatch, x, y);
        }

        public void StartJump()
        {
            if (OnGround)
            {
                jump.Reset();
                velocityY = -JumpSpeed;
            }
            else if (velocityY < 0.0f)
            {
                jump.Reactivate();
            }
        }

        public void StopJump()
        {
            jump.Deactivate();
        }
    }
}
