using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CaveStory
{
    public class Timer
    {
        static HashSet<Timer> timers = new HashSet<Timer>();
        private TimeSpan currentTime;

        // Assumes the user knows if this is active or not
        public TimeSpan CurrentTime { get { return currentTime; } }
        private TimeSpan expirationTime;
        public bool Active { get { return currentTime < expirationTime; } }
        public bool Expired { get { return !Active; } }
        
        private long classInstanceId;

        public Timer(TimeSpan expirationTime, bool startActive = false)
        {
            bool known;
            classInstanceId = Game1.objectIdGen.GetId(this, out known);
            currentTime = startActive ? TimeSpan.Zero : expirationTime;
            this.expirationTime = expirationTime;
            Timer.timers.Add(this);
        }

        public void Reset()
        {
            currentTime = TimeSpan.Zero;
        }

        void Update(GameTime gameTime)
        {
            if (Active)
            {
                currentTime += gameTime.ElapsedGameTime;
            }
        }

        public static void UpdateAll(GameTime gameTime)
        {
            foreach (Timer timer in Timer.timers)
            {
                timer.Update(gameTime);
            }
        }

        public override int GetHashCode()
        {
            return Convert.ToInt32(classInstanceId);
        }
    }
}
