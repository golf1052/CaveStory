using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public struct SpriteState
    {
        public enum MotionType
        {
            FirstMotionType,
            Standing = FirstMotionType,
            Walking,
            Jumping,
            Falling,
            LastMotionType
        }

        public enum HorizontalFacing
        {
            FirstHorizontalFacing,
            Left = FirstHorizontalFacing,
            Right,
            LastHorizontalFacing
        }

        public enum VerticalFacing
        {
            FirstVerticalFacing,
            Up = FirstVerticalFacing,
            Down,
            Horizontal,
            LastVerticalFacing
        }

        public MotionType motionType;
        public HorizontalFacing horizontalFacing;
        public VerticalFacing verticalFacing;

        public SpriteState(MotionType motionType = MotionType.Standing,
            HorizontalFacing horizontalFacing = HorizontalFacing.Left,
            VerticalFacing verticalFacing = VerticalFacing.Horizontal)
        {
            this.motionType = motionType;
            this.horizontalFacing = horizontalFacing;
            this.verticalFacing = verticalFacing;
        }

        public static bool operator < (SpriteState a, SpriteState b)
        {
            if (a.motionType != b.motionType)
            {
                return a.motionType < b.motionType;
            }
            if (a.horizontalFacing != b.horizontalFacing)
            {
                return a.horizontalFacing < b.horizontalFacing;
            }
            if (a.verticalFacing != b.verticalFacing)
            {
                return a.verticalFacing < b.verticalFacing;
            }
            return false;
        }

        public static bool operator > (SpriteState a, SpriteState b)
        {
            return b < a;
        }
    }
}
