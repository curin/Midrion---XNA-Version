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

namespace com.dbtgaming.Library.UI
{
    public struct Sprite : ISprite
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="drawing">is this sprite drawing?</param>
        /// <param name="spriteBatch">spritebatch from game class</param>
        /// <param name="location">sprite location on screen</param>
        /// <param name="tex">texture</param>
        /// <param name="col">color overlay for texture</param>
        /// <param name="effects">spriteeffects to be applied at draw</param>
        /// <param name="rotation">sprite rotation</param>
        /// <param name="rotationOrigin">Origin of Rotation</param>
        /// <param name="depth">depth layer of sprite</param>
        /// <param name="Spritesheetsource">Location of sprite on spritesheet</param>
        public Sprite(string name, Rectangle location, Texture2D tex, Color col, 
            SpriteEffects effects, float rotation, Vector2 rotationOrigin, float depth, Rectangle Spritesheetsource,
            bool drawing)
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
            _spriteSheetLocation = Spritesheetsource;
            drawtemp = new Rectangle();
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

        private Rectangle _location;
        public Rectangle Location
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

        private Texture2D _tex;
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

        private Rectangle _spriteSheetLocation;
        public Rectangle SpriteSheetLocation
        {
            get
            {
                return _spriteSheetLocation;
            }
            set
            {
                _spriteSheetLocation = value;
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

        private Rectangle drawtemp;

        public void Draw(ICamera Camera)
        {
            if (_drawing)
                if (Utilities.Vector2ToRect(Camera.Location, Camera.Viewport.Width, Camera.Viewport.Height).Intersects(this.Location))
                {
                    drawtemp = new Rectangle(_location.X - (int)Camera.Location.X, _location.Y - (int)Camera.Location.Y, _location.Width, _location.Height);
                    UniversalVariables.spriteBatch.Draw(_tex, drawtemp, _spriteSheetLocation, _col, _rotation, _rotationOrigin, _effects, _depth);
                }
        }

        public void Dispose()
        {
            
        }
    }

    public interface ISprite: IDisposable
    {
        string Name { get; set; }
        /// <summary>
        /// draw sprite on screen
        /// </summary>
        void Draw(ICamera Camera);
    }

    public interface ICamera
    {
        Vector2 Location { get; set; }
        Rectangle Viewport { get; set; }
        void Update();
    }
}
