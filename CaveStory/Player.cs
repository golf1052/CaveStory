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

        Sprite sprite;
        int x;
        int y;
        float velocityX;
        float accelerationX;

        public Player(ContentManager Content, int x, int y)
        {
            sprite = new AnimatedSprite(Content, "MyChar", 0, 0, Game1.TileSize, Game1.TileSize, 15, 3);
            this.x = x;
            this.y = y;
            velocityX = 0;
            accelerationX = 0;
        }

        public void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
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
        }

        public void StartMovingRight()
        {
            accelerationX = WalkingAcceleration;
        }

        public void StopMoving()
        {
            accelerationX = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, x, y);
        }
    }
}
