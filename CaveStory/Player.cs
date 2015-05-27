using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public class Player : IDamageable
    {
        private class WalkingAnimation
        {
            public SpriteState.StrideType Stride
            {
                get
                {
                    switch (currentFrame)
                    {
                        case 0:
                            return SpriteState.StrideType.StrideLeft;
                        case 1:
                            return SpriteState.StrideType.StrideMiddle;
                        case 2:
                            return SpriteState.StrideType.StrideRight;
                        default:
                            return SpriteState.StrideType.StrideMiddle;
                    }
                }
            }

            Timer frameTimer;
            int currentFrame;
            bool forward;

            public WalkingAnimation()
            {
                frameTimer = new Timer(TimeSpan.FromMilliseconds(1000 / WalkFps));
                currentFrame = 0;
                forward = true;
            }

            public void Reset()
            {
                forward = true;
                currentFrame = 0;
                frameTimer.Reset(); 
            }

            public void Update()
            {
                if (frameTimer.Expired)
                {
                    frameTimer.Reset();

                    if (forward)
                    {
                        currentFrame++;
                        forward = currentFrame != NumWalkFrames - 1;
                    }
                    else
                    {
                        currentFrame--;
                        forward = currentFrame == 0;
                    }
                }
            }
        }

        // Walk Motion
        // (pixels / millisecond) / millisecond
        static AccelerationUnit Friction { get { return 0.00049804687f; } }
        // (pixels / millisecond) / millisecond
        static AccelerationUnit WalkingAcceleration { get { return 0.00083007812f; } }
        // pixels / millisecond
        static VelocityUnit MaxSpeedX { get { return 0.15859375f; } }

        // Fall Motion
        // (pixels / millisecond) / millisecond
        static AccelerationUnit Gravity { get { return 0.00078125f; } }
        // pixels / millisecond
        static VelocityUnit MaxSpeedY { get { return 0.2998046875f; } }

        // Jump Motion
        // pixels / millisecond
        static VelocityUnit JumpSpeed { get { return 0.25f; } }
        static VelocityUnit ShortJumpSpeed { get { return JumpSpeed / 1.5f; } }
        // (pixels / millisecond) / millisecond
        static AccelerationUnit AirAcceleration { get { return 0.0003125f; } }
        // (pixels / millisecond) / millisecond
        static AccelerationUnit JumpGravity { get { return 0.0003125f; } }

        // Sprites
        const string SpriteFilePath = "MyChar";

        // Sprite Frames
        static FrameUnit CharacterFrame { get { return 0; } }

        static FrameUnit WalkFrame { get { return 0; } }
        static FrameUnit StandFrame { get { return 0; } }
        static FrameUnit JumpFrame { get { return 1; } }
        static FrameUnit FallFrame { get { return 2; } }
        static FrameUnit UpFrameOffset { get { return 3; } }
        static FrameUnit DownFrame { get { return 6; } }
        static FrameUnit BackFrame { get { return 7; } }

        // Walk Animation
        static FrameUnit NumWalkFrames { get { return 3; } }
        static int WalkFps { get { return 15; } }

        // Collision Rectangle
        GameUnit CollisionYTop { get { return 2; } }
        GameUnit CollisionYHeight { get { return 30; } }
        GameUnit CollisionTopWidth { get { return 18; } }
        GameUnit CollisionBottomWidth { get { return 10; } }
        GameUnit CollisionTopLeft { get { return (Units.TileToGame(1) - CollisionTopWidth) / 2; } }
        GameUnit CollisionBottomLeft { get { return (Units.TileToGame(1) - CollisionBottomWidth) / 2; } }
        CollisionRectangle collisionRectangle;

        TimeSpan InvincibleFlashTime { get { return TimeSpan.FromMilliseconds(50); } }

        TimeSpan InvincibleTime { get { return TimeSpan.FromMilliseconds(3000); } }

        public GameUnit CenterX { get { return kinematicsX.position + Units.HalfTile; } }
        public GameUnit CenterY { get { return kinematicsY.position + Units.HalfTile; } }

        ParticleTools particleTools;
        Kinematics kinematicsX;
        Kinematics kinematicsY;
        int accelerationX;
        SpriteState.HorizontalFacing horizontalFacing;
        SpriteState.VerticalFacing intendedVerticalFacing;
        private bool onGround;
        public SpriteState.MotionType MotionType
        {
            get
            {
                SpriteState.MotionType motion;
                if (interacting)
                {
                    motion = SpriteState.MotionType.Interacting;
                }
                else if (OnGround)
                {
                    motion = accelerationX == 0 ? SpriteState.MotionType.Standing : SpriteState.MotionType.Walking;
                }
                else
                {
                    motion = kinematicsY.velocity < 0.0f ? SpriteState.MotionType.Jumping : SpriteState.MotionType.Falling;
                }
                return motion;
            }
        }

        bool OnGround
        {
            get
            {
                return onGround;
            }
            set
            {
                onGround = value;
            }
        }

        bool GunUp
        {
            get
            {
                return MotionType == SpriteState.MotionType.Walking &&
                    walkingAnimation.Stride != SpriteState.StrideType.StrideMiddle;
            }
        }

        public SpriteState.VerticalFacing VerticalFacing
        {
            get
            {
                return OnGround && intendedVerticalFacing == SpriteState.VerticalFacing.Down ?
                    SpriteState.VerticalFacing.Horizontal : intendedVerticalFacing;
            }
        }
        private bool jumpActive;
        PlayerHealth playerHealth;
        bool interacting;
        Timer invincibleTimer;
        DamageText damageText;
        public DamageText DamageText { get { return damageText; } }

        WalkingAnimation walkingAnimation;

        GunExperienceHud gunExperienceHud;
        PolarStar polarStar;

        Dictionary<SpriteState, Sprite> sprites;

        public SpriteState SpriteState
        {
            get
            {
                return new SpriteState(
                    new Tuple<SpriteState.MotionType, SpriteState.HorizontalFacing, SpriteState.VerticalFacing, SpriteState.StrideType>(
                        MotionType, horizontalFacing, VerticalFacing, walkingAnimation.Stride));
            }
        }

        public Rectangle DamageRectangle
        {
            get
            {
                return new Rectangle((int)Math.Round(kinematicsX.position) + collisionRectangle.BoundingBox.Left,
                    (int)Math.Round(kinematicsY.position) + collisionRectangle.BoundingBox.Top,
                    collisionRectangle.BoundingBox.Width,
                    collisionRectangle.BoundingBox.Height);
            }
        }

        public List<IProjectile> Projectiles
        {
            get
            {
                return polarStar.Projectiles;
            }
        }

        public Player(ContentManager Content, ParticleTools particleTools, GameUnit x, GameUnit y)
        {
            sprites = new Dictionary<SpriteState, Sprite>();
            InitializeSprites(Content);
            this.particleTools = particleTools;
            kinematicsX = new Kinematics(x, 0);
            kinematicsY = new Kinematics(y, 0);
            accelerationX = 0;
            horizontalFacing = SpriteState.HorizontalFacing.Left;
            intendedVerticalFacing = SpriteState.VerticalFacing.Horizontal;
            walkingAnimation = new WalkingAnimation();
            onGround = false;
            jumpActive = false;
            playerHealth = new PlayerHealth(Content);
            interacting = false;
            invincibleTimer = new Timer(InvincibleTime);
            damageText = new DamageText(Content);
            gunExperienceHud = new GunExperienceHud(Content);
            polarStar = new PolarStar(Content);
            collisionRectangle = new CollisionRectangle(new Rectangle((int)Math.Round(CollisionTopLeft), (int)Math.Round(CollisionYTop),
                (int)Math.Round(CollisionTopWidth), (int)Math.Round(CollisionYHeight / 2)),
                new Rectangle((int)Math.Round(CollisionBottomLeft), (int)Math.Round(CollisionYTop + CollisionYHeight / 2),
                (int)Math.Round(CollisionBottomWidth), (int)Math.Round(CollisionYHeight / 2)),
                new Rectangle(6, 10, 10, 12),
                new Rectangle(16, 10, 10, 12));
        }

        public void InitializeSprites(ContentManager Content)
        {
            //healthNumberSprite = new NumberSprite(Content, 52, HealthNumberNumDigits);

            for (SpriteState.MotionType motionType = SpriteState.MotionType.FirstMotionType;
                motionType < SpriteState.MotionType.LastMotionType;
                ++motionType)
            {
                for (SpriteState.HorizontalFacing horizontalFacing = SpriteState.HorizontalFacing.FirstHorizontalFacing;
                    horizontalFacing < SpriteState.HorizontalFacing.LastHorizontalFacing;
                    ++horizontalFacing)
                {
                    for (SpriteState.VerticalFacing verticalFacing = SpriteState.VerticalFacing.FirstVerticalFacing;
                        verticalFacing < SpriteState.VerticalFacing.LastVerticalFacing;
                        ++verticalFacing)
                    {
                        for (SpriteState.StrideType strideType = SpriteState.StrideType.FirstStrideType;
                            strideType < SpriteState.StrideType.LastStrideType;
                            ++strideType)
                        InitializeSprite(Content, new SpriteState(
                            new Tuple<SpriteState.MotionType, SpriteState.HorizontalFacing, SpriteState.VerticalFacing, SpriteState.StrideType>(
                                motionType, horizontalFacing, verticalFacing, strideType)));
                    }
                }
            }
        }

        public void InitializeSprite(ContentManager Content, SpriteState spriteState)
        {
            TileUnit tileY = spriteState.horizontalFacing == SpriteState.HorizontalFacing.Left ?
                Convert.ToUInt32(CharacterFrame) : Convert.ToUInt32(1 + CharacterFrame);

            TileUnit tileX = 0;
            switch (spriteState.motionType)
            {
                case SpriteState.MotionType.Walking:
                    tileX = Convert.ToUInt32(WalkFrame);
                    break;
                case SpriteState.MotionType.Standing:
                    tileX = Convert.ToUInt32(StandFrame);
                    break;
                case SpriteState.MotionType.Interacting:
                    tileX = Convert.ToUInt32(BackFrame);
                    break;
                case SpriteState.MotionType.Jumping:
                    tileX = Convert.ToUInt32(JumpFrame);
                    break;
                case SpriteState.MotionType.Falling:
                    tileX = Convert.ToUInt32(FallFrame);
                    break;
                case SpriteState.MotionType.LastMotionType:
                    break;
            }
            
            switch (spriteState.verticalFacing)
            {
                case SpriteState.VerticalFacing.Horizontal:
                    break;
                case SpriteState.VerticalFacing.Up:
                    tileX += Convert.ToUInt32(UpFrameOffset);
                    break;
                case SpriteState.VerticalFacing.Down:
                    tileX = Convert.ToUInt32(DownFrame);
                    break;
                default:
                    break;
            }

            if (spriteState.motionType == SpriteState.MotionType.Walking)
            {
                switch (spriteState.strideType)
                {
                    case SpriteState.StrideType.StrideMiddle:
                        break;
                    case SpriteState.StrideType.StrideLeft:
                        tileX += 1;
                        break;
                    case SpriteState.StrideType.StrideRight:
                        tileX += 2;
                        break;
                    default:
                        break;
                }
                sprites.Add(spriteState, new Sprite(Content, SpriteFilePath,
                    Units.TileToPixel(tileX), Units.TileToPixel(tileY),
                    Units.TileToPixel(1), Units.TileToPixel(1)));
            }
            else
            {
                sprites.Add(spriteState, new Sprite(Content, SpriteFilePath,
                    Units.TileToPixel(tileX), Units.TileToPixel(tileY),
                    Units.TileToPixel(1), Units.TileToPixel(1)));
            }
        }

        public void Update(GameTime gameTime, Map map)
        {
            sprites[SpriteState].Update();

            playerHealth.Update(gameTime);

            walkingAnimation.Update();

            polarStar.UpdateProjectiles(gameTime, map, particleTools);

            UpdateX(gameTime, map);
            UpdateY(gameTime, map);
        }

        public void UpdateX(GameTime gameTime, Map map)
        {
            AccelerationUnit accX = 0;
            if (accelerationX < 0)
            {
                accX = OnGround ? -WalkingAcceleration : -AirAcceleration;
            }
            else if (accelerationX > 0)
            {
                accX = OnGround ? WalkingAcceleration : AirAcceleration;
            }
            kinematicsX.velocity += accX * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (accelerationX < 0)
            {
                kinematicsX.velocity = Math.Max(kinematicsX.velocity, -MaxSpeedX);
            }
            else if (accelerationX > 0)
            {
                kinematicsX.velocity = Math.Min(kinematicsX.velocity, MaxSpeedX);
            }
            else if (OnGround)
            {
                kinematicsX.velocity = kinematicsX.velocity > 0.0f ?
                    (float)Math.Max(0.0f, kinematicsX.velocity - Friction * gameTime.ElapsedGameTime.TotalMilliseconds) :
                    (float)Math.Min(0.0f, kinematicsX.velocity + Friction * gameTime.ElapsedGameTime.TotalMilliseconds);
            }

            GameUnit delta = kinematicsX.velocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            
            if (delta > 0.0f)
            {
                CollisionInfo info = GetWallCollisionInfo(map, collisionRectangle.RightCollision(kinematicsX.position, kinematicsY.position, delta));

                if (info.collided)
                {
                    kinematicsX.position = Units.TileToGame(info.col) - collisionRectangle.BoundingBox.Right;
                    OnCollision(MapCollidable.SideType.RightSide, true);
                }
                else
                {
                    kinematicsX.position += delta;
                    OnDelta(MapCollidable.SideType.RightSide);
                }

                info = GetWallCollisionInfo(map, collisionRectangle.LeftCollision(kinematicsX.position, kinematicsY.position, 0));

                if (info.collided)
                {
                    kinematicsX.position = Units.TileToGame(info.col + 1) - collisionRectangle.BoundingBox.Left;
                    OnCollision(MapCollidable.SideType.LeftSide, false);
                }
            }
            else
            {
                CollisionInfo info = GetWallCollisionInfo(map, collisionRectangle.LeftCollision(kinematicsX.position, kinematicsY.position, delta));

                if (info.collided)
                {
                    kinematicsX.position = Units.TileToGame(info.col + 1) - collisionRectangle.BoundingBox.Left;
                    OnCollision(MapCollidable.SideType.LeftSide, true);
                }
                else
                {
                    kinematicsX.position += delta;
                    OnDelta(MapCollidable.SideType.LeftSide);
                }

                info = GetWallCollisionInfo(map, collisionRectangle.RightCollision(kinematicsX.position, kinematicsY.position, 0));

                if (info.collided)
                {
                    kinematicsX.position = Units.TileToGame(info.col) - collisionRectangle.BoundingBox.Right;
                    OnCollision(MapCollidable.SideType.RightSide, false);
                }
            }
        }

        public void UpdateY(GameTime gameTime, Map map)
        {
            AccelerationUnit gravity = jumpActive && kinematicsY.velocity < 0 ?
                JumpGravity : Gravity;
            kinematicsY.velocity = (float)Math.Min(kinematicsY.velocity + gravity * gameTime.ElapsedGameTime.TotalMilliseconds, MaxSpeedY);

            GameUnit delta = kinematicsY.velocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (delta > 0)
            {
                CollisionInfo info = GetWallCollisionInfo(map, collisionRectangle.BottomCollision(kinematicsX.position, kinematicsY.position, delta));

                if (info.collided)
                {
                    kinematicsY.position = Units.TileToGame(info.row) - collisionRectangle.BoundingBox.Bottom;
                    OnCollision(MapCollidable.SideType.BottomSide, true);
                }
                else
                {
                    kinematicsY.position += delta;
                    OnDelta(MapCollidable.SideType.BottomSide);
                }

                info = GetWallCollisionInfo(map, collisionRectangle.TopCollision(kinematicsX.position, kinematicsY.position, 0));

                if (info.collided)
                {
                    kinematicsY.position = Units.TileToGame(info.row + 1) - collisionRectangle.BoundingBox.Top;
                    OnCollision(MapCollidable.SideType.TopSide, false);
                }
            }
            else
            {
                CollisionInfo info = GetWallCollisionInfo(map, collisionRectangle.TopCollision(kinematicsX.position, kinematicsY.position, delta));

                if (info.collided)
                {
                    kinematicsY.position = Units.TileToGame(info.row + 1) - collisionRectangle.BoundingBox.Top;
                    OnCollision(MapCollidable.SideType.TopSide, true);
                }
                else
                {
                    kinematicsY.position += delta;
                    OnDelta(MapCollidable.SideType.TopSide);
                }

                info = GetWallCollisionInfo(map, collisionRectangle.BottomCollision(kinematicsX.position, kinematicsY.position, 0));

                if (info.collided)
                {
                    kinematicsY.position = Units.TileToGame(info.row) - collisionRectangle.BoundingBox.Bottom;
                    OnCollision(MapCollidable.SideType.BottomSide, false);
                }
            }
        }

        protected void OnCollision(MapCollidable.SideType side, bool isDeltaDirection)
        {
            switch (side)
            {
                case MapCollidable.SideType.TopSide:
                    if (isDeltaDirection)
                    {
                        kinematicsY.velocity = 0;
                    }
                    particleTools.FrontSystem.AddNewParticle(new HeadBumpParticle(particleTools.Content, CenterX, kinematicsY.position + collisionRectangle.BoundingBox.Top));
                    break;
                case MapCollidable.SideType.BottomSide:
                    onGround = true;
                    if (isDeltaDirection)
                    {
                        kinematicsY.velocity = 0;
                    }
                    break;
                case MapCollidable.SideType.LeftSide:
                    if (isDeltaDirection)
                    {
                        kinematicsX.velocity = 0;
                    }
                    break;
                case MapCollidable.SideType.RightSide:
                    if (isDeltaDirection)
                    {
                        kinematicsX.velocity = 0;
                    }
                    break;
            }
        }

        protected void OnDelta(MapCollidable.SideType side)
        {
            switch (side)
            {
                case MapCollidable.SideType.TopSide:
                    onGround = false;
                    break;
                case MapCollidable.SideType.BottomSide:
                    onGround = true;
                    break;
                case MapCollidable.SideType.LeftSide:
                    break;
                case MapCollidable.SideType.RightSide:
                    break;
            }
        }

        CollisionInfo GetWallCollisionInfo(Map map, Rectangle rectangle)
        {
            CollisionInfo info = new CollisionInfo(false, 0, 0);
            List<CollisionTile> tiles = map.GetCollidingTiles(rectangle);
            for (int i = 0; i < tiles.Count; i++)
            {
                if (tiles[i].tileType == Tile.TileType.WallTile)
                {
                    info = new CollisionInfo(true, tiles[i].row, tiles[i].col);
                    break;
                }
            }
            return info;
        }

        public void StartMovingLeft()
        {
            if (OnGround && accelerationX == 0)
            {
                walkingAnimation.Reset();
            }
            interacting = false;
            accelerationX = -1;
            horizontalFacing = SpriteState.HorizontalFacing.Left;
        }

        public void StartMovingRight()
        {
            if (OnGround && accelerationX == 0)
            {
                walkingAnimation.Reset();
            }
            interacting = false;
            accelerationX = 1;
            horizontalFacing = SpriteState.HorizontalFacing.Right;
        }

        public void StopMoving()
        {
            accelerationX = 0;
        }

        public void LookUp()
        {
            interacting = false;
            intendedVerticalFacing = SpriteState.VerticalFacing.Up;
        }

        public void LookDown()
        {
            if (intendedVerticalFacing == SpriteState.VerticalFacing.Down)
            {
                return;
            }
            interacting = OnGround;
            intendedVerticalFacing = SpriteState.VerticalFacing.Down;
        }

        public void LookHorizontal()
        {
            intendedVerticalFacing = SpriteState.VerticalFacing.Horizontal;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (SpriteIsVisible())
            {
                polarStar.Draw(spriteBatch, horizontalFacing, VerticalFacing, GunUp, kinematicsX.position, kinematicsY.position);
                sprites[SpriteState].Draw(spriteBatch, kinematicsX.position, kinematicsY.position);
            }
        }

        public void DrawHud(SpriteBatch spriteBatch)
        {
            if (SpriteIsVisible())
            {
                playerHealth.Draw(spriteBatch);
                polarStar.DrawHud(spriteBatch, gunExperienceHud);
            }
        }

        public void StartJump()
        {
            interacting = false;
            jumpActive = true;
            if (OnGround)
            {
                kinematicsY.velocity = -JumpSpeed;
            }
        }

        public void StopJump()
        {
            jumpActive = false;
        }

        public void StartFire()
        {
            polarStar.StartFire(kinematicsX.position, kinematicsY.position, horizontalFacing, VerticalFacing, GunUp, particleTools);
        }

        public void StopFire()
        {
            polarStar.StopFire();
        }

        public void TakeDamage(HPUnit damage)
        {
            if (invincibleTimer.Active)
            {
                return;
            }
            playerHealth.health.TakeDamage(damage);
            damageText.Damage = damage;
            kinematicsY.velocity = Math.Min(kinematicsY.velocity, -ShortJumpSpeed);
            invincibleTimer.Reset();
        }

        bool SpriteIsVisible()
        {
            return !(invincibleTimer.Active && invincibleTimer.CurrentTime.Ticks / InvincibleFlashTime.Ticks % 2 == 0);
        }
    }
}
