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
        List<List<Sprite>> foregroundSprites;

        public Map()
        {
            foregroundSprites = new List<List<Sprite>>();
        }

        public static Map CreateTestMap(ContentManager Content)
        {
            Map map = new Map();

            const int numRows = 15;
            const int numCols = 20;

            map.foregroundSprites = new List<List<Sprite>>();
            for (int i = 0; i < numRows; i++)
            {
                map.foregroundSprites.Add(new List<Sprite>());
                for (int j = 0; j < numCols; j++)
                {
                    map.foregroundSprites[i].Add(null);
                }
            }

            Sprite sprite = new Sprite(Content, "PrtCave", Game1.TileSize, 0, Game1.TileSize, Game1.TileSize);
            const int row = 11;
            for (int col = 0; col < numCols; col++)
            {
                map.foregroundSprites[row][col] = sprite;
            }
            map.foregroundSprites[10][5] = sprite;
            map.foregroundSprites[9][4] = sprite;
            map.foregroundSprites[8][3] = sprite;
            map.foregroundSprites[7][2] = sprite;
            map.foregroundSprites[10][3] = sprite;
            return map;
        }

        public void Update(GameTime gameTime)
        {
            for (int row = 0; row < foregroundSprites.Count; row++)
            {
                for (int col = 0; col < foregroundSprites[row].Count; col++)
                {
                    if (foregroundSprites[row][col] != null)
                    {
                        foregroundSprites[row][col].Update(gameTime);
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int row = 0; row < foregroundSprites.Count; row++)
            {
                for (int col = 0; col < foregroundSprites[row].Count; col++)
                {
                    if (foregroundSprites[row][col] != null)
                    {
                        foregroundSprites[row][col].Draw(spriteBatch,
                            col * Game1.TileSize, row * Game1.TileSize);
                    }
                }
            }
        }
    }
}
