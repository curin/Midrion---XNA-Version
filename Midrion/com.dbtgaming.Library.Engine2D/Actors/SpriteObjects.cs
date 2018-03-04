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
using com.dbtgaming.Library.UI;

namespace com.dbtgaming.Library.Engine2D.Actors
{
    public interface IMovingSprite : IAdvancedSprite2
    {
        float MaxSpeed { get; set; }
        void AllStop(IMovingSpriteInstance instance);
        void MoveTo(Vector2 Location, int FramesToMove, IMovingSpriteInstance instance);
        void Move(Vector2 Direction, int speed, IMovingSpriteInstance instance);
        void Move(Vector2 VelocityMod, IMovingSpriteInstance instance);
    }

    public interface IMovingSpriteInstance : IAdvancedSpriteInstance
    {
        Vector2 MoveTo { get; set; }
        Vector2 Velocity { get; set; }
        Vector2 Deceleration { get; set; }
        float MaxVelocityMod { get; set; }
        new IMovingSprite SpriteBase { get; set; }
    }

    public interface IMultiSprite
    {
        IMovingSprite this[string ID] { get; set; }
        string MultiID { get; set; }

        void PreLoad();
        void Load();
        void PostLoad();
        byte Draw(IMultiSpriteInstance instance);
        byte Update(IMultiSpriteInstance instance);
        void PreUnload();
        void Unload();
        void PostUnload();
    }

    public interface IMultiSpriteInstance
    {
        string InstanceName { get; set; }
        Vector2 Location { get; set; }
        Color OverlayColor { get; set; }
        Vector2 Origin { get; set; }
        float Rotation { get; set; }
        Vector2 Scale { get; set; }
        bool Drawing { get; set; }
        IMovingSpriteInstance this[string ID] { get; set; }
        List<IMovingSpriteInstance> Sprites { get; set; }
        IMultiSprite SpriteBase { get; set; }
    }
}
