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
    public class ScreenManager
    {

        private ScreenCompare comp = new ScreenCompare();
        private int _highestPriority = 0;
        private Screen _temp;
        private List<Screen> screens = new List<Screen>();

        /// <summary>
        /// Query the Screen Manager for a specific Screen based on its unique ID
        /// </summary>
        /// <param name="ScreenID">The Identifier String for the Screen</param>
        /// <returns>Screen with Specified ID</returns>
        public Screen this[string ScreenID]
        {
            get
            {
                foreach (Screen sc in screens)
                {
                    if (sc.ID == ScreenID)
                    {
                        return sc;
                    }
                }
                return null;
            }
            set
            {
                foreach (Screen sc in screens)
                {
                    if (sc.ID == ScreenID)
                    {
                        _temp = sc;
                    }
                }
                _temp = value;
            }
        }

        /// <summary>
        /// Add a Screen to the Screen Manager
        /// </summary>
        /// <param name="screen">The Screen to be added</param>
        /// <param name="Priority">The priority in the load of the screens, don't put higher than necessary</param>
        public void Add(Screen screen, int Priority)
        {
            screen.Priority = Priority;
            screens.Add(screen);
            if (screen.Priority > _highestPriority)
            {
                _highestPriority = screen.Priority;
            }
            screens.Sort(comp);
        }

        /// <summary>
        /// add screens to the screen manager without rewriting the priority
        /// </summary>
        /// <param name="screen">Screen to be added</param>
        public void Add(Screen screen)
        {
            screens.Add(screen);
            if (screen.Priority > _highestPriority)
            {
                _highestPriority = screen.Priority;
            }
            screens.Sort(comp);
        }

        /// <summary>
        /// Basic constructor for the Screen manager
        /// </summary>
        /// <param name="Content">the Content manager from the Game class</param>
        /// <param name="Spritebatch">the Spritebatch from the Game class</param>
        /// <param name="Graphics">the GraphicsDeviceManager from the Game class</param>
        public ScreenManager()
        {
        }

        /// <summary>
        /// Initialize the active screens
        /// </summary>
        public void Initialize() 
        {
            //sort to put the highest priority first
            screens.Sort(comp);
            foreach (Screen sc in screens)
            {
                if (sc.IsActive)
                {
                    sc.Initialize();
                }
            } 
        }

        /// <summary>
        /// Load Content for the active Screens
        /// </summary>
        public void LoadContent() 
        {
            //sort to put the highest priority first
            screens.Sort(comp);
            foreach (Screen sc in screens)
            {
                if (sc.IsActive)
                {
                    sc.LoadContent();
                }
            }
        }

        /// <summary>
        /// Run the Update Methods for the active Screens
        /// </summary>
        /// <param name="gameTime">game time as passed from the game class</param>
        public void Update(GameTime gameTime, bool isactive) 
        {
            //sort to put the highest priority first
            screens.Sort(comp);
            foreach (Screen sc in screens)
            {
                if (sc.IsActive)
                {
                    sc.Update(gameTime, isactive);
                }
            }
        }

        /// <summary>
        /// Run the Draw Methods for the active Screens
        /// </summary>
        /// <param name="gameTime">game time as passed from the game class</param>
        public void Draw(GameTime gameTime, GraphicsDevice device) 
        {
            //sort to put the highest priority first
            screens.Sort(comp);
            foreach (Screen sc in screens)
            {
                if (sc.IsActive)
                {
                    //Draw 3D objects first
                    sc.Draw3D(gameTime, device);
                }
            }

            //start spritebatch for 2d draw
            UniversalVariables.spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            foreach (Screen sc in screens)
            {
                if (sc.IsActive)
                {
                    // Draw 2D elements
                    sc.Draw2D(gameTime, device);
                }
            }
            //End Spritebatch
            UniversalVariables.spriteBatch.End();
        }

        /// <summary>
        /// Unload the content from the active screens
        /// </summary>
        public void UnloadContent() 
        {
            foreach (Screen sc in screens)
            {
                if (sc.IsActive)
                    sc.UnloadContent();
            }
        }

        /// <summary>
        /// unload all content from every screen
        /// </summary>
        public void UnloadAllContent()
        {
            foreach (Screen sc in screens)
            {
                sc.UnloadContent();
            }
        }
    }

    /// <summary>
    /// a Class used to sort the screens in the Screen Manager
    /// </summary>
    class ScreenCompare : IComparer<Screen>
    {
        public int Compare(Screen screen1, Screen screen2)
        {
            return -screen1.Priority.CompareTo(screen2.Priority);
        }

    }

}
