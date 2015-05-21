using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public class GunExperienceHud
    {
        static GameUnit DrawY { get { return 3 * Units.HalfTile; } }
        static GameUnit LevelNumberDrawX { get { return 3 * Units.HalfTile; } }

        static TileUnit LevelDrawX { get { return 1; } }
        static TileUnit LevelSourceX { get { return 5; } }
        static GameUnit LevelSourceY { get { return 160; } }
        static TileUnit LevelSourceWidth { get { return 1; } }
        static GameUnit LevelSourceHeight { get { return Units.HalfTile; } }

        static GameUnit ExperienceBarDrawX { get { return 5 * Units.HalfTile; } }
        static GameUnit ExperienceBarSourceX { get { return 0; } }
        static GameUnit ExperienceBarSourceY { get { return 9 * Units.HalfTile; } }
        static GameUnit ExperienceBarSourceWidth { get { return 5 * Units.HalfTile; } }
        static GameUnit ExperienceBarSourceHeight { get { return Units.HalfTile; } }

        const string SpriteName = "TextBox";

        Sprite experienceBarSprite;
        Sprite levelSprite;

        NumberSprite number;

        public GunExperienceHud(ContentManager Content)
        {
            experienceBarSprite = new Sprite(Content, SpriteName,
                Units.GameToPixel(ExperienceBarSourceX), Units.GameToPixel(ExperienceBarSourceY),
                Units.GameToPixel(ExperienceBarSourceWidth), Units.GameToPixel(ExperienceBarSourceHeight));
            levelSprite = new Sprite(Content, SpriteName,
                Units.TileToPixel(LevelSourceX), Units.GameToPixel(LevelSourceY),
                Units.TileToPixel(LevelSourceWidth), Units.GameToPixel(LevelSourceHeight));
            number = NumberSprite.HudNumber(Content, 0, 2);
        }

        public void Draw(SpriteBatch spriteBatch, GunLevelUnit gunLevel)
        {
            levelSprite.Draw(spriteBatch,
                Units.TileToGame(LevelDrawX),
                DrawY);
            number.number = Convert.ToInt32(gunLevel);
            number.LoadNumber();
            number.Draw(spriteBatch, LevelNumberDrawX, DrawY);
            experienceBarSprite.Draw(spriteBatch,
                ExperienceBarDrawX, DrawY);
        }
    }
}
