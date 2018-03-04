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
using com.dbtgaming.Library.Engine2D.Collision;
using com.dbtgaming.Library.UI;
using com.dbtgaming.Library.Engine2D.Actors;

namespace com.dbtgaming.Game.Library.Items
{
    public interface IInventoryItem : IAdvancedSprite, ICollider
    {
        string ToolTip { get; set; }
        IItem Owner { get; set; }
        void Draw();
    }

    public interface INonInventoryItem : IAdvancedSprite, ICollider
    {
        IItem Owner { get; set; }
        void DrawDropped();
        void DrawEquipped();
        void UpdateDropped();
        void UpdateEquipped();
    }

    public interface IItem
    {
        bool Unique { get; }
        IInventoryItem InvItem { get; set; }
        INonInventoryItem NonInvItem { get; set; }
        string Name { get; set; }
        void DrawInventory();
        void DrawNonInventory();
        ItemMode Mode { get; set; }
        Vector2 InvLoc { get; }
        int Weight { get; set; }
    }

    public interface IItemAttribute
    {
        string Name { get; set; }
        IItem Owner { get; set; }
        int Value { get; set; }
        void UpdateInventory();
        void UpdateEquipped();
        void UpdateDropped();
        void DrawNonInventory();
        void DrawInventory();
    }

    public enum ItemMode
    {
        Equipped,
        Dropped,
        InInventory
    }
}
