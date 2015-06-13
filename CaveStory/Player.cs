using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Collections;

namespace CaveStory
{
    public class Player : MapCollidable, IDamageable
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

        static BidirectionalAccelerators WalkingAccelerators = new BidirectionalAccelerators(WalkingAcceleration, MaxSpeedX);
        static FrictionAccelerator FrictionAccelerator = new FrictionAccelerator(Friction);

        // Jump Motion
        // pixels / millisecond
        static VelocityUnit JumpSpeed { get { return 0.25f; } }
        static VelocityUnit ShortJumpSpeed { get { return JumpSpeed / 1.5f; } }
        // (pixels / millisecond) / millisecond
        static AccelerationUnit AirAcceleration { get { return 0.0003125f; } }
        // (pixels / millisecond) / millisecond
        static AccelerationUnit JumpGravity { get { return 0.0003125f; } }

        static BidirectionalAccelerators AirAccelerators = new BidirectionalAccelerators(AirAcceleration, MaxSpeedX);
        static ConstantAccelerator JumpGravityAccelerator = new ConstantAccelerator(JumpGravity, ConstantAccelerator.TerminalSpeed);

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
        ICollisionRectangle collisionRectangle;

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
        private BitArray maybeGroundTile;
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
                return maybeGroundTile != null;
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
        FloatingNumber damageText;
        public FloatingNumber DamageText { get { return damageText; } }
        FloatingNumber experienceText;
        public FloatingNumber ExperienceText { get { return experienceText; } }

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
            maybeGroundTile = null;
            jumpActive = false;
            playerHealth = new PlayerHealth(Content);
            interacting = false;
            invincibleTimer = new Timer(InvincibleTime);
            damageText = new FloatingNumber(Content, FloatingNumber.NumberType.Damage);
            experienceText = new FloatingNumber(Content, FloatingNumber.NumberType.Experience);
            gunExperienceHud = new GunExperienceHud(Content);
            polarStar = new PolarStar(Content);
            collisionRectangle = new CompositeCollisionRectangle(new Rectangle((int)Math.Round(CollisionTopLeft), (int)Math.Round(CollisionYTop),
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

            experienceText.Update(gameTime);
            experienceText.SetPosition(CenterX, CenterY);
        }

        public void UpdateX(GameTime gameTime, Map map)
        {
            IAccelerator accelerator;
            if (OnGround)
            {
                if (accelerationX == 0)
                {
                    accelerator = FrictionAccelerator;
                }
                else if (accelerationX < 0)
                {
                    accelerator = WalkingAccelerators.negative;
                }
                else
                {
                    accelerator = WalkingAccelerators.positive;
                }
            }
            else
            {
                if (accelerationX == 0)
                {
                    accelerator = ZeroAccelerator.Zero;
                }
                else if (accelerationX < 0)
                {
                    accelerator = AirAccelerators.negative;
                }
                else
                {
                    accelerator = AirAccelerators.positive;
                }
            }

            UpdateX(collisionRectangle, accelerator, kinematicsX, kinematicsY, gameTime, map);
        }

        public void UpdateY(GameTime gameTime, Map map)
        {
            IAccelerator accelerator = jumpActive && kinematicsY.velocity < 0 ? JumpGravityAccelerator : ConstantAccelerator.Gravity;

            UpdateY(collisionRectangle, accelerator, kinematicsX, kinematicsY, gameTime, map, maybeGroundTile);
        }

        protected override void OnCollision(TileInfo.SideType side, bool isDeltaDirection, BitArray tileType)
        {
            switch (side)
            {
                case TileInfo.SideType.TopSide:
                    if (isDeltaDirection)
                    {
                        kinematicsY.velocity = 0;
                        particleTools.FrontSystem.AddNewParticle(new HeadBumpParticle(particleTools.Content, CenterX, kinematicsY.position + collisionRectangle.BoundingBox.Top));
                    }
                    break;
                case TileInfo.SideType.BottomSide:
                    maybeGroundTile = tileType;
                    if (isDeltaDirection)
                    {
                        kinematicsY.velocity = 0;
                    }
                    break;
                case TileInfo.SideType.LeftSide:
                    if (isDeltaDirection)
                    {
                        kinematicsX.velocity = 0;
                    }
                    break;
                case TileInfo.SideType.RightSide:
                    if (isDeltaDirection)
                    {
                        kinematicsX.velocity = 0;
                    }
                    break;
            }
        }

        protected override void OnDelta(TileInfo.SideType side)
        {
            switch (side)
            {
                case TileInfo.SideType.TopSide:
                case TileInfo.SideType.BottomSide:
                    maybeGroundTile = null;
                    break;
                case TileInfo.SideType.LeftSide:
                    break;
                case TileInfo.SideType.RightSide:
                    break;
            }
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
            experienceText.Draw(spriteBatch);
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
            interacting = false;
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
            damageText.Value = damage;
            polarStar.DamageExperience(damage * 2);
            kinematicsY.velocity = Math.Min(kinematicsY.velocity, -ShortJumpSpeed);
            invincibleTimer.Reset();
        }

        public void CollectPickup(IPickup pickup)
        {
            if (pickup.Type == Pickup.PickupType.Experience)
            {
                polarStar.CollectExpereince(pickup.Value);
                experienceText.Value = pickup.Value;
                gunExperienceHud.ActivateFlash();
            }
            else if (pickup.Type == Pickup.PickupType.Health)
            {
                playerHealth.health.AddHealth(pickup.Value);
            }
        }

        bool SpriteIsVisible()
        {
            return !(invincibleTimer.Active && invincibleTimer.CurrentTime.Ticks / InvincibleFlashTime.Ticks % 2 == 0);
        }
    }
}
