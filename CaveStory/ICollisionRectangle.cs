using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CaveStory
{
    public interface ICollisionRectangle
    {
        Rectangle BoundingBox { get; }

        Rectangle LeftCollision(GameUnit x, GameUnit y, GameUnit delta);

        Rectangle RightCollision(GameUnit x, GameUnit y, GameUnit delta);

        Rectangle TopCollision(GameUnit x, GameUnit y, GameUnit delta);

        Rectangle BottomCollision(GameUnit x, GameUnit y, GameUnit delta);
    }
}
