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
        List<List<Tile>> tiles;

        public Map()
        {
            tiles = new List<List<Tile>>();
        }

        public static Map CreateTestMap(ContentManager Content)
        {
            Map map = new Map();

            const int numRows = 15;
            const int numCols = 20;

            map.tiles = new List<List<Tile>>();
            for (int i = 0; i < numRows; i++)
            {
                map.tiles.Add(new List<Tile>());
                for (int j = 0; j < numCols; j++)
                {
                    map.tiles[i].Add(new Tile());
                }
            }

            Sprite sprite = new Sprite(Content, "PrtCave", Game1.TileSize, 0, Game1.TileSize, Game1.TileSize);
            Tile tile = new Tile(Tile.TileType.WallTile, sprite);
            const int row = 11;
            for (int col = 0; col < numCols; col++)
            {
                map.tiles[row][col] = tile;
            }
            map.tiles[10][5] = tile;
            map.tiles[9][4] = tile;
            map.tiles[8][3] = tile;
            map.tiles[7][2] = tile;
            map.tiles[10][3] = tile;
            return map;
        }

        public List<CollisionTile> GetCollidingTiles(Rectangle rectangle)
        {
            int firstRow = rectangle.Top / Game1.TileSize;
            int lastRow = rectangle.Bottom / Game1.TileSize;
            int firstCol = rectangle.Left / Game1.TileSize;
            int lastCol = rectangle.Right / Game1.TileSize;
            List<CollisionTile> collisionTiles = new List<CollisionTile>();
            for (int row = firstRow; row <= lastRow; row++)
            {
                for (int col = firstCol; col <= lastCol; col++)
                {
                    collisionTiles.Add(new CollisionTile(row, col, tiles[row][col].tileType));
                }
            }
            return collisionTiles;
        }

        public void Update(GameTime gameTime)
        {
            for (int row = 0; row < tiles.Count; row++)
            {
                for (int col = 0; col < tiles[row].Count; col++)
                {
                    if (tiles[row][col].sprite != null)
                    {
                        tiles[row][col].sprite.Update(gameTime);
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int row = 0; row < tiles.Count; row++)
            {
                for (int col = 0; col < tiles[row].Count; col++)
                {
                    if (tiles[row][col].sprite != null)
                    {
                        tiles[row][col].sprite.Draw(spriteBatch,
                            col * Game1.TileSize, row * Game1.TileSize);
                    }
                }
            }
        }
    }
}
