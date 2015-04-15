using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public static class Units
    {
        public static GameUnit TileSize
        {
            get
            {
                return new GameUnit(32.0f);
            }
        }

        public static PixelUnit GameToPixel(GameUnit game)
        {
            return new PixelUnit((int)Math.Round(game / 2.0f));
        }

        public static TileUnit GameToTile(GameUnit game)
        {
            return new TileUnit((uint)(game / TileSize));
        }

        public static GameUnit TileToGame(TileUnit tile)
        {
            return tile * TileSize;
        }

        public static PixelUnit TileToPixel(TileUnit tile)
        {
            return GameToPixel(TileToGame(tile));
        }
    }
}
