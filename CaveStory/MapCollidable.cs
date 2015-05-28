using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CaveStory
{
    public abstract class MapCollidable
    {
        public enum SideType
        {
            TopSide,
            BottomSide,
            LeftSide,
            RightSide
        }

        CollisionInfo GetWallCollisionInfo(Map map, Rectangle rectangle)
        {
            CollisionInfo info = new CollisionInfo(false, 0, 0);
            List<CollisionTile> tiles = map.GetCollidingTiles(rectangle);
            for (int i = 0; i < tiles.Count; i++)
            {
                if (tiles[i].tileType == Tile.TileType.WallTile)
                {
                    info = new CollisionInfo(true, tiles[i].row, tiles[i].col);
                    break;
                }
            }
            return info;
        }

        protected void UpdateX(CollisionRectangle collisionRectangle,
            Kinematics kinematicsX, Kinematics kinematicsY,
            GameTime gameTime, Map map)
        {
            GameUnit delta = kinematicsX.velocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (delta > 0.0f)
            {
                CollisionInfo info = GetWallCollisionInfo(map, collisionRectangle.RightCollision(kinematicsX.position, kinematicsY.position, delta));

                if (info.collided)
                {
                    kinematicsX.position = Units.TileToGame(info.col) - collisionRectangle.BoundingBox.Right;
                    OnCollision(SideType.RightSide, true);
                }
                else
                {
                    kinematicsX.position += delta;
                    OnDelta(SideType.RightSide);
                }

                info = GetWallCollisionInfo(map, collisionRectangle.LeftCollision(kinematicsX.position, kinematicsY.position, 0));

                if (info.collided)
                {
                    kinematicsX.position = Units.TileToGame(info.col + 1) - collisionRectangle.BoundingBox.Left;
                    OnCollision(SideType.LeftSide, false);
                }
            }
            else
            {
                CollisionInfo info = GetWallCollisionInfo(map, collisionRectangle.LeftCollision(kinematicsX.position, kinematicsY.position, delta));

                if (info.collided)
                {
                    kinematicsX.position = Units.TileToGame(info.col + 1) - collisionRectangle.BoundingBox.Left;
                    OnCollision(SideType.LeftSide, true);
                }
                else
                {
                    kinematicsX.position += delta;
                    OnDelta(SideType.LeftSide);
                }

                info = GetWallCollisionInfo(map, collisionRectangle.RightCollision(kinematicsX.position, kinematicsY.position, 0));

                if (info.collided)
                {
                    kinematicsX.position = Units.TileToGame(info.col) - collisionRectangle.BoundingBox.Right;
                    OnCollision(SideType.RightSide, false);
                }
            }
        }

        protected void UpdateY(CollisionRectangle collisionRectangle,
            Kinematics kinematicsX, Kinematics kinematicsY,
            GameTime gameTime, Map map)
        {
            GameUnit delta = kinematicsY.velocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (delta > 0)
            {
                CollisionInfo info = GetWallCollisionInfo(map, collisionRectangle.BottomCollision(kinematicsX.position, kinematicsY.position, delta));

                if (info.collided)
                {
                    kinematicsY.position = Units.TileToGame(info.row) - collisionRectangle.BoundingBox.Bottom;
                    OnCollision(SideType.BottomSide, true);
                }
                else
                {
                    kinematicsY.position += delta;
                    OnDelta(SideType.BottomSide);
                }

                info = GetWallCollisionInfo(map, collisionRectangle.TopCollision(kinematicsX.position, kinematicsY.position, 0));

                if (info.collided)
                {
                    kinematicsY.position = Units.TileToGame(info.row + 1) - collisionRectangle.BoundingBox.Top;
                    OnCollision(SideType.TopSide, false);
                }
            }
            else
            {
                CollisionInfo info = GetWallCollisionInfo(map, collisionRectangle.TopCollision(kinematicsX.position, kinematicsY.position, delta));

                if (info.collided)
                {
                    kinematicsY.position = Units.TileToGame(info.row + 1) - collisionRectangle.BoundingBox.Top;
                    OnCollision(SideType.TopSide, true);
                }
                else
                {
                    kinematicsY.position += delta;
                    OnDelta(SideType.TopSide);
                }

                info = GetWallCollisionInfo(map, collisionRectangle.BottomCollision(kinematicsX.position, kinematicsY.position, 0));

                if (info.collided)
                {
                    kinematicsY.position = Units.TileToGame(info.row) - collisionRectangle.BoundingBox.Bottom;
                    OnCollision(SideType.BottomSide, false);
                }
            }
        }

        protected abstract void OnCollision(SideType side, bool isDeltaDirection);

        protected abstract void OnDelta(SideType side);
    }
}
