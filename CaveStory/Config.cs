using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public class Config
    {
        public enum GraphicsQuality
        {
            OriginalQuality,
            HighQuality
        }

        public static GraphicsQuality graphicsQuality { get { return GraphicsQuality.OriginalQuality; } }
    }
}
