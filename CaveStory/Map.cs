using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public class Map
    {
        List<List<Sprite>> backgroundTiles;
        List<List<Tile>> tiles;
        Backdrop backdrop;

        public Map()
        {
            backgroundTiles = new List<List<Sprite>>();
            tiles = new List<List<Tile>>();
        }

        public static Map CreateTestMap(ContentManager Content)
        {
            Map map = new Map();
            map.backdrop = new FixedBackdrop("bkBlue", Content);

            TileUnit numRows = 15;
            TileUnit numCols = 20;

            map.tiles = new List<List<Tile>>();
            for (int i = 0; i < numRows; i++)
            {
                map.backgroundTiles.Add(new List<Sprite>());
                map.tiles.Add(new List<Tile>());
                for (int j = 0; j < numCols; j++)
                {
                    map.backgroundTiles[i].Add(null);
                    map.tiles[i].Add(new Tile());
                }
            }

            Sprite sprite = new Sprite(Content, "Stage/PrtCave", Units.TileToPixel(1), 0, Units.TileToPixel(1), Units.TileToPixel(1));
            Tile tile = new Tile(Tile.TileType.WallTile, sprite);
            TileUnit row = 11;
            for (TileUnit col = 0; col < numCols; col++)
            {
                map.tiles[Convert.ToInt32(row)][Convert.ToInt32(col)] = tile;
            }
            map.tiles[10][5] = tile;
            map.tiles[9][4] = tile;
            map.tiles[8][3] = tile;
            map.tiles[7][2] = tile;
            map.tiles[10][3] = tile;

            Sprite chainTop = new Sprite(Content, "Stage/PrtCave", Units.TileToPixel(11), Units.TileToPixel(2),
                Units.TileToPixel(1), Units.TileToPixel(1));
            Sprite chainMiddle = new Sprite(Content, "Stage/PrtCave", Units.TileToPixel(12), Units.TileToPixel(2),
                Units.TileToPixel(1), Units.TileToPixel(1));
            Sprite chainBottom = new Sprite(Content, "Stage/PrtCave", Units.TileToPixel(13), Units.TileToPixel(2),
                Units.TileToPixel(1), Units.TileToPixel(1));

            map.backgroundTiles[8][2] = chainTop;
            map.backgroundTiles[9][2] = chainMiddle;
            map.backgroundTiles[10][2] = chainBottom;
            return map;
        }

        public List<CollisionTile> GetCollidingTiles(Rectangle rectangle)
        {
            TileUnit firstRow = Units.GameToTile(rectangle.Top);
            TileUnit lastRow = Units.GameToTile(rectangle.Bottom);
            TileUnit firstCol = Units.GameToTile(rectangle.Left);
            TileUnit lastCol = Units.GameToTile(rectangle.Right);
            List<CollisionTile> collisionTiles = new List<CollisionTile>();
            for (TileUnit row = firstRow; row <= lastRow; row++)
            {
                for (TileUnit col = firstCol; col <= lastCol; col++)
                {
                    collisionTiles.Add(new CollisionTile(row, col, tiles[Convert.ToInt32(row)][Convert.ToInt32(col)].tileType));
                }
            }
            return collisionTiles;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (TileUnit row = 0; row < tiles.Count; row++)
            {
                for (TileUnit col = 0; col < tiles[Convert.ToInt32(row)].Count; col++)
                {
                    if (tiles[Convert.ToInt32(row)][Convert.ToInt32(col)].sprite != null)
                    {
                        tiles[Convert.ToInt32(row)][Convert.ToInt32(col)].sprite.Draw(spriteBatch,
                            Units.TileToGame(col), Units.TileToGame(row));
                    }
                }
            }
        }

        public void DrawBackground(SpriteBatch spriteBatch)
        {
            backdrop.Draw(spriteBatch);
            for (TileUnit row = 0; row < backgroundTiles.Count; row++)
            {
                for (TileUnit col = 0; col < backgroundTiles[Convert.ToInt32(row)].Count; col++)
                {
                    if (backgroundTiles[Convert.ToInt32(row)][Convert.ToInt32(col)] != null)
                    {
                        backgroundTiles[Convert.ToInt32(row)][Convert.ToInt32(col)].Draw(spriteBatch,
                            Units.TileToGame(col), Units.TileToGame(row));
                    }
                }
            }
        }
    }
}
