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

        private Tuple<MotionType, HorizontalFacing, VerticalFacing> tuple;
        public MotionType motionType { get { return tuple.Item1; } }
        public HorizontalFacing horizontalFacing { get { return tuple.Item2; } }
        public VerticalFacing verticalFacing { get { return tuple.Item3; } }

        public SpriteState(Tuple<MotionType, HorizontalFacing, VerticalFacing> tuple)
        {
            this.tuple = tuple;
        }
    }
}
