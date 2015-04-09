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

        int x;
        int y;
        float velocityX;
        float accelerationX;
        SpriteState.HorizontalFacing horizontalFacing;

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
            accelerationX = 0;
            horizontalFacing = SpriteState.HorizontalFacing.Left;
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
            else
            {
                velocityX *= SlowDownFactor;
            }
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
    }
}
