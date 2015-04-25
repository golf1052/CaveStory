using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public class PlayerHealth
    {
        // HUD constants
        static GameUnit HealthBarX { get { return Units.TileToGame(1); } }
        static GameUnit HealthBarY { get { return Units.TileToGame(2); } }
        static GameUnit HealthBarSourceX { get { return 0; } }
        static GameUnit HealthBarSourceY { get { return 5 * Units.HalfTile; } }
        static GameUnit HealthBarSourceWidth { get { return Units.TileToGame(4); } }
        static GameUnit HealthBarSourceHeight { get { return Units.HalfTile; } }

        static GameUnit HealthFillX { get { return 5 * Units.HalfTile; } }
        static GameUnit HealthFillY { get { return Units.TileToGame(2); } }
        public static GameUnit MaxFillWidth { get { return 5 * Units.HalfTile - 2; } }
        static GameUnit HealthDamageSourceX { get { return 0; } }
        static GameUnit HealthDamageSourceY { get { return Units.TileToGame(2); } }
        static GameUnit HealthDamageSourceHeight { get { return Units.HalfTile; } }

        static GameUnit HealthFillSourceX { get { return 0; } }
        static GameUnit HealthFillSourceY { get { return 3 * Units.HalfTile; } }
        static GameUnit HealthFillSourceHeight { get { return Units.HalfTile; } }

        static GameUnit HealthNumberX { get { return Units.TileToGame(3) / 2; } }
        static GameUnit HealthNumberY { get { return Units.TileToGame(2); } }
        const int HealthNumberNumDigits = 2;
        const string SpritePath = "TextBox";
        static TimeSpan DamageDelay { get { return TimeSpan.FromMilliseconds(1500); } }
        public Health health;

        public PlayerHealth(ContentManager Content)
        {
            health = new Health(Content);
            health.healthBarSprite = new Sprite(Content, SpritePath,
                Units.GameToPixel(HealthBarSourceX), Units.GameToPixel(HealthBarSourceY),
                Units.GameToPixel(HealthBarSourceWidth), Units.GameToPixel(HealthBarSourceHeight));

            health.healthFillSprite = new VaryingWidthSprite(Content, SpritePath,
                Units.GameToPixel(HealthFillSourceX), Units.GameToPixel(HealthFillSourceY),
                Units.GameToPixel(MaxFillWidth), Units.GameToPixel(HealthFillSourceHeight));

            health.damageFillSprite = new VaryingWidthSprite(Content, SpritePath,
                Units.GameToPixel(HealthDamageSourceX), Units.GameToPixel(HealthDamageSourceY),
                Units.GameToPixel(0), Units.GameToPixel(HealthDamageSourceHeight));
            health.healthNumberSprite = new NumberSprite(Content, 0, HealthNumberNumDigits);
        }

        public void Update(GameTime gameTime)
        {
            if (health.damage > 0)
            {
                health.damageTime += gameTime.ElapsedGameTime;
                if (health.damageTime > DamageDelay)
                {
                    health.currentHealth -= health.damage;
                    health.damage = 0;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            health.healthBarSprite.Draw(spriteBatch, HealthBarX, HealthBarY);
            health.healthFillSprite.Draw(spriteBatch, HealthFillX, HealthFillY);
            if (health.damage > 0)
            {
                health.damageFillSprite.Draw(spriteBatch, HealthFillX + health.FillOffset(health.currentHealth - health.damage), HealthFillY);
            }
            health.healthNumberSprite.number = health.currentHealth;
            health.healthNumberSprite.Draw(spriteBatch, HealthNumberX, HealthNumberY);
        }
    }
}
