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
using com.dbtgaming.Library.Engine2D.Actors;

namespace com.dbtgaming.Game.Library.Magic
{
    public class SpellModifier
    {
    }

    public interface ISpellModifier
    {

    }

    public interface IAreaOfEffect
    {
        bool Intersect(Entity entity);
    }
}
