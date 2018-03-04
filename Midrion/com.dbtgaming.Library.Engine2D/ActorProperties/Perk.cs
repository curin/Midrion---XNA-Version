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

namespace com.dbtgaming.Library.Engine2D.ActorProperties
{
    /// <summary>
    /// Base Class for Perks
    /// </summary>
    public abstract class Perk
    {
        private string _name = "";
        /// <summary>
        /// Name of the Perks
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// Owner of the Perk
        /// </summary>
        protected Entity _owner;

        /// <summary>
        /// Attributes of the Entity that owns this perk
        /// </summary>
        protected List<GameAttribute> _internalAttributes
        {
            get
            {
                return _owner._attributes;
            }
        }

        /// <summary>
        /// Any Additional Modifiers for this Perk
        /// </summary>
        protected List<IModifier> _modifiers = new List<IModifier>();
        public List<IModifier> Modifiers
        {
            get
            {
                return _modifiers;
            }
            set
            {
                _modifiers = value;
            }
        }

        /// <summary>
        /// Update Methods for PErks
        /// </summary>
        public virtual void Update()
        {

        }

        /// <summary>
        /// Draw Method for Perks
        /// </summary>
        public virtual void Draw()
        {

        }
    }

    /// <summary>
    /// Modifiers used in perks
    /// </summary>
    public interface IModifier
    {
        Perk Owner { get; set; }
        void Update();
        int Value { get; set; }
    }

    /// <summary>
    /// A Basic Modifier
    /// </summary>
    public struct Modifier : IModifier
    {
        private Perk _owner;
        public Perk Owner 
        { 
            get
            {
                return _owner;
            }
            set
            {
                _owner = value;
            }
        }
        public void Update() { }
        private int _value;
        public int Value 
        { 
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }
    }
}
