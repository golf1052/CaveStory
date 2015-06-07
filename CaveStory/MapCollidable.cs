using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CaveStory
{
    public abstract class MapCollidable
    {
        GameUnit? TestMapCollision(Map map, Rectangle rectangle, TileInfo.SideType direction)
        {
            List<CollisionTile> tiles = map.GetCollidingTiles(rectangle);
            for (int i = 0; i < tiles.Count; i++)
            {
                TileInfo.SideType side = TileInfo.OppositeSide(direction);
                GameUnit position;
                if (TileInfo.Vertical(side))
                {
                    position = rectangle.Center.X;
                }
                else
                {
                    position = rectangle.Center.Y;
                }
                GameUnit? maybePosition = tiles[i].TestCollision(side, position);
                if (maybePosition.HasValue)
                {
                    return maybePosition.Value;
                }
            }
            return null;
        }

        protected void UpdateX(ICollisionRectangle collisionRectangle,
            IAccelerator accelerator,
            Kinematics kinematicsX, Kinematics kinematicsY,
            GameTime gameTime, Map map)
        {
            accelerator.UpdateVelocity(kinematicsX, gameTime);
            GameUnit delta = kinematicsX.velocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (delta > 0.0f)
            {
                TileInfo.SideType direction = TileInfo.SideType.RightSide;
                GameUnit? maybePosition = TestMapCollision(map,
                    collisionRectangle.RightCollision(kinematicsX.position, kinematicsY.position, delta),
                    direction);

                if (maybePosition.HasValue)
                {
                    kinematicsX.position = maybePosition.Value - collisionRectangle.BoundingBox.Right;
                    OnCollision(direction, true);
                }
                else
                {
                    kinematicsX.position += delta;
                    OnDelta(direction);
                }

                TileInfo.SideType oppositeDirection = TileInfo.OppositeSide(direction);
                maybePosition = TestMapCollision(map,
                    collisionRectangle.LeftCollision(kinematicsX.position, kinematicsY.position, 0),
                    oppositeDirection);

                if (maybePosition.HasValue)
                {
                    kinematicsX.position = maybePosition.Value - collisionRectangle.BoundingBox.Left;
                    OnCollision(oppositeDirection, false);
                }
            }
            else
            {
                TileInfo.SideType direction = TileInfo.SideType.LeftSide;
                GameUnit? maybePosition = TestMapCollision(map,
                    collisionRectangle.LeftCollision(kinematicsX.position, kinematicsY.position, delta),
                    direction);

                if (maybePosition.HasValue)
                {
                    kinematicsX.position = maybePosition.Value - collisionRectangle.BoundingBox.Left;
                    OnCollision(direction, true);
                }
                else
                {
                    kinematicsX.position += delta;
                    OnDelta(direction);
                }

                TileInfo.SideType oppositeDirection = TileInfo.OppositeSide(direction);
                maybePosition = TestMapCollision(map,
                    collisionRectangle.RightCollision(kinematicsX.position, kinematicsY.position, 0),
                    oppositeDirection);

                if (maybePosition.HasValue)
                {
                    kinematicsX.position = maybePosition.Value - collisionRectangle.BoundingBox.Right;
                    OnCollision(oppositeDirection, false);
                }
            }
        }

        protected void UpdateY(ICollisionRectangle collisionRectangle,
            IAccelerator accelerator,
            Kinematics kinematicsX, Kinematics kinematicsY,
            GameTime gameTime, Map map)
        {
            accelerator.UpdateVelocity(kinematicsY, gameTime);
            GameUnit delta = kinematicsY.velocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (delta > 0)
            {
                TileInfo.SideType direction = TileInfo.SideType.BottomSide;
                GameUnit? maybePosition = TestMapCollision(map,
                    collisionRectangle.BottomCollision(kinematicsX.position, kinematicsY.position, delta),
                    direction);

                if (maybePosition.HasValue)
                {
                    kinematicsY.position = maybePosition.Value - collisionRectangle.BoundingBox.Bottom;
                    OnCollision(direction, true);
                }
                else
                {
                    kinematicsY.position += delta;
                    OnDelta(direction);
                }

                TileInfo.SideType oppositeDirection = TileInfo.OppositeSide(direction);
                maybePosition = TestMapCollision(map,
                    collisionRectangle.TopCollision(kinematicsX.position, kinematicsY.position, 0),
                    oppositeDirection);

                if (maybePosition.HasValue)
                {
                    kinematicsY.position = maybePosition.Value - collisionRectangle.BoundingBox.Top;
                    OnCollision(oppositeDirection, false);
                }
            }
            else
            {
                TileInfo.SideType direction = TileInfo.SideType.TopSide;
                GameUnit? maybePosition = TestMapCollision(map,
                    collisionRectangle.TopCollision(kinematicsX.position, kinematicsY.position, delta),
                    direction);

                if (maybePosition.HasValue)
                {
                    kinematicsY.position = maybePosition.Value - collisionRectangle.BoundingBox.Top;
                    OnCollision(direction, true);
                }
                else
                {
                    kinematicsY.position += delta;
                    OnDelta(direction);
                }

                TileInfo.SideType oppositeDirection = TileInfo.OppositeSide(direction);
                maybePosition = TestMapCollision(map,
                    collisionRectangle.BottomCollision(kinematicsX.position, kinematicsY.position, 0),
                    oppositeDirection);

                if (maybePosition.HasValue)
                {
                    kinematicsY.position = maybePosition.Value - collisionRectangle.BoundingBox.Bottom;
                    OnCollision(oppositeDirection, false);
                }
            }
        }

        protected abstract void OnCollision(TileInfo.SideType side, bool isDeltaDirection);

        protected abstract void OnDelta(TileInfo.SideType side);
    }
}
