using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace com.dbtgaming.Library.UI
{
    public struct SpriteText : ITextSprite
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text">text to be displayed</param>
        /// <param name="spriteBatch">spritebatch from the game class</param>
        /// <param name="location">location on screen</param>
        /// <param name="font">font to be used</param>
        /// <param name="col">color overlay for texture</param>
        /// <param name="effects">spriteeffects to be applied at draw</param>
        /// <param name="rotation">sprite rotation</param>
        /// <param name="rotationOrigin">Origin of Rotation</param>
        /// <param name="depth">depth layer of sprite</param>
        /// <param name="scale">scale of font</param>
        public SpriteText(string name, string text, Vector2 location, SpriteFont font, Color col,
            SpriteEffects effects, float rotation, Vector2 rotationOrigin, float depth, float scale)
        {
            _name = name;
            _col = col;
            _text = text;
            _font = font;
            _position = location;
            _effects = effects;
            _rotation = rotation;
            _rotationOrigin = rotationOrigin;
            _depth = depth;
            _scale = scale;
            _drawing = true;
            drawtemp = new Vector2();
        }

        string _name;
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

        //initializa properties and variables
        private SpriteFont _font;
        public SpriteFont Font
        {
            get
            {
                return _font;
            }
            set
            {
                _font = value;
            }
        }

        private string _text;
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }

        private Color _col;
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

        private SpriteEffects _effects;
        public SpriteEffects Effects
        {
            get
            {
                return _effects;
            }
        }

        private float _rotation;
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

        private Vector2 _rotationOrigin;
        public Vector2 RotationOrigin
        {
            get
            {
                return _rotationOrigin;
            }
            set
            {
                _rotationOrigin = value;
            }
        }

        private float _depth;
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

        private Vector2 _position;
        public Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        private float _scale;
        public float Scale
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

        private bool _drawing;
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

        private Vector2 drawtemp;

        public void Draw(ICamera Camera)
        {
            if (_drawing)
                if (Camera.Location.X < _position.X && Camera.Location.X + Camera.Viewport.Width > _position.X &&
                    Camera.Location.Y < _position.Y && Camera.Location.Y + Camera.Viewport.Height > _position.Y)
                {
                    drawtemp = new Vector2(_position.X - Camera.Location.X, _position.Y - Camera.Location.Y);
                    UniversalVariables.spriteBatch.DrawString(_font, _text, _position, _col, _rotation, _rotationOrigin, _scale, _effects, _depth);
                }
        }

        public void Dispose()
        {

        }
    }

    public interface ITextSprite : ISprite
    {
        SpriteFont Font { get; set; }
    }
}
