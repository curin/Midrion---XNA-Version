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
using com.dbtgaming.Game.Library.Items;

namespace com.dbtgaming.Game.Library.EntityProperties
{
    public class CharacterInventory : IInventory
    {
        Dictionary<Vector2, IItem> _items;
        public Dictionary<Vector2, IItem> Items 
        { 
            get
            {
                return _items;
            }
            set
            {
                _items = value;
            }
        }

        public void Draw()
        {

        }

        Vector2 _size;
        public Vector2 Size 
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
            }
        }
    }
}
