using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace CaveStory
{
    public class ProjectileWallParticle : ImmobileSingleLoopParticle
    {
        const string SpriteName = "Caret";
        static TileUnit SourceX { get { return 11; } }
        static TileUnit SourceY { get { return 0; } }
        static TileUnit SourceWidth { get { return 1; } }
        static TileUnit SourceHeight { get { return 1; } }
        const int Fps = 20;

        static FrameUnit NumFrames { get { return 4; } }
        public ProjectileWallParticle(ContentManager Content, string spriteName,
            PixelUnit sourceX, PixelUnit sourceY,
            PixelUnit sourceWidth, PixelUnit sourceHeight,
            int fps, FrameUnit numFrames,
            GameUnit x, GameUnit y)
            : base(Content, spriteName,
                  sourceX, sourceY,
                  sourceWidth, sourceHeight,
                  fps, numFrames,
                  x, y)
        {
        }

        public static ProjectileWallParticle Create(ContentManager Content, GameUnit x, GameUnit y)
        {
            return new ProjectileWallParticle(Content, SpriteName,
                Units.TileToPixel(SourceX), Units.TileToPixel(SourceY),
                Units.TileToPixel(SourceWidth), Units.TileToPixel(SourceHeight),
                Fps, NumFrames,
                x, y);
        }
    }
}
