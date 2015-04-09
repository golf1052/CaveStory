using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public class Game1 : Game
    {
        public const int TileSize = 32;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        Input input;

        public Game1() : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.AllScreens[0];
            Window.IsBorderless = true;
            Window.Position = new Point(screen.Bounds.X, screen.Bounds.Y);
            graphics.PreferredBackBufferWidth = 640;
            graphics.PreferredBackBufferHeight = 480;
            graphics.IsFullScreen = false;
        }

        protected override void Initialize()
        {
            input = new Input();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            player = new Player(Content, 320, 240);
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            input.BeginFrame();

            if (input.WasKeyPressed(Keys.Escape))
            {
                Exit();
            }

            // Player horizontal movement
            if (input.IsKeyHeld(Keys.Left) && input.IsKeyHeld(Keys.Right))
            {
                player.StopMoving();
            }
            else if (input.IsKeyHeld(Keys.Left))
            {
                player.StartMovingLeft();
            }
            else if (input.IsKeyHeld(Keys.Right))
            {
                player.StartMovingRight();
            }
            else
            {
                player.StopMoving();
            }

            // Player Jump Logic
            if (input.WasKeyPressed(Keys.Z))
            {
                player.StartJump();
            }
            else if (input.WasKeyReleased(Keys.Z))
            {
                player.StopJump();
            }
            player.Update(gameTime);

            input.EndFrame();
            base.Update(gameTime);
        }

        public void Draw()
        {
            player.Draw(spriteBatch);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            Draw();
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
