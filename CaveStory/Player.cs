using Microsoft.Xna.Framework;
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

        public Player(int x, int y, Sprite sprite)
        {
            this.x = x;
            this.y = y;
            this.sprite = sprite;
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
