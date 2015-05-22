using Microsoft.Xna.Framework;
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

        static GameUnit FlashSourceX { get { return 5 * Units.HalfTile; } }
        static TileUnit FlashSourceY { get { return 5; } }
        static TimeSpan FlashTime { get { return TimeSpan.FromMilliseconds(800); } }
        static TimeSpan FlashPeriod { get { return TimeSpan.FromMilliseconds(40); } }

        static TileUnit FillSourceX { get { return 0; } }
        static TileUnit FillSourceY { get { return 5; } }
        
        static GameUnit MaxSourceX { get { return 5 * Units.HalfTile; } }
        static GameUnit MaxSourceY { get { return 9 * Units.HalfTile; } }

        const string SpriteName = "TextBox";

        Sprite experienceBarSprite;
        Sprite levelSprite;
        Sprite flashSprite;
        VaryingWidthSprite fillSprite;
        Sprite maxSprite;
        Timer flashTimer;

        NumberSprite number;

        public GunExperienceHud(ContentManager Content)
        {
            experienceBarSprite = new Sprite(Content, SpriteName,
                Units.GameToPixel(ExperienceBarSourceX), Units.GameToPixel(ExperienceBarSourceY),
                Units.GameToPixel(ExperienceBarSourceWidth), Units.GameToPixel(ExperienceBarSourceHeight));
            levelSprite = new Sprite(Content, SpriteName,
                Units.TileToPixel(LevelSourceX), Units.GameToPixel(LevelSourceY),
                Units.TileToPixel(LevelSourceWidth), Units.GameToPixel(LevelSourceHeight));
            flashSprite = new Sprite(Content, SpriteName,
                Units.GameToPixel(FlashSourceX), Units.TileToPixel(FlashSourceY),
                Units.GameToPixel(ExperienceBarSourceWidth), Units.GameToPixel(ExperienceBarSourceHeight));
            fillSprite = new VaryingWidthSprite(Content, SpriteName,
                Units.TileToPixel(FillSourceX), Units.TileToPixel(FillSourceY),
                Units.GameToPixel(ExperienceBarSourceWidth), 0,
                Units.GameToPixel(ExperienceBarSourceHeight));
            maxSprite = new Sprite(Content, SpriteName,
                Units.GameToPixel(MaxSourceX), Units.GameToPixel(MaxSourceY),
                Units.GameToPixel(ExperienceBarSourceWidth), Units.GameToPixel(ExperienceBarSourceHeight));
            flashTimer = new Timer(FlashTime);
            number = NumberSprite.HudNumber(Content, 0, 2);
        }

        public void ActivateFlash()
        {
            flashTimer.Reset();
        }

        public void Draw(SpriteBatch spriteBatch, GunLevelUnit gunLevel,
            GunExperienceUnit currentExperience, GunExperienceUnit levelExpereince)
        {
            levelSprite.Draw(spriteBatch,
                Units.TileToGame(LevelDrawX),
                DrawY);
            number.number = Convert.ToInt32(gunLevel);
            number.LoadNumber();
            number.Draw(spriteBatch, LevelNumberDrawX, DrawY);
            experienceBarSprite.Draw(spriteBatch,
                ExperienceBarDrawX, DrawY);

            if (currentExperience < levelExpereince)
            {
                fillSprite.SetPercentageWidth((float)currentExperience / (float)levelExpereince);
                fillSprite.Draw(spriteBatch, ExperienceBarDrawX, DrawY);
            }
            else
            {
                maxSprite.Draw(spriteBatch, ExperienceBarDrawX, DrawY);
            }

            if (flashTimer.Active && flashTimer.CurrentTime.Ticks / FlashPeriod.Ticks % 2 == 0)
            {
                flashSprite.Draw(spriteBatch, ExperienceBarDrawX, DrawY);
            }
        }
    }
}
