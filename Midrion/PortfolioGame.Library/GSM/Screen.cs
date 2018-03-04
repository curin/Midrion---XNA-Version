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

namespace com.dbtgaming.Library.GSM
{
    public abstract class Screen
    {
        protected ICamera cam;

        private InterfaceManager _interface = new InterfaceManager();
        public InterfaceManager Interface
        {
            get
            {
                return _interface;
            }
            set
            {
                _interface = value;
            }
        }

        public string ID
        {
            get
            {
                return _iD;
            }
        }
        protected string _iD = "";

        public bool IsWindow
        {
            get
            {
                return _isWindow;
            }
        }
        protected bool _isWindow = false;

        public bool Pauses
        {
            get
            {
                return _pauses;
            }
        }
        protected bool _pauses = false;

        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
            }
        }
        protected bool _isActive = false;

        public int Priority
        {
            get
            {
                return _priority;
            }
            set
            {
                _priority = value;
            }
        }
        protected int _priority = 0;

        public Screen(ScreenManager manager)
        {
            _scrnmanager = manager;
        }

        protected ScreenManager _scrnmanager;

        //classes to model the Game class setup

        /// <summary>
        /// Initializes this Screen
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// Loads this Screen's Content
        /// </summary>
        public virtual void LoadContent() { }

        /// <summary>
        /// Updates this Screen
        /// </summary>
        /// <param name="gameTime">game time as passed from the game class</param>
        public virtual void Update(GameTime gameTime, bool isactive) 
        {
            _interface.Update();
        }

        /// <summary>
        /// Draws the 3D Content for this Screen
        /// </summary>
        /// <param name="gameTime">game time as passed from the game class</param>
        public virtual void Draw3D(GameTime gameTime, GraphicsDevice device) { }

        /// <summary>
        /// Draws the 2D Content for this Screen
        /// </summary>
        /// <param name="gameTime">game time as passed from the game class</param>
        public virtual void Draw2D(GameTime gameTime, GraphicsDevice device) 
        {
            Interface.Draw(cam);
        }

        /// <summary>
        /// Unloads this Screen's Content
        /// </summary>
        public virtual void UnloadContent() { }

        /// <summary>
        /// Deinitializes this Screen
        /// </summary>
        public virtual void Deinitialize() { }
    }
}
