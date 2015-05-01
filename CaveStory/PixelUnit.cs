using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    /// <summary>
    /// Integer for discrete units. Pixel values can be +/-.
    /// </summary>
    public struct PixelUnit
    {
        public int Value { get; set; }

        public PixelUnit(int value)
        {
            Value = value;
        }

        public static implicit operator int(PixelUnit pixelUnit)
        {
            return pixelUnit.Value;
        }

        public static implicit operator PixelUnit(int i)
        {
            return new PixelUnit(i);
        }
    }
}
