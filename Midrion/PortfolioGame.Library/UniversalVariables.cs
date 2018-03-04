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

namespace com.dbtgaming.Library
{
    public class UniversalVariables
    {
        protected static SpriteBatch _spriteBatch;
        public static SpriteBatch spriteBatch
        {
            get
            {
                return _spriteBatch;
            }
            set
            {
                _spriteBatch = value;
            }
        }

        protected static ContentManager _content;
        public static ContentManager content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
            }
        }

        protected static GraphicsDeviceManager _graphics;
        public static GraphicsDeviceManager graphics
        {
            get
            {
                return _graphics;
            }
            set
            {
                _graphics = value;
            }
        }

        protected static GameTime _gameTime;
        public static GameTime gameTime
        {
            get
            {
                return _gameTime;
            }
            set
            {
                _gameTime = value;
            }
        }
    }
}
