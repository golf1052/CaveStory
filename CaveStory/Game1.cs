using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CaveStory
{
    public class Game1 : Game
    {
        public static ObjectIDGenerator objectIdGen;
        public static TileUnit ScreenWidth { get { return 20; } }
        public static TileUnit ScreenHeight { get { return 15; } }
        public static Random Random { get; private set; }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        FirstCaveBat bat;
        Map map;
        Input input;
        DamageTexts damageTexts;
        Pickups pickups;
        ParticleSystem frontParticleSystem;
        ParticleSystem entityParticleSystem;
        ParticleTools particleTools;

        public Game1() : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = Config.graphicsQuality == Config.GraphicsQuality.OriginalQuality ?
                "Content/OriginalGraphics" : "Content/HighQualityGraphics";
            System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.AllScreens[0];
            Window.IsBorderless = true;
            Window.Position = new Point(screen.Bounds.X, screen.Bounds.Y);
            graphics.PreferredBackBufferWidth = Units.TileToPixel(ScreenWidth);
            graphics.PreferredBackBufferHeight = Units.TileToPixel(ScreenHeight);
            graphics.IsFullScreen = false;
        }

        protected override void Initialize()
        {
            objectIdGen = new ObjectIDGenerator();
            input = new Input();
            damageTexts = new DamageTexts();
            int seed = 507450343;
            Random = new Random(seed);
            pickups = new Pickups();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            frontParticleSystem = new ParticleSystem();
            entityParticleSystem = new ParticleSystem();
            particleTools = new ParticleTools(frontParticleSystem, entityParticleSystem, Content);
            player = new Player(Content, particleTools, Units.TileToGame(ScreenWidth / 2), Units.TileToGame(ScreenHeight / 2));
            damageTexts.AddDamageable(player);
            bat = new FirstCaveBat(Content, Units.TileToGame(7), Units.TileToGame(ScreenHeight / 2 + 1));
            damageTexts.AddDamageable(bat);
            map = Map.CreateSlopeTestMap(Content);

            pickups.Add(new PowerDoritoPickup(Content, bat.CenterX, bat.CenterY, PowerDoritoPickup.SizeType.Medium));
            pickups.Add(new PowerDoritoPickup(Content, bat.CenterX, bat.CenterY, PowerDoritoPickup.SizeType.Medium));
            pickups.Add(new PowerDoritoPickup(Content, bat.CenterX, bat.CenterY, PowerDoritoPickup.SizeType.Medium));
            base.LoadContent();
        }

        public static Texture2D LoadImage(ContentManager Content, string filePath, bool blackIsTransparent = false)
        {
            Texture2D tex = Content.Load<Texture2D>(filePath);
            Color[] colorData = new Color[tex.Width * tex.Height];
            tex.GetData(colorData);
            for (long i = 0; i < colorData.LongLength; i++)
            {
                if (colorData[i] == Color.Black)
                {
                    colorData[i] = Color.Transparent;
                }
            }
            tex.SetData(colorData);
            return tex;
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

            if (input.IsKeyHeld(Keys.Up) && input.IsKeyHeld(Keys.Down))
            {
                player.LookHorizontal();
            }
            else if (input.IsKeyHeld(Keys.Up))
            {
                player.LookUp();
            }
            else if (input.IsKeyHeld(Keys.Down))
            {
                player.LookDown();
            }
            else
            {
                player.LookHorizontal();
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

            if (input.WasKeyPressed(Keys.X))
            {
                player.StartFire();
            }
            else if (input.WasKeyReleased(Keys.X))
            {
                player.StopFire();
            }

            player.debugCollidingTiles.Clear();
            player.debugOppositeCollidingTiles.Clear();
            Timer.UpdateAll(gameTime);
            damageTexts.Update(gameTime);
            pickups.Update(gameTime, map);
            frontParticleSystem.Update(gameTime);
            entityParticleSystem.Update(gameTime);
            player.Update(gameTime, map);

            if (bat != null)
            {
                if (!bat.Update(gameTime, player.CenterX))
                {
                    DeathCloudParticle.CreateRandomDeathCloud(particleTools,
                        bat.CenterX, bat.CenterY,
                        3);
                    pickups.Add(FlashingPickup.HeartPickup(Content, bat.CenterX, bat.CenterY));
                    damageTexts.damageTextDict[bat.DamageText] = null;
                    bat = null;
                }
            }

            List<IProjectile> projectiles = player.Projectiles;
            for (int i = 0; i < projectiles.Count; i++)
            {
                if (bat != null && bat.CollisionRectangle.Intersects(projectiles[i].CollisionRectangle))
                {
                    bat.TakeDamage(projectiles[i].ContactDamage);
                    projectiles[i].CollideWithEnemy();
                }
            }

            pickups.HandleCollisions(player);

            if (bat != null && bat.DamageRectangle.Intersects(player.DamageRectangle))
            {
                player.TakeDamage(bat.ContactDamage);
            }

            input.EndFrame();
            base.Update(gameTime);
        }

        public void DebugDraw()
        {
            foreach (Tile2D tile in player.debugCollidingTiles)
            {
                Position2D position = tile.ToPixelUnit2D();
                Rectangle rectangle = RectangleExtensions.NewRectangle(position, new Tile2D(new Vector2(1)).ToPixelUnit2D());
                DrawRectangleOutline(rectangle, 1, Color.Red);
            }

            foreach (Tile2D tile in player.debugOppositeCollidingTiles)
            {
                Position2D position = tile.ToPixelUnit2D();
                Rectangle rectangle = RectangleExtensions.NewRectangle(position, new Tile2D(new Vector2(1)).ToPixelUnit2D());
                DrawRectangleOutline(rectangle, 1, Color.Green);
            }

            foreach (IPickup pickup in pickups.pickups)
            {
                if (pickup.Type == Pickup.PickupType.Experience)
                {
                    PowerDoritoPickup dorito = (PowerDoritoPickup)pickup;
                    foreach (Tile2D tile in dorito.debugCollidingTiles)
                    {
                        Position2D position = tile.ToPixelUnit2D();
                        Rectangle rectangle = RectangleExtensions.NewRectangle(position, new Tile2D(new Vector2(1)).ToPixelUnit2D());
                        DrawRectangleOutline(rectangle, 1, Color.Red);
                    }

                    foreach (Tile2D tile in dorito.debugOppositeCollidingTiles)
                    {
                        Position2D position = tile.ToPixelUnit2D();
                        Rectangle rectangle = RectangleExtensions.NewRectangle(position, new Tile2D(new Vector2(1)).ToPixelUnit2D());
                        DrawRectangleOutline(rectangle, 1, Color.Green);
                    }
                }
            }
        }

        public void Draw()
        {
            map.DrawBackground(spriteBatch);
            if (bat != null)
            {
                bat.Draw(spriteBatch);
            }
            entityParticleSystem.Draw(spriteBatch);
            pickups.Draw(spriteBatch);
            player.Draw(spriteBatch);
            map.Draw(spriteBatch);
            frontParticleSystem.Draw(spriteBatch);
            DebugDraw();
            damageTexts.Draw(spriteBatch);
            player.DrawHud(spriteBatch);
        }

        public void DrawRectangleOutline(Rectangle rectangle, GameUnit thickness, Color color)
        {
            List<Line> lines = new List<Line>();
            lines.Add(new Line(graphics, new Vector2(rectangle.Left, rectangle.Top), new Vector2(rectangle.Left, rectangle.Bottom), thickness));
            lines.Add(new Line(graphics, new Vector2(rectangle.Left, rectangle.Bottom), new Vector2(rectangle.Right, rectangle.Bottom), thickness));
            lines.Add(new Line(graphics, new Vector2(rectangle.Right, rectangle.Bottom), new Vector2(rectangle.Right, rectangle.Top), thickness));
            lines.Add(new Line(graphics, new Vector2(rectangle.Right, rectangle.Top), new Vector2(rectangle.Left, rectangle.Top), thickness));
            foreach (Line line in lines)
            {
                line.Draw(spriteBatch, color);
            }
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
