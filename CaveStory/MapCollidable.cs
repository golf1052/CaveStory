using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CaveStory
{
    public abstract class MapCollidable
    {
        public enum AxisType
        {
            XAxis,
            YAxis
        }

        GameUnit? TestMapCollision(Map map, Rectangle rectangle, TileInfo.SideType direction)
        {
            List<CollisionTile> tiles = map.GetCollidingTiles(rectangle, direction);
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

        private void Update(ICollisionRectangle collisionRectangle,
            IAccelerator accelerator,
            Kinematics kinematicsX, Kinematics kinematicsY,
            GameTime gameTime, Map map,
            Kinematics kinematics, AxisType axis)
        {
            accelerator.UpdateVelocity(kinematics, gameTime);
            GameUnit delta = kinematics.velocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            TileInfo.SideType direction = axis == AxisType.XAxis ?
                (delta > 0 ? TileInfo.SideType.RightSide : TileInfo.SideType.LeftSide) :
                (delta > 0 ? TileInfo.SideType.BottomSide : TileInfo.SideType.TopSide);
            GameUnit? maybePosition = TestMapCollision(map,
                collisionRectangle.Collision(direction, kinematicsX.position, kinematicsY.position, delta),
                direction);

            if (maybePosition.HasValue)
            {
                kinematics.position = maybePosition.Value - collisionRectangle.BoundingBox.Side(direction);
                OnCollision(direction, true);
            }
            else
            {
                kinematics.position += delta;
                OnDelta(direction);
            }

            TileInfo.SideType oppositeDirection = TileInfo.OppositeSide(direction);
            maybePosition = TestMapCollision(map,
                collisionRectangle.Collision(oppositeDirection, kinematicsX.position, kinematicsY.position, 0),
                oppositeDirection);

            if (maybePosition.HasValue)
            {
                kinematics.position = maybePosition.Value - collisionRectangle.BoundingBox.Side(oppositeDirection);
                OnCollision(oppositeDirection, false);
            }
        }

        protected void UpdateX(ICollisionRectangle collisionRectangle,
            IAccelerator accelerator,
            Kinematics kinematicsX, Kinematics kinematicsY,
            GameTime gameTime, Map map)
        {
            Update(collisionRectangle, accelerator,
                kinematicsX, kinematicsY,
                gameTime, map,
                kinematicsX, AxisType.XAxis);
        }

        protected void UpdateY(ICollisionRectangle collisionRectangle,
            IAccelerator accelerator,
            Kinematics kinematicsX, Kinematics kinematicsY,
            GameTime gameTime, Map map)
        {
            Update(collisionRectangle, accelerator,
                kinematicsX, kinematicsY,
                gameTime, map,
                kinematicsY, AxisType.YAxis);
        }

        protected abstract void OnCollision(TileInfo.SideType side, bool isDeltaDirection);

        protected abstract void OnDelta(TileInfo.SideType side);
    }
}
