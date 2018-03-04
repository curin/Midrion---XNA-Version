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
    /// <summary>
    /// A Registry for ISprite and IAdvancedSprite
    /// </summary>
    public class BasicSpriteRegistry
    {
        protected static Dictionary<string, ISprite2> _sprites = new Dictionary<string, ISprite2>();
        public static Dictionary<string, ISprite2> Sprites
        {
            get
            {
                return _sprites;
            }
            set
            {
                _sprites = value;
            }
        }

        protected static Dictionary<string, ISpriteInstance> _instances = new Dictionary<string, ISpriteInstance>();
        public static Dictionary<string, ISpriteInstance> Instances
        {
            get
            {
                return _instances;
            }
            set
            {
                _instances = value;
            }
        }

        protected static Dictionary<string, ISpriteTextInstance> _textInstances = new Dictionary<string, ISpriteTextInstance>();
        public static Dictionary<string, ISpriteTextInstance> TextInstances
        {
            get
            {
                return _textInstances;
            }
            set
            {
                _textInstances = value;
            }
        }
        
        protected static Dictionary<string, ISpriteText> _text = new Dictionary<string, ISpriteText>();
        public static Dictionary<string, ISpriteText> Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }

        protected static Rectangle _advancedSource = new Rectangle(0,0,0,0);
        public static Rectangle AdvancedSource
        {
            get
            {
                return _advancedSource;
            }
            set
            {
                _advancedSource = value;
            }
        }

        protected static Rectangle _colSource = new Rectangle(0, 0, 0, 0);
        public static Rectangle CollisionSource
        {
            get
            {
                return _colSource;
            }
            set
            {
                _colSource = value;
            }
        }

        protected static Rectangle _colSource2 = new Rectangle(0, 0, 0, 0);
        public static Rectangle CollisionSource2
        {
            get
            {
                return _colSource2;
            }
            set
            {
                _colSource2 = value;
            }
        }

        protected static Texture2D _colTexture;
        public static Texture2D CollisionTexture
        {
            get
            {
                return _colTexture;
            }
            set
            {
                _colTexture = value;
            }
        }

        protected static Texture2D _colTexture2;
        public static Texture2D CollisionTexture2
        {
            get
            {
                return _colTexture2;
            }
            set
            {
                _colTexture2 = value;
            }
        }

        protected static Rectangle _drawRect = new Rectangle(0, 0, 0, 0);
        public static Rectangle DrawRectangle
        {
            get
            {
                return _drawRect;
            }
            set
            {
                _drawRect = value;
            }
        }

        protected static Rectangle _colRect = new Rectangle(0, 0, 0, 0);
        public static Rectangle CollisionRectangle
        {
            get
            {
                return _colRect;
            }
            set
            {
                _colRect = value;
            }
        }

        protected static Color[] _colColors;
        public static Color[] CollisionColors
        {
            get
            {
                return _colColors;
            }
            set
            {
                _colColors = value;
            }
        }

        protected static Color[] _colColors2;
        public static Color[] CollisionColors2
        {
            get
            {
                return _colColors2;
            }
            set
            {
                _colColors2 = value;
            }
        }

        private static bool _higherLevel = false;
        public static bool HigherLevel
        {
            get
            {
                return _higherLevel;
            }
            set
            {
                _higherLevel = value;
            }
        }

        protected static float _gameFramePerAnimFrame;

        protected static IAdvancedSpriteInstance _advancedInstance;
        protected static IAdvancedSpriteInstance _colInstance;

        public static void Add(ISpriteText text, ContentManager content)
        {
            text.PreLoad();
            text.Load(content);
            text.PostLoad();
            _text.Add(text.TextID, text);
        }

        public static void LoadText(string TextID, ContentManager content)
        {
            _text[TextID].PreLoad();
            _text[TextID].Load(content);
            _text[TextID].PostLoad();
        }

        public static void UnloadText(string TextID)
        {
            _text[TextID].PreUnload();
            _text[TextID].Unload();
            _text[TextID].PostUnload();
        }

        public static void LoadAllText(ContentManager content)
        {
            foreach (ISpriteText text in _text.Values)
            {
                text.PreLoad();
                text.Load(content);
                text.PostLoad();
            }
        }

        public static void UnloadAllText()
        {
            foreach (ISpriteText text in _text.Values)
            {
                text.PreUnload();
                text.Unload();
                text.PostUnload();
            }
        }

        public static void AddInstance(ISpriteTextInstance instance)
        {
            if (!_text.ContainsValue(instance.TextBase))
                _textInstances.Add(instance.InstanceID, instance);
        }

        public static void Add(ISprite2 sprite, ContentManager content)
        {
            sprite.PreLoad();
            sprite.Load(content);
            sprite.PostLoad();
            _sprites.Add(sprite.SpriteID, sprite);
        }

        public static void Load(string spriteID, ContentManager content)
        {
            _sprites[spriteID].PreLoad();
            _sprites[spriteID].Load(content);
            _sprites[spriteID].PostLoad();
        }

        public static void LoadAllSprites(ContentManager content)
        {
            foreach (ISprite2 sprite in _sprites.Values)
            {
                sprite.PreLoad();
                sprite.Load(content);
                sprite.PostLoad();
            }
        }

        public static void UnLoad(string spriteID)
        {
            _sprites[spriteID].PreUnload();
            _sprites[spriteID].Unload();
            _sprites[spriteID].PostUnload();
        }

        public static void UnLoadAll()
        {
            foreach (ISprite2 sprite in _sprites.Values)
            {
                sprite.PreUnload();
                sprite.Unload();
                sprite.PostUnload();
            }
        }

        public static bool CheckCollision(string instancename1, string instancename2, bool AllowTranslucent1, bool AllowTranslucent2)
        {
            _drawRect = new Rectangle((int)_instances[instancename1].Location.X, (int)(_instances[instancename1].Location.Y), (int)(_instances[instancename1].SpriteBase.SourceRect.Width * _instances[instancename1].Scale.X), (int)(_instances[instancename1].SpriteBase.SourceRect.Height * _instances[instancename1].Scale.Y));
            _colInstance = _instances[instancename1] as IAdvancedSpriteInstance;
            if (_colInstance != null)
            {
                _drawRect.Width = (int)(_colInstance.Animation.StartingSource.Width * _colInstance.Scale.X);
                _drawRect.Height = (int)(_colInstance.Animation.StartingSource.Height * _colInstance.Scale.Y);
            }
            _colRect = new Rectangle((int)_instances[instancename2].Location.X, (int)(_instances[instancename2].Location.Y), (int)(_instances[instancename2].SpriteBase.SourceRect.Width * _instances[instancename2].Scale.X), (int)(_instances[instancename2].SpriteBase.SourceRect.Height * _instances[instancename2].Scale.Y));
            _colInstance = _instances[instancename2] as IAdvancedSpriteInstance;
            if (_colInstance != null)
            {
                _colRect.Width = (int)(_colInstance.Animation.StartingSource.Width * _colInstance.Scale.X);
                _colRect.Height = (int)(_colInstance.Animation.StartingSource.Height * _colInstance.Scale.Y);
            }
            if (_drawRect.Intersects(_colRect))
            {
                _colSource2 = _instances[instancename2].SpriteBase.CollisonSource;
                _colSource = _instances[instancename1].SpriteBase.CollisonSource;
                _colTexture = _instances[instancename1].SpriteBase.SpriteSheet;
                _colTexture2 = _instances[instancename2].SpriteBase.SpriteSheet;
                _colInstance = _instances[instancename1] as IAdvancedSpriteInstance;
                if (_colInstance != null)
                {
                    _colSource = new Rectangle(_colInstance.Animation.StartCollision.X + (_colInstance.Animation.StartCollision.Width * _colInstance.Frame), _colInstance.Animation.StartCollision.Y, _colInstance.Animation.StartCollision.Width, _colInstance.Animation.StartCollision.Height);
                    _colTexture = _colInstance.Animation.SpriteSheet;
                }
                _colInstance = _instances[instancename1] as IAdvancedSpriteInstance;
                if (_colInstance != null)
                {
                    _colSource2 = new Rectangle(_colInstance.Animation.StartCollision.X + (_colInstance.Animation.StartCollision.Width * _colInstance.Frame), _colInstance.Animation.StartCollision.Y, _colInstance.Animation.StartCollision.Width, _colInstance.Animation.StartCollision.Height);
                    _colTexture2 = _colInstance.Animation.SpriteSheet;
                }

                _colColors = SpriteUtilities.GetColColors(_colTexture, _colSource);
                _colColors2 = SpriteUtilities.GetColColors(_colTexture2, _colSource2);

                return SpriteUtilities.SpriteCollisionPixel(_instances[instancename1], _instances[instancename2], _colColors, _colColors2, _colSource, _colSource2, AllowTranslucent1, AllowTranslucent2);
            }
            return false;
        }

        public static void AddInstance(ISpriteInstance instance)
        {
            if (_sprites.ContainsValue(instance.SpriteBase))
                _instances.Add(instance.InstanceName, instance);
            else
                throw new Exception("Missing Instance Base Sprite");
        }

        public static void Update()
        {
            if (!_higherLevel)
            {
                foreach (ISpriteInstance instance in _instances.Values)
                {
                    _advancedInstance = instance as IAdvancedSpriteInstance;
                    if (_advancedInstance != null)
                    {
                        if (_advancedInstance.Updating)
                        {
                            if (_advancedInstance.SpriteBase.Update(_advancedInstance) == 0)
                            {
                                if (_advancedInstance.Speed <= 0)
                                    throw new Exception("Instance: " + _advancedInstance.InstanceName + "'s speed must be greater than 0");
                                else
                                {
                                    _gameFramePerAnimFrame = (1 / _advancedInstance.Speed) * (1 / _advancedInstance.Animation.BaseSpeed);
                                }

                                if (_gameFramePerAnimFrame < 0)
                                    throw new Exception("Animation: " + _advancedInstance.Animation.AnimationName + " from Sprite:" + _advancedInstance.SpriteBase.SpriteID + " must have a speed greater than 0");
                                else if (_gameFramePerAnimFrame > 1)
                                {
                                    if (_advancedInstance.InternalFrame >= _gameFramePerAnimFrame)
                                    {
                                        _advancedInstance.InternalFrame = 0;
                                        _advancedInstance.Frame++;
                                    }
                                    else
                                        _advancedInstance.InternalFrame++;
                                    if (_advancedInstance.Frame >= _advancedInstance.Animation.FrameCount)
                                        _advancedInstance.Frame = 0;
                                }
                                else
                                {
                                    _gameFramePerAnimFrame = 1 / _gameFramePerAnimFrame;
                                    _advancedInstance.Frame += (int)_gameFramePerAnimFrame;
                                    if (_advancedInstance.Frame >= _advancedInstance.Animation.FrameCount)
                                        _advancedInstance.Frame = 0;
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void Draw()
        {
            foreach (ISpriteInstance instance in _instances.Values)
            {
                
                if (instance.Drawing)
                {
                    _drawRect = new Rectangle((int)instance.Location.X, (int)(instance.Location.Y), (int)(instance.SpriteBase.SourceRect.Width * instance.Scale.X), (int)(instance.SpriteBase.SourceRect.Height * instance.Scale.Y));
                    _advancedInstance = instance as IAdvancedSpriteInstance;
                    if (_advancedInstance != null)
                    {
                        if (_advancedInstance.SpriteBase.Animations.ContainsKey(_advancedInstance.CurrentAnimation))
                        {
                            if (_advancedInstance.Animation.SpriteSheet != null)
                            {
                                _advancedSource = _advancedInstance.Animation.StartingSource;
                                _advancedSource.X += _advancedSource.Width * _advancedInstance.Frame;
                                _drawRect.Width = (int)(_advancedSource.Width * instance.Scale.X);
                                _drawRect.Height = (int)(_advancedSource.Height * instance.Scale.Y);
                                if (instance.SpriteBase.Draw(instance, _drawRect) == 0)
                                {
                                    UniversalVariables.spriteBatch.Draw(_advancedInstance.Animation.SpriteSheet, _drawRect, _advancedSource, instance.DrawColor, instance.Rotation, instance.SpriteBase.Origin, instance.DrawEffects, instance.Depth);
                                }
                            }
                        }
                    }
                    else if (instance.SpriteBase.Draw(instance, _drawRect) == 0)
                        if (instance.SpriteBase.SpriteSheet != null)
                        UniversalVariables.spriteBatch.Draw(instance.SpriteBase.SpriteSheet, _drawRect, instance.SpriteBase.SourceRect, instance.DrawColor, instance.Rotation, instance.SpriteBase.Origin, instance.DrawEffects, instance.Depth);
                }
            }

            foreach (ISpriteTextInstance instance in _textInstances.Values)
            {
                UniversalVariables.spriteBatch.DrawString(instance.TextBase.Font, instance.Text, instance.Location, instance.DrawColor, instance.Rotation, instance.Origin, instance.Scale, instance.DrawEffects, instance.Depth);
            }
        }
    }
}
