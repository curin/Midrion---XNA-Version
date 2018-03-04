using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using com.dbtgaming.Library.UI;
using com.dbtgaming.Library.Engine2D.Collision;
using com.dbtgaming.Library.Engine2D.Actors;
using com.dbtgaming.Library;

namespace com.dbtgaming.Game.Library
{
    public class CompMouse : Mover
    {
        public CompMouse(string name, Texture2D tex, Animation2D Neutralanim)
            : base(name, new Vector2(InputManager.Mou.X, InputManager.Mou.Y), tex, Color.White, SpriteEffects.None, 
            0, Vector2.Zero, 1, "neutral", 0, true, Neutralanim, true)
        {

        }

        public override void Update()
        {
            base.Update();
        }

        public void Update(GameCam cam)
        {
            _location.X = InputManager.Mou.X + cam.Location.X;
            _location.Y = InputManager.Mou.Y + cam.Location.Y;
            base.Update();
        } 
    }

    public class CompMouse2 : IMovingSprite
    {
        public CompMouse2()
        {
            
        }

        float _maxSpeed;
        string _spriteID;
        Vector2 _origin;
        Dictionary<string, Animation2Data> _animations;
        Texture2D _spriteSheet;
        Rectangle _sourceRect;
        Rectangle _colSource;

        public Texture2D SpriteSheet 
        { 
            get
            {
                return _spriteSheet;
            }
            set
            {
                _spriteSheet = value;
            }
        }

        public Rectangle SourceRect 
        { 
            get
            {
                return _sourceRect;
            }
            set
            {
                _sourceRect = value;
            }
        }

        public Rectangle CollisonSource 
        { 
            get
            {
                return _colSource;
            }
            set
            {
                _colSource = value;
            }
        }

        public float MaxSpeed 
        { 
            get
            {
                return _maxSpeed;
            }
            set
            {
                _maxSpeed = value;
            }
        }

        public string SpriteID 
        { 
            get
            {
                return _spriteID;
            }
            set
            {
                _spriteID = value;
            }
        }

        public Vector2 Origin 
        { 
            get
            {
                return _origin;
            }
            set
            {
                _origin = value;
            }
        }

        public Dictionary<string, Animation2Data> Animations 
        { 
            get
            {
                return _animations;
            }
            set
            {
                _animations = value;
            }
        }

        public void Dispose()
        {

        }
        
        public byte Update(IAdvancedSpriteInstance _instance)
        {
            _instance.Location = new Vector2(InputManager.Mou.X, InputManager.Mou.Y);
            return 0;
        }

        public void PreLoad()
        {

        }

        public void Load(ContentManager content)
        {
            
        }

        public void PostLoad()
        {

        }

        public byte Draw(ISpriteInstance instance, Rectangle drawrect)
        {
            return 0;
        }

        public void PreUnload()
        {

        }

        public void Unload()
        {

        }

        public void PostUnload()
        {

        }

        public void AllStop(IMovingSpriteInstance instance)
        {

        }

        public void MoveTo(Vector2 Location, int FramesToMove, IMovingSpriteInstance instance)
        {

        }

        public void Move(Vector2 Direction, int speed, IMovingSpriteInstance instance)
        {

        }

        public void Move(Vector2 VelocityMod, IMovingSpriteInstance instance)
        {

        }
    }

    public class CompMouseInstance : IMovingSpriteInstance
    {
        Vector2 _moveTo;
        Vector2 _velocity;
        Vector2 _deceleration;
        float _maxVelocityMod;
        int _internalFrame;
        int _frame;
        float _speed;
        string _currentAnimation;
        CompMouse2 _base;
        bool _updating;
        string _instanceName;
        Vector2 _location;
        float _roatation;
        Color _drawColor;
        SpriteEffects _effects;
        float _depth;
        Vector2 _scale;
        bool _drawing;
        string _drawID;

        public Vector2 CollisionCorrection
        {
            get
            {
                return Animation.CollisionCorrection;
            }
            set
            {
                SpriteBase.Animations[_currentAnimation].CollisionCorrection = value;
            }
        }

        public Vector2 MoveTo 
        { 
            get
            {
                return _moveTo;
            }
            set
            {
                _moveTo = value;
            }
        }

        public Vector2 Velocity 
        { 
            get
            {
                return _velocity;
            }
            set
            {
                _velocity = value;
            }
        }

        public Vector2 Deceleration 
        { 
            get
            {
                return _deceleration;
            }
            set
            {
                _deceleration = value;
            }
        }

        public float MaxVelocityMod 
        { 
            get
            {
                return _maxVelocityMod;
            }
            set
            {
                _maxVelocityMod = value;
            }
        }

        public Animation2Data Animation 
        { 
            get
            {
                return SpriteBase.Animations[_currentAnimation];
            }
            set
            {
                foreach (string key in SpriteBase.Animations.Keys)
                {
                    if (SpriteBase.Animations[key] == value)
                        CurrentAnimation = key;
                }
            }
        }

        public int InternalFrame 
        { 
            get
            {
                return _internalFrame;
            }
            set
            {
                _internalFrame = value;
            }
        }

        public int Frame 
        { 
            get
            {
                return _frame;
            }
            set
            {
                _frame = value;
            }
        }

        public float Speed 
        { 
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
            }
        }

        public string CurrentAnimation 
        { 
            get
            {
                return _currentAnimation;
            }
            set
            {
                _currentAnimation = value;
            }
        }

        public IMovingSprite SpriteBase 
        { 
            get
            {
                return _base;
            }
            set
            {
                _base = (CompMouse2)value;
            }
        }

        IMovingSprite IMovingSpriteInstance.SpriteBase
        {
            get
            {
                return _base;
            }
            set
            {
                _base = (CompMouse2)value;
            }
        }

        IAdvancedSprite2 IAdvancedSpriteInstance.SpriteBase
        {
            get
            {
                return _base;
            }
            set
            {
                _base = (CompMouse2)value;
            }
        }

        ISprite2 ISpriteInstance.SpriteBase
        {
            get
            {
                return _base;
            }
            set
            {
                _base = (CompMouse2)value;
            }
        }

        public bool Updating 
        { 
            get
            {
                return _updating;
            }
            set
            {
                _updating = value;
            }
        }

        public string InstanceName 
        {
            get
            {
                return _instanceName;
            }
            set
            {
                _instanceName = value;
            }
        }

        public Vector2 Location 
        { 
            get
            {
                return _location;
            }
            set
            {
                _location = value;
            }
        }

        public float Rotation 
        { 
            get
            {
                return _roatation;
            }
            set
            {
                _roatation = value;
            }
        }

        public Color DrawColor 
        { 
            get
            {
                return _drawColor;
            }
            set
            {
                _drawColor = value;
            }
        }

        public SpriteEffects DrawEffects 
        { 
            get
            {
                return _effects;
            }
            set
            {
                _effects = value;
            }
        }

        public float Depth 
        { 
            get
            {
                return _depth;
            }
            set
            {
                _depth = value;
            }
        }

        public Vector2 Scale 
        { 
            get
            {
                return _scale;
            }
            set
            {
                _scale = value;
            }
        }

        public bool Drawing 
        { 
            get
            {
                return _drawing;
            }
            set
            {
                _drawing = value;
            }
        }

        public string CurrentDrawID
        {
            get
            {
                return _drawID;
            }
            set
            {
                _drawID = value;
            }
        }

        public void Dispose()
        {

        }
    }
}
