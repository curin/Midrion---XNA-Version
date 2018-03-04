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
using Skill = com.dbtgaming.Library.Engine2D.ActorProperties.Perk;
using com.dbtgaming.Library.Engine2D.Actors;

namespace com.dbtgaming.Library.Engine2D.ActorProperties
{
    /// <summary>
    /// Interface for anything that has health
    /// </summary>
    public interface ILiving
    {
        int Health { get; set; }
        //Dictionary<string, int> Resistances { get; set; }
    }

    /// <summary>
    /// Interface for anything that has Perks
    /// </summary>
    public interface ISkilled : IAttributable
    {
        List<Perk> Perks { get; set; }
    }

    /// <summary>
    /// interface for anything with magic potential
    /// </summary>
    public interface ICaster
    {
        int MagicalPotential { get; set; }
        int Taint { get; set; }
        float MagicFatigue { get; set; }
    }

    /// <summary>
    /// Interface for anything that has attributes
    /// </summary>
    public interface IAttributable
    {
        List<GameAttribute> Attributes { get; set; }
    }

    /// <summary>
    /// Interface for anything that gains expirence
    /// </summary>
    public interface IExpirenced
    {
        int Expirence { get; set; }
        int Level { get; set; }
        void LevelUp();
    }

    /// <summary>
    /// Interface for anything that uses an artificial intellegence
    /// </summary>
    public interface IIntellegent
    {
        IArtificialIntellegence Intellect { get; set; }
    }

    /// <summary>
    /// Interface used in all Artificial Intellegence
    /// </summary>
    public interface IArtificialIntellegence
    {
        void Update();
        Entity Owner { get; set; }
    }

    /// <summary>
    /// Interface for anything that can hit an ILiving
    /// </summary>
    public interface IBattleReady
    {
        void Attack(ILiving victim);
    }
}
