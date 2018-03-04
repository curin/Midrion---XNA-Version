using com.dbtgaming.Game.Library.Screens;
using com.dbtgaming.Library;
using com.dbtgaming.Library.GSM;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace com.dbtgaming.Game
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ScreenManager manager;
        PlayScreen scrn;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            InputManager.PopulateLists();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            UniversalVariables.spriteBatch = spriteBatch;
            UniversalVariables.content = Content;
            UniversalVariables.graphics = graphics;
            ///
            //Remove Content and Graphics references
            ///

            manager = new ScreenManager();
            scrn = new PlayScreen(manager);
            manager.Add(scrn);
            scrn.IsActive = true;
            manager.Initialize();
            // TODO: use this.Content to load your game content here
            manager.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            manager.UnloadAllContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (UniversalVariables.gameTime == null)
                UniversalVariables.gameTime = gameTime;
            // TODO: Add your update logic here
            manager.Update(gameTime, this.IsActive);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            manager.Draw(gameTime, GraphicsDevice);
            // TODO: Add your drawing code here
            base.Draw(gameTime);
        }
    }
}
