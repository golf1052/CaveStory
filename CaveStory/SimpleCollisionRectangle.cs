using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CaveStory
{
    public class SimpleCollisionRectangle : ICollisionRectangle
    {
        Rectangle rectangle;
        const int ExtraOffset = 1;

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle(rectangle.Left - ExtraOffset,
                    rectangle.Top - ExtraOffset,
                    rectangle.Width + 2 * ExtraOffset,
                    rectangle.Height + 2 * ExtraOffset);
            }
        }

        public SimpleCollisionRectangle(Rectangle rectangle)
        {
            this.rectangle = rectangle;
        }

        public Rectangle LeftCollision(GameUnit x, GameUnit y, GameUnit delta)
        {
            return new Rectangle((int)Math.Round(x + rectangle.Left + delta),
                (int)Math.Round(y + rectangle.Top),
                (int)Math.Round(rectangle.Width - delta),
                rectangle.Height);
        }

        public Rectangle RightCollision(GameUnit x, GameUnit y, GameUnit delta)
        {
            return new Rectangle((int)Math.Round(x + rectangle.Left),
                (int)Math.Round(y + rectangle.Top),
                (int)Math.Round(rectangle.Width + delta),
                rectangle.Height);
        }

        public Rectangle TopCollision(GameUnit x, GameUnit y, GameUnit delta)
        {
            return new Rectangle((int)Math.Round(x + rectangle.Left),
                (int)Math.Round(y + rectangle.Top + delta),
                rectangle.Width,
                (int)Math.Round(rectangle.Height - delta));
        }

        public Rectangle BottomCollision(GameUnit x, GameUnit y, GameUnit delta)
        {
            return new Rectangle((int)Math.Round(x + rectangle.Left),
                (int)Math.Round(y + rectangle.Top),
                rectangle.Width,
                (int)Math.Round(rectangle.Height + delta));
        }
    }
}
