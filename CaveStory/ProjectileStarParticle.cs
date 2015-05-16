using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CaveStory
{
    public class ProjectileStarParticle : IParticle
    {
        const string SpriteName = "Caret";
        static TileUnit SourceX { get { return 0; } }
        static TileUnit SourceY { get { return 3; } }
        static TileUnit SourceWidth { get { return 1; } }
        static TileUnit SourceHeight { get { return 1; } }
        const int Fps = 18;
        static FrameUnit NumFrames { get { return 4; } }


        GameUnit x;
        GameUnit y;
        AnimatedSprite sprite;

        public ProjectileStarParticle(ContentManager Content, GameUnit x, GameUnit y)
        {
            this.x = x;
            this.y = y;
            sprite = new AnimatedSprite(Content, SpriteName,
                Units.TileToPixel(SourceX), Units.TileToPixel(SourceY),
                Units.TileToPixel(SourceWidth), Units.TileToPixel(SourceHeight),
                Fps, NumFrames);
        }

        public bool Update(GameTime gameTime)
        {
            sprite.Update();
            return sprite.NumCompletedLoops == 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, x, y);
        }
    }
}
