using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public struct ParticleTools
    {
        private readonly ParticleSystem frontSystem;
        private readonly ParticleSystem entitySystem;
        public ParticleSystem FrontSystem { get { return frontSystem; } }
        public ParticleSystem EntitySystem { get { return entitySystem; } }
        private readonly ContentManager content;
        public ContentManager Content { get { return content; } }

        public ParticleTools(ParticleSystem frontSystem, ParticleSystem entitySystem, ContentManager Content)
        {
            this.frontSystem = frontSystem;
            this.entitySystem = entitySystem;
            this.content = Content;
        }
    }

    public class ParticleSystem
    {
        HashSet<IParticle> particles;

        public ParticleSystem()
        {
            particles = new HashSet<IParticle>();
        }

        public void AddNewParticle(IParticle particle)
        {
            particles.Add(particle);
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < particles.Count;)
            {
                if (particles.ElementAt(i).Update(gameTime))
                {
                    i++;
                }
                else
                {
                    particles.Remove(particles.ElementAt(i++));
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (IParticle particle in particles)
            {
                particle.Draw(spriteBatch);
            }
        }
    }
}
