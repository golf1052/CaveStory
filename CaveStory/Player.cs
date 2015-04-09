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
        const float SlowDownFactor = 0.8f;
        const float WalkingAcceleration = 0.0012f; // (pixels / millisecond) / millisecond
        const float MaxSpeedX = 0.325f; // pixels / millisecond

        const float Gravity = 0.0012f; // (pixels / millisecond) / millisecond

        const float JumpSpeed = 0.325f; // pixels / millisecond
        const float MaxSpeedY = 0.325f; // pixels / millisecond
        public static TimeSpan JumpTime = TimeSpan.FromMilliseconds(275);

        int x;
        int y;
        float velocityX;
        float velocityY;
        float accelerationX;
        SpriteState.HorizontalFacing horizontalFacing;
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
                return new SpriteState(accelerationX == 0.0f ? SpriteState.MotionType.Standing : SpriteState.MotionType.Walking,
                    horizontalFacing);
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
            onGround = false;
            jump = new Jump();
        }

        public void InitializeSprites(ContentManager Content)
        {
            sprites.Add(new SpriteState(SpriteState.MotionType.Standing, SpriteState.HorizontalFacing.Left),
                new Sprite(Content, "MyChar", 0, 0, Game1.TileSize, Game1.TileSize));
            sprites.Add(new SpriteState(SpriteState.MotionType.Walking, SpriteState.HorizontalFacing.Left),
                new AnimatedSprite(Content, "MyChar", 0, 0, Game1.TileSize, Game1.TileSize, 15, 3));

            sprites.Add(new SpriteState(SpriteState.MotionType.Standing, SpriteState.HorizontalFacing.Right),
                new Sprite(Content, "MyChar", 0, Game1.TileSize, Game1.TileSize, Game1.TileSize));
            sprites.Add(new SpriteState(SpriteState.MotionType.Walking, SpriteState.HorizontalFacing.Right),
                new AnimatedSprite(Content, "MyChar", 0, Game1.TileSize, Game1.TileSize, Game1.TileSize, 15, 3));
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
