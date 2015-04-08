using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLX;

namespace CaveStory
{
    public class Game1 : Game
    {
        const string game1 = "game1";

        GraphicsDeviceManager graphics;
        World world;
        GameTimeWrapper mainGameTime;
        Texture2D sprites;
        Sprite sprite;

        public Game1() : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 640;
            graphics.PreferredBackBufferHeight = 480;
            graphics.IsFullScreen = false;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            world = new World(graphics);
            mainGameTime = new GameTimeWrapper(MainUpdate, this, 1.0m);
            world.AddGameState(game1, graphics);
            world.gameStates[game1].AddTime(mainGameTime);
            world.gameStates[game1].AddDraw(MainDraw);
            world.ActivateGameState(game1);
            sprites = Content.Load<Texture2D>("MyChar");
            sprite = new Sprite(new SpriteSheetInfo(32, 32), mainGameTime);
            sprite.animations["stand"] = sprite.animations.AddSpriteSheet(sprites, 1, 1, 1, SpriteSheet.Direction.LeftToRight, 100, true);
            sprite.animations.currentAnimation = "stand";
            sprite.Ready(graphics);
            sprite.pos = new Vector2(100, 100);
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            world.Update(gameTime);
        }

        public void MainUpdate(GameTimeWrapper gameTime)
        {
            sprite.Update(gameTime, graphics);
            world.gameStates[game1].UpdateCurrentCamera(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            world.DrawWorld();
        }

        public void MainDraw()
        {
            world.BeginDraw();
            world.Draw(sprite.Draw);
            world.EndDraw();
        }
    }
}
