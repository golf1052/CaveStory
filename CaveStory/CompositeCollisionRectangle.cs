using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CaveStory
{
    public class CompositeCollisionRectangle : ICollisionRectangle
    {
        Rectangle top;
        Rectangle bottom;
        Rectangle left;
        Rectangle right;

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle(left.Left, top.Top,
                    left.Width + right.Width, top.Height + bottom.Height);
            }
        }

        public CompositeCollisionRectangle(Rectangle top, Rectangle bottom,
            Rectangle left, Rectangle right)
        {
            this.top = top;
            this.bottom = bottom;
            this.left = left;
            this.right = right;
        }

        public Rectangle LeftCollision(GameUnit x, GameUnit y, GameUnit delta)
        {
            return new Rectangle((int)Math.Round(x + left.Left + delta),
                (int)Math.Round(y + left.Top),
                (int)Math.Round(left.Width - delta),
                left.Height);
        }

        public Rectangle RightCollision(GameUnit x, GameUnit y, GameUnit delta)
        {
            return new Rectangle((int)Math.Round(x + right.Left),
                (int)Math.Round(y + right.Top),
                (int)Math.Round(right.Width + delta),
                right.Height);
        }

        public Rectangle TopCollision(GameUnit x, GameUnit y, GameUnit delta)
        {
            return new Rectangle((int)Math.Round(x + top.Left),
                (int)Math.Round(y + top.Top + delta),
                top.Width,
                (int)Math.Round(top.Height - delta));
        }

        public Rectangle BottomCollision(GameUnit x, GameUnit y, GameUnit delta)
        {
            return new Rectangle((int)Math.Round(x + bottom.Left),
                (int)Math.Round(y + bottom.Top),
                bottom.Width,
                (int)Math.Round(bottom.Height + delta));
        }
    }
}
