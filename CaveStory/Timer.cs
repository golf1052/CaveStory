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
        private TimeSpan currentTime;

        // Assumes the user knows if this is active or not
        public TimeSpan CurrentTime { get { return currentTime; } }
        public TimeSpan expirationTime;
        public bool Active { get { return currentTime < expirationTime; } }
        public bool Expired { get { return !Active; } }
        
        private long classInstanceId;
        //static HashSet<Timer> timers;

        public Timer(TimeSpan expirationTime)
        {
            bool known;
            classInstanceId = Game1.objectIdGen.GetId(this, out known);
            currentTime = expirationTime;
            this.expirationTime = expirationTime;
            Game1.Timers.Add(this);
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
            foreach (Timer timer in Game1.Timers)
            {
                timer.Update(gameTime);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else
            {
                Timer objTimer = obj as Timer;
                if (objTimer == null)
                {
                    return false;
                }
                return CurrentTime == objTimer.CurrentTime && expirationTime == objTimer.expirationTime &&
                    Active == objTimer.Active && Expired == objTimer.Expired;
            }
        }

        public override int GetHashCode()
        {
            return Convert.ToInt32(classInstanceId);
        }
    }
}
