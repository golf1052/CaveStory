﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public struct PolarStarSpriteState
    {
        private Tuple<SpriteState.HorizontalFacing, SpriteState.VerticalFacing> tuple;
        public SpriteState.HorizontalFacing HorizontalFacing { get { return tuple.Item1; } }
        public SpriteState.VerticalFacing VerticalFacing { get { return tuple.Item2; } }

        public PolarStarSpriteState(Tuple<SpriteState.HorizontalFacing, SpriteState.VerticalFacing> tuple)
        {
            this.tuple = tuple;
        }
    }

    public class Projectile
    {
        Sprite sprite;
        GameUnit x;
        GameUnit y;
        SpriteState.HorizontalFacing horizontalDirection;
        SpriteState.VerticalFacing verticalDirection;
        GameUnit offset;

        static VelocityUnit ProjectileSpeed { get { return 0.6f; } }
        static GameUnit ProjectileMaxOffset { get { return 7 * Units.HalfTile; } }
        static GameUnit ProjectileWidth { get { return 4; } }
        GameUnit X
        {
            get
            {
                if (verticalDirection == SpriteState.VerticalFacing.Horizontal)
                {
                    if (horizontalDirection == SpriteState.HorizontalFacing.Left)
                    {
                        return x + -offset;
                    }
                    else
                    {
                        return x + offset;
                    }
                }
                return x;
            }
        }

        GameUnit Y
        {
            get
            {
                GameUnit projectileY = y;
                switch (verticalDirection)
                {
                    case SpriteState.VerticalFacing.Up:
                        y -= offset;
                        break;
                    case SpriteState.VerticalFacing.Down:
                        y += offset;
                        break;
                    default:
                        break;
                }
                return projectileY;
            }
        }

        Rectangle CollisionRectangle
        {
            get
            {
                GameUnit width = verticalDirection == SpriteState.VerticalFacing.Horizontal ? Units.TileToGame(1) : ProjectileWidth;
                GameUnit height = verticalDirection != SpriteState.VerticalFacing.Horizontal ? Units.TileToGame(1) : ProjectileWidth;
                return new Rectangle((int)Math.Round(X + Units.HalfTile - width / 2),
                    (int)Math.Round(Y + Units.HalfTile - height / 2),
                    (int)Math.Round(width), (int)Math.Round(height));
            }
        }

        public Projectile(Sprite sprite,
            SpriteState.HorizontalFacing horizontalDirection, SpriteState.VerticalFacing verticalDirection,
            GameUnit x, GameUnit y)
        {
            this.sprite = sprite;
            this.horizontalDirection = horizontalDirection;
            this.verticalDirection = verticalDirection;
            this.x = x;
            this.y = y;
            offset = 0;
        }

        /// <summary>
        /// Returns true if projectile is still alive
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns>True if projectile is still alive</returns>
        public bool Update(GameTime gameTime, Map map)
        {
            offset += (float)gameTime.ElapsedGameTime.TotalMilliseconds * ProjectileSpeed;

            List<CollisionTile> collidingTiles = map.GetCollidingTiles(CollisionRectangle);
            for (int i = 0; i < collidingTiles.Count; i++)
            {
                if (collidingTiles[i].tileType == Tile.TileType.WallTile)
                {
                    return false;
                }
            }
            return offset < ProjectileMaxOffset;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, X, Y);
        }
    }

    public class PolarStar
    {
        const string SpritePath = "Arms";
        const int PolarStarIndex = 2;
        static GameUnit GunWidth { get { return 3 * Units.HalfTile; } }
        static GameUnit GunHeight { get { return 2 * Units.HalfTile; } }

        static TileUnit HorizontalOffset { get { return 0; } }
        static TileUnit UpOffset { get { return 2; } }
        static TileUnit DownOffset { get { return 4; } }
        
        static TileUnit LeftOffset { get { return 0; } }
        static TileUnit RightOffset { get { return 1; } }

        // Gun offsets
        static GameUnit NozzleHorizontalY { get { return 23; } }
        static GameUnit NozzleHorizontalLeftX { get { return 10; } }
        static GameUnit NozzleHorizontalRightX { get { return 38; } }

        static GameUnit NozzleUpY { get { return 4; } }
        static GameUnit NozzleUpLeftX { get { return 27; } }
        static GameUnit NozzleUpRightX { get { return 21; } }

        static GameUnit NozzleDownY { get { return 28; } }
        static GameUnit NozzleDownLeftX { get { return 29; } }
        static GameUnit NozzleDownRightX { get { return 19; } }

        static TileUnit ProjectileSourceY { get { return 2; } }
        static TileUnit HorizontalProjectileSourceX { get { return 8; } }
        static TileUnit VerticalProjectileSourceX { get { return 9; } }

        Dictionary<PolarStarSpriteState, Sprite> sprites;

        Sprite horizontalProjectile;
        Sprite verticalProjectile;

        Projectile projectileA;
        Projectile projectileB;

        public PolarStar(ContentManager Content)
        {
            sprites = new Dictionary<PolarStarSpriteState, Sprite>();
            InitializeSprites(Content);
        }

        public void InitializeSprites(ContentManager Content)
        {
            horizontalProjectile = new Sprite(Content, "Bullet",
                Units.TileToPixel(HorizontalProjectileSourceX), Units.TileToPixel(ProjectileSourceY),
                Units.TileToPixel(1), Units.TileToPixel(1));
            verticalProjectile = new Sprite(Content, "Bullet",
                Units.TileToPixel(VerticalProjectileSourceX), Units.TileToPixel(ProjectileSourceY),
                Units.TileToPixel(1), Units.TileToPixel(1));
            for (SpriteState.HorizontalFacing horizontalFacing = SpriteState.HorizontalFacing.FirstHorizontalFacing;
                    horizontalFacing < SpriteState.HorizontalFacing.LastHorizontalFacing;
                    ++horizontalFacing)
            {
                for (SpriteState.VerticalFacing verticalFacing = SpriteState.VerticalFacing.FirstVerticalFacing;
                    verticalFacing < SpriteState.VerticalFacing.LastVerticalFacing;
                    ++verticalFacing)
                {
                    InitializeSprite(Content, new PolarStarSpriteState(
                        new Tuple<SpriteState.HorizontalFacing, SpriteState.VerticalFacing>(
                            horizontalFacing, verticalFacing)));
                }
            }
        }

        public void InitializeSprite(ContentManager Content, PolarStarSpriteState spriteState)
        {
            TileUnit tileY = spriteState.HorizontalFacing == SpriteState.HorizontalFacing.Left ?
                Convert.ToUInt32(LeftOffset) : Convert.ToUInt32(RightOffset);
            switch (spriteState.VerticalFacing)
            {
                case SpriteState.VerticalFacing.Horizontal:
                    tileY += HorizontalOffset;
                    break;
                case SpriteState.VerticalFacing.Up:
                    tileY += UpOffset;
                    break;
                case SpriteState.VerticalFacing.Down:
                    tileY += DownOffset;
                    break;
                case SpriteState.VerticalFacing.LastVerticalFacing:
                    break;
            }
            sprites[spriteState] = new Sprite(Content, SpritePath,
                Units.GameToPixel(PolarStarIndex * GunWidth), Units.TileToPixel(tileY),
                Units.GameToPixel(GunWidth), Units.GameToPixel(GunHeight));
        }

        public void UpdateProjectiles(GameTime gameTime, Map map)
        {
            if (projectileA != null)
            {
                if (!projectileA.Update(gameTime, map))
                {
                    projectileA = null;
                }
            }
            if (projectileB != null)
            {
                if (!projectileB.Update(gameTime, map))
                {
                    projectileB = null;
                }
            }
        }

        public void StartFire(GameUnit playerX, GameUnit playerY,
            SpriteState.HorizontalFacing horizontalFacing,
            SpriteState.VerticalFacing verticalFacing,
            bool gunUp)
        {
            if (projectileA != null && projectileB != null)
            {
                return;
            }
            GameUnit bulletX = GunX(horizontalFacing, playerX) - Units.HalfTile;
            GameUnit bulletY = GunY(verticalFacing, gunUp, playerY) - Units.HalfTile;
            switch (verticalFacing)
            {
                case SpriteState.VerticalFacing.Horizontal:
                    bulletY += NozzleHorizontalY;
                    if (horizontalFacing == SpriteState.HorizontalFacing.Left)
                    {
                        bulletX += NozzleHorizontalLeftX;
                    }
                    else
                    {
                        bulletX += NozzleHorizontalRightX;
                    }
                    break;
                case SpriteState.VerticalFacing.Up:
                    bulletY += NozzleUpY;
                    if (horizontalFacing == SpriteState.HorizontalFacing.Left)
                    {
                        bulletX += NozzleUpLeftX;
                    }
                    else
                    {
                        bulletX += NozzleUpRightX;
                    }
                    break;
                case SpriteState.VerticalFacing.Down:
                    bulletY += NozzleDownY;
                    if (horizontalFacing == SpriteState.HorizontalFacing.Left)
                    {
                        bulletX += NozzleDownLeftX;
                    }
                    else
                    {
                        bulletX += NozzleDownRightX;
                    }
                    break;
                default:
                    break;
            }

            if (projectileA == null)
            {
                projectileA = new Projectile(verticalFacing == SpriteState.VerticalFacing.Horizontal ? horizontalProjectile : verticalProjectile,
                    horizontalFacing, verticalFacing, bulletX, bulletY);
            }
            else if (projectileB == null)
            {
                projectileB = new Projectile(verticalFacing == SpriteState.VerticalFacing.Horizontal ? horizontalProjectile : verticalProjectile,
                    horizontalFacing, verticalFacing, bulletX, bulletY);
            }
        }

        public void StopFire()
        {

        }

        private GameUnit GunX(SpriteState.HorizontalFacing horizontalFacing, GameUnit playerX)
        {
            if (horizontalFacing == SpriteState.HorizontalFacing.Left)
            {
                return playerX - Units.HalfTile;
            }
            else
            {
                return playerX;
            }
        }

        private GameUnit GunY(SpriteState.VerticalFacing verticalFacing, bool gunUp, GameUnit playerY)
        {
            GameUnit gunY = playerY;
            if (verticalFacing == SpriteState.VerticalFacing.Up)
            {
                gunY -= Units.HalfTile / 2;
            }
            else if (verticalFacing == SpriteState.VerticalFacing.Down)
            {
                gunY += Units.HalfTile / 2;
            }
            if (gunUp)
            {
                gunY -= 2;
            }
            return gunY;
        }

        public void Draw(SpriteBatch spriteBatch,
            SpriteState.HorizontalFacing horizontalFacing, SpriteState.VerticalFacing verticalFacing,
            bool gunUp,
            GameUnit playerX, GameUnit playerY)
        {
            GameUnit x = GunX(horizontalFacing, playerX);
            GameUnit y = GunY(verticalFacing, gunUp, playerY);
            
            sprites[new PolarStarSpriteState(new Tuple<SpriteState.HorizontalFacing, SpriteState.VerticalFacing>(
                horizontalFacing, verticalFacing))].Draw(spriteBatch, x, y);
            if (projectileA != null)
            {
                projectileA.Draw(spriteBatch);
            }
            if (projectileB != null)
            {
                projectileB.Draw(spriteBatch);
            }
        }
    }
}
