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
            Standing,
            Walking
        }

        public enum HorizontalFacing
        {
            Left,
            Right
        }

        MotionType motionType;
        HorizontalFacing horizontalFacing;

        public SpriteState(MotionType motionType = MotionType.Standing,
            HorizontalFacing horizontalFacing = HorizontalFacing.Left)
        {
            this.motionType = motionType;
            this.horizontalFacing = horizontalFacing;
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
            return false;
        }

        public static bool operator > (SpriteState a, SpriteState b)
        {
            return b < a;
        }
    }
}
