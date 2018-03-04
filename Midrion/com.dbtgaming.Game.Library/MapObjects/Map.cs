using com.dbtgaming.Library;
using com.dbtgaming.Library.Engine2D.Collision;
using com.dbtgaming.Library.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace com.dbtgaming.Game.Library.MapObjects
{
    public class Map
    {
        public Map(SpriteBatch batch)
        {
            spriteBatch = batch;
        }

        SpriteBatch spriteBatch;
        Rectangle tempdrawrect;

        Dictionary<Vector2, Tile> _tiles = new Dictionary<Vector2, Tile>();
        public Dictionary<Vector2, Tile> Tiles
        {
            get
            {
                return _tiles;
            }
            set
            {
                _tiles = value;
            }
        }

        public virtual void DrawTile(ICamera Camera, Tile tile, Vector2 Location)
        {

            tempdrawrect = new Rectangle((int)Location.X, (int)Location.Y, tile.SourceRect.Width, tile.SourceRect.Height);
            if (Utilities.Vector2ToRect(Camera.Location, Camera.Viewport.Width, Camera.Viewport.Height).Intersects(tempdrawrect))
            {
                tempdrawrect.X -= (int)Camera.Location.X;
                tempdrawrect.Y -= (int)Camera.Location.Y;
                spriteBatch.Draw(tile.SpriteSheet, tempdrawrect, tile.SourceRect, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            }
        }

        public virtual void Draw(ICamera Camera)
        {
            foreach (Vector2 loc in Tiles.Keys)
            {
                DrawTile(Camera, Tiles[loc], loc);
            }
        }
    }

    public class Tile
    {
        Texture2D _spriteSheet;
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

        Rectangle _sourceRect;
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

        List<ICollider> _barriers;
        public List<ICollider> Barriers
        {
            get
            {
                return _barriers;
            }
            set
            {
                _barriers = value;
            }
        }
    }
}
