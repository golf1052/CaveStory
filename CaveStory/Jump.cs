using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public class Jump
    {
        private TimeSpan timeRemaining;
        private bool active;
        public bool Active
        {
            get
            {
                return active;
            }
        }

        public Jump()
        {
            timeRemaining = TimeSpan.Zero;
            active = false;
        }

        public void Update(GameTime gameTime)
        {
            if (active)
            {
                timeRemaining -= gameTime.ElapsedGameTime;
                if (timeRemaining <= TimeSpan.Zero)
                {
                    active = false;
                }
            }
        }

        public void Reset()
        {
            timeRemaining = Player.JumpTime;
            Reactivate();
        }

        public void Reactivate()
        {
            active = timeRemaining > TimeSpan.Zero;
        }

        public void Deactivate()
        {
            active = false;
        }
    }
}
