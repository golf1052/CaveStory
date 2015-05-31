using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CaveStory
{
    public class PowerDoritoPickup : IPickup
    {
        public enum SizeType
        {
            Small,
            Medium,
            Large
        }

        static GunExperienceUnit[] Values { get { return new GunExperienceUnit[] { 1, 5, 20 }; } }
        const string SpriteName = "Npc\\NpcSym";
        static TileUnit SourceX { get { return 0; } }
        static TileUnit[] SourceYs { get { return new TileUnit[] { 1, 2, 3 }; } }
        static TileUnit SourceWidth = 1;
        static TileUnit SourceHeight = 1;
        const int Fps = 14;
        static FrameUnit NumFrames { get { return 6; } }

        public override PickupType Type { get { return PickupType.Experience; } }

        public override int Value
        {
            get
            {
                return Values[(int)size];
            }
        }

        Kinematics kinematicsX;
        Kinematics kinematicsY;
        AnimatedSprite sprite;
        SizeType size;

        public PowerDoritoPickup(ContentManager Content, GameUnit centerX, GameUnit centerY, SizeType size)
        {
            kinematicsX = new Kinematics(centerX - Units.HalfTile, (Game1.Random.Next(0, 11) - 5) * 0.025f);
            kinematicsY = new Kinematics(centerY - Units.HalfTile, (Game1.Random.Next(0, 11) - 5) * 0.025f);
            sprite = new AnimatedSprite(Content, SpriteName,
                Units.TileToPixel(SourceX), Units.TileToPixel(SourceYs[(int)size]),
                Units.TileToPixel(SourceWidth), Units.TileToPixel(SourceHeight),
                Fps, NumFrames);
            this.size = size;
        }

        public override Rectangle CollisionRectangle()
        {
            throw new NotImplementedException();
        }

        public override bool Update(GameTime gameTime, Map map)
        {
            sprite.Update();
            return true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, kinematicsX.position, kinematicsY.position);
        }
    }
}
