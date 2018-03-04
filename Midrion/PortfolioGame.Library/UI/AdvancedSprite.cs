using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace com.dbtgaming.Library.UI
{
    public class AdvancedSprite : IAdvancedSprite
    {
        //basic constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">A unique identifier for this sprite</param>
        /// <param name="SpriteBatch">Spritebatch from game class</param>
        /// <param name="drawing">is this sprite drawing?</param>
        /// <param name="spriteBatch">spritebatch from game class</param>
        /// <param name="location">sprite location on screen</param>
        /// <param name="tex">texture</param>
        /// <param name="col">color overlay for texture</param>
        /// <param name="effects">spriteeffects to be applied at draw</param>
        /// <param name="rotation">sprite rotation</param>
        /// <param name="rotationOrigin">Origin of Rotation</param>
        /// <param name="depth">depth layer of sprite</param>
        /// <param name="startinganimation">name of starting animation</param>
        /// <param name="startingframe">frame to start on</param>
        /// <param name="updating">is the sprite updating?</param>
        /// <param name="startingAnim">the Animation that is used at start</param>
        public AdvancedSprite(string name, Vector2 location, Texture2D tex, Color col,
            SpriteEffects effects, float rotation, Vector2 rotationOrigin, float depth, string startinganimation,
            int startingframe, bool updating, Animation2D startingAnim, bool drawing)
        {
            _location = location;
            _col = col;
            _tex = tex;
            _effects = effects;
            _rotation = rotation;
            _rotationOrigin = rotationOrigin;
            _depth = depth;
            _currentAnimation = startinganimation;
            _frame = startingframe;
            _updating = updating;
            _animations.Add(startinganimation, startingAnim);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">A unique identifier for this sprite</param>
        /// <param name="SpriteBatch">Spritebatch from game class</param>
        /// <param name="drawing">is this sprite drawing? </param>
        /// <param name="spriteBatch">spritebatch from game class</param>
        /// <param name="location">sprite location on screen</param>
        /// <param name="tex">texture</param>
        /// <param name="col">color overlay for texture</param>
        /// <param name="effects">spriteeffects to be applied at draw</param>
        /// <param name="rotation">sprite rotation</param>
        /// <param name="rotationOrigin">Origin of Rotation</param>
        /// <param name="depth">depth layer of sprite</param>
        /// <param name="startinganimation">name of starting animation</param>
        /// <param name="startingframe">frame to start on</param>
        /// <param name="updating">is the sprite updating?</param>
        /// <param name="animations">Dictionary of animations and string names</param>
        public AdvancedSprite(string name, Vector2 location, Texture2D tex, Color col,
            SpriteEffects effects, float rotation, Vector2 rotationOrigin, float depth, string startinganimation,
            int startingframe, bool updating, Dictionary<string, Animation2D> animations, bool drawing)
        {
            _name = name;
            _drawing = drawing;
            _location = location;
            _col = col;
            _tex = tex;
            _effects = effects;
            _rotation = rotation;
            _rotationOrigin = rotationOrigin;
            _depth = depth;
            _currentAnimation = startinganimation;
            _frame = startingframe;
            _updating = updating;
            _animations = animations;
        }

        protected string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        protected Vector2 _location;
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

        protected Texture2D _tex;
        public Texture2D Tex
        {
            get
            {
                return _tex;
            }
            set
            {
                _tex = value;
            }
        }

        protected Color _col;
        public Color Col
        {
            get
            {
                return _col;
            }
            set
            {
                _col = value;
            }
        }

        protected SpriteEffects _effects;
        public SpriteEffects Effects
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

        protected float _rotation;
        public float Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
            }
        }

        protected Vector2 _rotationOrigin;
        public Vector2 RotationOrigin
        {
            get
            {
                return _rotationOrigin;
            }
        }

        protected float _depth;
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

        protected int _frame = 0;
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

        protected Dictionary<string, Animation2D> _animations = new Dictionary<string, Animation2D>();
        public Dictionary<string, Animation2D> Animations
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

        protected string _currentAnimation = "";
        public string CurrentAnimation
        {
            get
            {
                return _currentAnimation;
            }
            set
            {
                _currentAnimation = value;
                currentAnim = _animations[_currentAnimation];
            }
        }

        protected bool _updating = true;
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

        protected bool _drawing = true;
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

        /// <summary>
        /// internal frame count used when one sprite frame isn't one game frame
        /// </summary>
        protected int _internalFrame = 0;

        //update the animation and increase the frame
        public virtual void Update()
        {
            if (_updating)
            {
                if (_internalFrame >= _animations[_currentAnimation].GameFramesPerFrame - 1)
                {
                    _internalFrame = 0;
                    _frame++;
                }
                else
                {
                    _internalFrame++;
                }

                if (_frame > currentAnim.Frames - 1)
                {
                    _frame = 0;
                }
            }
        }


        protected Vector2 _scale = new Vector2(1, 1);
        /// <summary>
        /// Scale for sprite on screen from spritesheet
        /// </summary>
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

        /// <summary>
        /// rectangle for drawing sprite/base collision
        /// </summary>
        protected Rectangle tempdrawrect;
        /// <summary>
        /// temporary pointer to current animation
        /// </summary>
        protected Animation2D currentAnim;
        public Animation2D CurrentAnim
        {
            get
            {
                return currentAnim;
            }
        }

        /// <summary>
        /// temporary rectangle for source texture location of sprite
        /// </summary>
        protected Rectangle tempsourcerect;

        public virtual void Draw(ICamera Camera)
        {
            if (_rotation >= 2 * (float)Math.PI)
                _rotation = 0;
            if (_rotation <= -2 * (float)Math.PI)
                _rotation = 0;

            if (_drawing)
            {
                tempsourcerect = new Rectangle((int)(currentAnim.SpriteSheetLoc.X + (_frame * currentAnim.SpriteSize.X)), (int)(currentAnim.SpriteSheetLoc.Y), (int)currentAnim.SpriteSize.X, (int)currentAnim.SpriteSize.Y);
                tempdrawrect = new Rectangle((int)_location.X, (int)_location.Y, (int)(currentAnim.SpriteSize.X * _scale.X), (int)(currentAnim.SpriteSize.Y * _scale.Y));
                if (Utilities.Vector2ToRect(Camera.Location, Camera.Viewport.Width, Camera.Viewport.Height).Intersects(tempdrawrect))
                {
                    tempdrawrect.X -= (int)Camera.Location.X;
                    tempdrawrect.Y -= (int)Camera.Location.Y;
                    UniversalVariables.spriteBatch.Draw(_tex, tempdrawrect, tempsourcerect, _col, _rotation, _rotationOrigin, _effects, _depth);
                }
            }
        }

        public void Dispose()
        {

        }
    }

    public struct Animation2D
    {
        int _frames;
        public int Frames
        {
            get
            {
                return _frames;
            }
            set
            {
                _frames = value;
            }
        }

        private Vector2 _spriteSheetLoc;
        public Vector2 SpriteSheetLoc
        {
            get
            {
                return _spriteSheetLoc;
            }
            set
            {
                _spriteSheetLoc = value;
            }
        }

        private Vector2 _colCorrection;
        public Vector2 ColCorrection
        {
            get
            {
                return _colCorrection;
            }
            set
            {
                _colCorrection = value;
            }
        }

        private Vector2 _spriteSize;
        public Vector2 SpriteSize
        {
            get
            {
                return _spriteSize;
            }
            set
            {
                _spriteSize = value;
            }
        }

        private Vector2 _spriteSheetColLoc;
        public Vector2 SpriteSheetColLoc
        {
            get
            {
                return _spriteSheetColLoc;
            }
            set
            {
                _spriteSheetColLoc = value;
            }
        }

        private Vector2 _colSize;
        public Vector2 ColSize
        {
            get
            {
                return _colSize;
            }
            set
            {
                _colSize = value;
            }
        }

        private int _gamesFramesPerFrame;
        public int GameFramesPerFrame
        {
            get
            {
                return _gamesFramesPerFrame;
            }
            set
            {
                _gamesFramesPerFrame = value;
            }
        }

        public void Dispose()
        {
            //sdfdfsofhgrigy
        }
    }

    public interface IAdvancedSprite : ISprite
    {
        /// <summary>
        /// update the sprite
        /// </summary>
        void Update();
    }
}
