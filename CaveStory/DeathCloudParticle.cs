using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CaveStory
{
    public class DeathCloudParticle : IParticle
    {
        const string SpriteName = "Npc\\NpcSym";
        static TileUnit SourceX { get { return 1; } }
        static TileUnit SourceY { get { return 0; } }
        static TileUnit SourceWidth { get { return 1; } }
        static TileUnit SourceHeight { get { return 1; } }
        const int Fps = 18;
        static FrameUnit NumFrames { get { return 7; } }
        static VelocityUnit BaseVelocity { get { return 0.12f; } }

        GameUnit centerX;
        GameUnit centerY;
        VelocityUnit speed;
        PolarVector offset;
        AnimatedSprite sprite;

        public DeathCloudParticle(ContentManager Content,
            GameUnit centerX, GameUnit centerY,
            VelocityUnit speed, DegreesUnit angle)
        {
            this.centerX = centerX - Units.HalfTile;
            this.centerY = centerY - Units.HalfTile;
            this.speed = speed;
            offset = new PolarVector(0, angle);
            sprite = new AnimatedSprite(Content, SpriteName,
                Units.TileToPixel(SourceX), Units.TileToPixel(SourceY),
                Units.TileToPixel(SourceWidth), Units.TileToPixel(SourceHeight),
                Fps, NumFrames);
        }

        public static void CreateRandomDeathCloud(ParticleTools particleTools,
            GameUnit centerX, GameUnit centerY,
            int numParticles)
        {
            for (int i = 0; i < numParticles; i++)
            {
                particleTools.EntitySystem.AddNewParticle(new DeathCloudParticle(particleTools.Content,
                    centerX, centerY,
                    Game1.Random.Next(0, 3) * BaseVelocity, Game1.Random.Next(0, 360)));
            }
        }

        public bool Update(GameTime gameTime)
        {
            sprite.Update();
            offset.magnitude += speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            return sprite.NumCompletedLoops == 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, centerX + offset.X, centerY + offset.Y);
        }
    }
}
