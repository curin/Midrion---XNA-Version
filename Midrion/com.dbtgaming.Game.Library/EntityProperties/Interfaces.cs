using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.dbtgaming.Game.Library.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace com.dbtgaming.Game.Library.EntityProperties
{
    public interface IContainer
    {
        IInventory Inventory { get;}
    }

    public interface IInventory
    {
        Dictionary<Vector2, IItem> Items { get; set; }
        void Draw();
        Vector2 Size { get; set; }
    }
}
