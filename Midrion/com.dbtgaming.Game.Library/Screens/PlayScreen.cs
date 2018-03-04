using com.dbtgaming.Game.Library.Entities;
using com.dbtgaming.Game.Library.EntityProperties;
using com.dbtgaming.Game.Library.Magic;
using com.dbtgaming.Game.Library.MapObjects;
using com.dbtgaming.Library;
using com.dbtgaming.Library.Engine2D.ActorProperties;
using com.dbtgaming.Library.Engine2D.Actors;
using com.dbtgaming.Library.GSM;
using com.dbtgaming.Library.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace com.dbtgaming.Game.Library.Screens
{
    public class PlayScreen : Screen
    {
        Character _char;
        Texture2D _charTex;
        Animation2D _anim;
        Texture2D Tiles;
        Map mp;
        Tile tile1;
        MagicManager MagMan;
        CompMouse Mous;
        List<Entity> EntityList = new List<Entity>();
        MagicTile[,] tiles;

        public PlayScreen(ScreenManager manager)
            : base(manager)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            tiles = TestMapGen();
            RegisterKeyBindings();
        }

        public void RegisterKeyBindings()
        {
            InputManager.RegisterKeyBinding("Forward", "Play.Character", new Keys[] { Keys.W }, new string[0], false);
            InputManager.RegisterKeyBinding("Backward", "Play.Character", new Keys[] { Keys.S }, new string[0], false);
            InputManager.RegisterKeyBinding("Left", "Play.Character", new Keys[] { Keys.A }, new string[0], false);
            InputManager.RegisterKeyBinding("Right", "Play.Character", new Keys[] { Keys.D }, new string[0], false);
        }

        public override void LoadContent()
        {
            _charTex = UniversalVariables.content.Load<Texture2D>("SpriteSheet Char");
            _char = new Character("Char", new Vector2(0, 0), _charTex, Color.White, SpriteEffects.None, 0,
                new Vector2(19, 29), .5f, "Standing", 0, true, AddCharAnims(), true, new List<GameAttribute>(), new List<Perk>(),
                new CharacterInventory(), 5f);
            base.LoadContent();
            cam = new GameCam(_char, new Rectangle(250, 0, UniversalVariables.graphics.PreferredBackBufferWidth, UniversalVariables.graphics.PreferredBackBufferHeight));
            _char.CurrentAnimation = "Standing";
            mp = new Map(UniversalVariables.spriteBatch);
            Tiles = UniversalVariables.content.Load<Texture2D>("SpriteSheet Tiles");
            GenerateMap();
            Mous = new CompMouse("mouse", UniversalVariables.content.Load<Texture2D>("SpriteSheet Misc"), CreateMouseAnimation());
            Mous.CurrentAnimation = "neutral";
            MagMan = new MagicManager(UniversalVariables.content.Load<Texture2D>("SpriteSheet Magic"), new Rectangle(0, 14, 6, 6), new Rectangle(0, 0, 12, 12), new Rectangle(0, 12, 2, 2));
            GenerateBaseEntityList();
            Interface.Add(0, Mous);
            MagMan.AddPrimalShapes();
        }

        public void GenerateBaseEntityList()
        {
            EntityList.Add(_char);
        }

        public void GenerateMap()
        {
            tile1 = new Tile();
            tile1.Barriers = new List<dbtgaming.Library.Engine2D.Collision.ICollider>();
            tile1.SourceRect = new Rectangle(0, 0, 512, 512);
            tile1.SpriteSheet = Tiles;
            mp.Tiles.Add(new Vector2(0, 0), tile1);
            tile1 = new Tile();
            tile1.Barriers = new List<dbtgaming.Library.Engine2D.Collision.ICollider>();
            tile1.SourceRect = new Rectangle(512, 0, 512, 512);
            tile1.SpriteSheet = Tiles;
            mp.Tiles.Add(new Vector2(-512, 0), tile1);
        }

        public Animation2D CreateMouseAnimation()
        {
            _anim = new Animation2D();
            _anim.ColCorrection = new Vector2(0, 0);
            _anim.ColSize = new Vector2(13, 19);
            _anim.Frames = 1;
            _anim.GameFramesPerFrame = 1;
            _anim.SpriteSheetColLoc = new Vector2(0, 0);
            _anim.SpriteSheetLoc = new Vector2(0, 0);
            _anim.SpriteSize = new Vector2(13, 19);
            return _anim;
        }

        public Dictionary<string, Animation2D> AddCharAnims()
        {
            Dictionary<string, Animation2D> ret = new Dictionary<string, Animation2D>();
            _anim = new Animation2D();
            _anim.ColCorrection = new Vector2(0, 49);
            _anim.ColSize = new Vector2(37, 9);
            _anim.Frames = 8;
            _anim.GameFramesPerFrame = 10;
            _anim.SpriteSheetColLoc = new Vector2(0, 58);
            _anim.SpriteSheetLoc = new Vector2(0, 0);
            _anim.SpriteSize = new Vector2(37, 58);
            ret.Add("WalkCycle", _anim);
            _anim = new Animation2D();
            _anim.ColCorrection = new Vector2(0, 49);
            _anim.ColSize = new Vector2(37, 9);
            _anim.Frames = 1;
            _anim.GameFramesPerFrame = 10;
            _anim.SpriteSheetColLoc = new Vector2(0, 58);
            _anim.SpriteSheetLoc = new Vector2(0, 0);
            _anim.SpriteSize = new Vector2(37, 58);
            ret.Add("Standing", _anim);
            return ret;
        }

        public override void Update(GameTime gameTime, bool isactive)
        {
            if (isactive)
                _char.InputHandler();
            if (_char.Velocity == Vector2.Zero)
                _char.CurrentAnimation = "Standing";
            else
                _char.CurrentAnimation = "WalkCycle";
            _char.Update();
            Mous.Update((GameCam)cam);
            if (isactive)
                MagMan.InputUpdate(Mous, (GameCam)cam, EntityList);
            MagMan.Update(EntityList, (GameCam)cam);
            cam.Update();
            InputManager.Update();
            base.Update(gameTime, isactive);
        }

        Viewport view;

        public override void Draw2D(GameTime gameTime, GraphicsDevice device)
        {
            view = device.Viewport;
            view.Bounds = cam.Viewport;
            device.Viewport = view;
            mp.Draw(cam);
            _char.Draw(this.cam);
            MagMan.Draw(cam, tiles);
            base.Draw2D(gameTime, device);
        }

        public MagicTile[,] TestMapGen()
        {
            Map2 map = new Map2();
            map.Regions = new Dictionary<string, MapRegion>();
            MapRegion region = new MapRegion();
            region.Size = new Vector2(10000, 10000);
            region.PoPs = new List<PlaceOfPower>();
            PlaceOfPower pop = new PlaceOfPower();
            pop.Location = new Point(100, 100);
            pop.PlaceType = PoPType.Fire;
            pop.Strength = 100;
            region.PoPs.Add(pop);
            pop = new PlaceOfPower();
            pop.Location = new Point(255, 297);
            pop.PlaceType = PoPType.Earth;
            pop.Strength = 249;
            region.PoPs.Add(pop);
            pop = new PlaceOfPower();
            pop.Location = new Point(9235, 2453);
            pop.PlaceType = PoPType.Holy;
            pop.Strength = 106;
            region.PoPs.Add(pop);
            pop = new PlaceOfPower();
            pop.Location = new Point(2131, 2356);
            pop.PlaceType = PoPType.Void;
            pop.Strength = 15;
            region.PoPs.Add(pop);
            pop = new PlaceOfPower();
            pop.Location = new Point(500, 8345);
            pop.PlaceType = PoPType.Water;
            pop.Strength = 204;
            region.PoPs.Add(pop);
            pop = new PlaceOfPower();
            pop.Location = new Point(6546, 235);
            pop.PlaceType = PoPType.Air;
            pop.Strength = 35;
            region.PoPs.Add(pop);
            pop = new PlaceOfPower();
            pop.Location = new Point(2353, 7853);
            pop.PlaceType = PoPType.Neutral;
            pop.Strength = 75;
            region.PoPs.Add(pop);
            pop = new PlaceOfPower();
            pop.Location = new Point(3256, 6574);
            pop.PlaceType = PoPType.Protection;
            pop.Strength = 152;
            region.PoPs.Add(pop);
            pop = new PlaceOfPower();
            pop.Location = new Point(7645, 2455);
            pop.PlaceType = PoPType.Fire;
            pop.Strength = 163;
            region.PoPs.Add(pop);
            pop = new PlaceOfPower();
            pop.Location = new Point(9999, 9888);
            pop.PlaceType = PoPType.Holy;
            pop.Strength = 72;
            region.PoPs.Add(pop);
            pop = new PlaceOfPower();
            pop.Location = new Point(2567, 9987);
            pop.PlaceType = PoPType.Summoning;
            pop.Strength = 185;
            region.PoPs.Add(pop);
            pop = new PlaceOfPower();
            pop.Location = new Point(346, 7686);
            pop.PlaceType = PoPType.Arcane;
            pop.Strength = 235;
            region.PoPs.Add(pop);
            pop = new PlaceOfPower();
            pop.Location = new Point(3216, 9785);
            pop.PlaceType = PoPType.Fire;
            pop.Strength = 36;
            region.PoPs.Add(pop);
            pop = new PlaceOfPower();
            pop.Location = new Point(5002, 5343);
            pop.PlaceType = PoPType.Dimension;
            pop.Strength = 125;
            region.PoPs.Add(pop);
            pop = new PlaceOfPower();
            pop.Location = new Point(6453, 4634);
            pop.PlaceType = PoPType.Darkness;
            pop.Strength = 215;
            region.PoPs.Add(pop);
            map.Regions.Add("test", region);

            MagicTile[,] tiles = MapRegistry.GenerateMagicTiles(map);
            return tiles;
        }
    }
}
