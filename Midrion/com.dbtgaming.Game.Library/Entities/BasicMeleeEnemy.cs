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
using com.dbtgaming.Library.Engine2D.ActorProperties;

namespace com.dbtgaming.Game.Library.Entities
{
    //public class BasicMeleeEnemy : NPC
    //{
    //    public override void Update()
    //    {
    //        base.Update();
    //    }
    //}

    public class BasicMeleeAI: IArtificialIntellegence
    {
        public void Update()
        {

        }

        private Entity _owner;

        public Entity Owner 
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
    }
}
