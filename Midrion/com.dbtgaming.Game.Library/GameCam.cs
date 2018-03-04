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
using com.dbtgaming.Game.Library.Entities;

namespace com.dbtgaming.Game.Library
{
    public class GameCam : ICamera
    {
        public GameCam(Character followed, Rectangle viewport)
        {
            _location = Vector2.Zero;
            _viewport = viewport;
            _followed = followed;
            _location.X = (int)followed.Location.X - (int)(.5f * viewport.Width);
            _location.Y = (int)followed.Location.Y - (int)(.5f * viewport.Height);
        }

        protected Character _followed;
        public Character Followed
        {
            get
            {
                return _followed;
            }
            set
            {
                _followed = value;
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

        protected Rectangle _viewport;
        public Rectangle Viewport
        {
            get
            {
                return _viewport;
            }
            set
            {
                _viewport = value;
            }
        }
        
        public void Update()
        {
            _location.X = (int)_followed.Location.X - (int)(.5f * _viewport.Width);
            _location.Y = (int)_followed.Location.Y - (int)(.5f * _viewport.Height);
        }
    }
}
