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
            Interacting,
            Walking,
            Jumping,
            Falling,
            LastMotionType
        }

        public enum StrideType
        {
            FirstStrideType,
            StrideMiddle = FirstStrideType,
            StrideLeft,
            StrideRight,
            LastStrideType
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

        private Tuple<MotionType, HorizontalFacing, VerticalFacing, StrideType> tuple;
        public MotionType motionType { get { return tuple.Item1; } }
        public HorizontalFacing horizontalFacing { get { return tuple.Item2; } }
        public VerticalFacing verticalFacing { get { return tuple.Item3; } }
        public StrideType strideType { get { return tuple.Item4; } }

        public SpriteState(Tuple<MotionType, HorizontalFacing, VerticalFacing, StrideType> tuple)
        {
            this.tuple = tuple;
        }
    }
}
