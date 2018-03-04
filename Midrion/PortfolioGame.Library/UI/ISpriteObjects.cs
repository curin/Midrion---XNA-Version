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

namespace com.dbtgaming.Library.UI
{
    public interface ISpriteText
    {
        string TextID { get; set; }
        SpriteFont Font { get; set; }

        void PreLoad();
        void Load(ContentManager content);
        void PostLoad();
        byte Draw(ISpriteInstance instance, Rectangle drawrect);
        void PreUnload();
        void Unload();
        void PostUnload();
    }

    public interface ISpriteTextInstance
    {
        ISpriteText TextBase { get; set; }
        string InstanceID { get; set; }
        Vector2 Location { get; set; }
        Vector2 Scale { get; set; }
        Vector2 Origin { get; set; }
        float Rotation { get; set; }
        Color DrawColor { get; set; }
        string Text { get; set; }
        SpriteEffects DrawEffects { get; set; }
        float Depth { get; set; }

        bool Drawing { get; set; }
    }

    /// <summary>
    /// Basic sprite (no animation, nothing)
    /// </summary>
    public interface ISprite2 : IDisposable
    {
        string SpriteID { get; set; }
        Texture2D SpriteSheet { get; set; }
        Rectangle SourceRect { get; set; }
        Rectangle CollisonSource { get; set; }
        Vector2 Origin { get; set; }

        void PreLoad();
        void Load(ContentManager content);
        void PostLoad();
        byte Draw(ISpriteInstance instance, Rectangle drawrect);
        void PreUnload();
        void Unload();
        void PostUnload();
    }

    /// <summary>
    /// Information supplied to isprite in order to draw individual instances
    /// </summary>
    public interface ISpriteInstance : IDisposable
    {
        string InstanceName { get; set; }
        Vector2 Location { get; set; }
        Vector2 CollisionCorrection { get; set; }
        float Rotation { get; set; }
        Color DrawColor { get; set; }
        SpriteEffects DrawEffects { get; set; }
        float Depth { get; set; }
        Vector2 Scale { get; set; }
        ISprite2 SpriteBase { get; set; }
        bool Drawing { get; set; }
    }

    /// <summary>
    /// More complex sprites with animation
    /// </summary>
    public interface IAdvancedSprite2 : ISprite2
    {
        Dictionary<string, Animation2Data> Animations { get; set; }

        byte Update(IAdvancedSpriteInstance _instance);
    }

    /// <summary>
    /// Data used in Sprites
    /// </summary>
    public interface Animation2Data : IDisposable
    {
        string AnimationName { get; set; }
        float BaseSpeed { get; set; }
        Rectangle StartingSource { get; set; }
        Rectangle StartCollision { get; set; }
        int FrameCount { get; set; }
        Vector2 CollisionCorrection { get; set; }
        Texture2D SpriteSheet { get; set; }
    }

    /// <summary>
    /// instance for an advanced Sprite
    /// </summary>
    public interface IAdvancedSpriteInstance : ISpriteInstance
    {
        Animation2Data Animation { get; set; }
        int InternalFrame { get; set; }
        int Frame { get; set; }
        float Speed { get; set; }
        string CurrentAnimation { get; set; }
        new IAdvancedSprite2 SpriteBase { get; set; }
        bool Updating { get; set; }
    }
}
