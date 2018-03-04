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
    public class AdvancedSpriteRegistry
    {
        private static IMovingSpriteInstance _movinstance;
        private static Vector2 _tempvect;
        private static IAdvancedSpriteInstance _advancedInstance;
        private static float _gameFramePerAnimFrame;

        public static Dictionary<string, ISprite2> Sprites
        {
            get
            {
                return BasicSpriteRegistry.Sprites;
            }
            set
            {
                BasicSpriteRegistry.Sprites = value;
            }
        }

        public static Dictionary<string, ISpriteInstance> Instances
        {
            get
            {
                return BasicSpriteRegistry.Instances;
            }
            set
            {
                BasicSpriteRegistry.Instances = value;
            }
        }

        public static Dictionary<string, ISpriteTextInstance> TextInstances
        {
            get
            {
                return BasicSpriteRegistry.TextInstances;
            }
            set
            {
                BasicSpriteRegistry.TextInstances = value;
            }
        }

        public static Dictionary<string, ISpriteText> Text
        {
            get
            {
                return BasicSpriteRegistry.Text;
            }
            set
            {
                BasicSpriteRegistry.Text = value;
            }
        }

        public static List< IMultiSpriteInstance> MultiInstances
        {
            get
            {
                return _multiInstances;
            }
            set
            {
                _multiInstances = value;
            }
        }
        private static List<IMultiSpriteInstance> _multiInstances = new List<IMultiSpriteInstance>();

        public static List <IMultiSprite> MultiSprites
        {
            get
            { 
                return _multisprites; 
            }
            set
            {
                _multisprites = value;
            }
        }
        private static List<IMultiSprite> _multisprites = new List<IMultiSprite>();

        public static void Update()
        {
            if (!BasicSpriteRegistry.HigherLevel)
                BasicSpriteRegistry.HigherLevel = false;
            foreach (ISpriteInstance instance in BasicSpriteRegistry.Instances.Values)
            {
                int update = _advancedInstance.SpriteBase.Update(_advancedInstance);
                _advancedInstance = instance as IAdvancedSpriteInstance;
                if (_advancedInstance != null)
                {
                    if (_advancedInstance.Updating)
                    {
                        if (update == 0)
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
                            }
                            else
                            {
                                _gameFramePerAnimFrame = 1 / _gameFramePerAnimFrame;
                                _advancedInstance.Frame += (int)_gameFramePerAnimFrame;
                            }
                        }

                        _movinstance = instance as IMovingSpriteInstance;
                        if (_movinstance != null)
                        {
                            if (update == 0)
                            {
                                _movinstance.Location += _movinstance.Velocity;
                                _tempvect = _movinstance.Velocity;
                                if (_movinstance.Velocity.X > 0)
                                    if (_movinstance.Velocity.X >= _movinstance.Deceleration.X)
                                        _tempvect.X -= _movinstance.Deceleration.X;
                                    else
                                        _tempvect.X = 0;
                                if (_movinstance.Velocity.X < 0)
                                    if (_movinstance.Velocity.X <= -_movinstance.Deceleration.X)
                                        _tempvect.X += _movinstance.Deceleration.X;
                                    else
                                        _tempvect.X = 0;
                                if (_movinstance.Velocity.Y > 0)
                                    if (_movinstance.Velocity.Y >= _movinstance.Deceleration.X)
                                        _tempvect.Y -= _movinstance.Deceleration.Y;
                                    else
                                        _tempvect.Y = 0;
                                if (_movinstance.Velocity.Y < 0)
                                    if (_movinstance.Velocity.Y <= -_movinstance.Deceleration.Y)
                                        _tempvect.Y += _movinstance.Deceleration.Y;
                                    else
                                        _tempvect.Y = 0;

                                _movinstance.Velocity = _tempvect;
                            }
                        }
                    }
                }
                
                
            }

            foreach (IMultiSpriteInstance _mulInstance in _multiInstances)
            {
                foreach (IMovingSpriteInstance inst in _mulInstance.Sprites)
                {
                    int update = _advancedInstance.SpriteBase.Update(inst);

                    if (_advancedInstance.Updating)
                    {
                        if (update == 0)
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

                        if (update == 0)
                        {
                            _movinstance.Location += _movinstance.Velocity;
                            _tempvect = _movinstance.Velocity;
                            if (_movinstance.Velocity.X > 0)
                                if (_movinstance.Velocity.X >= _movinstance.Deceleration.X)
                                    _tempvect.X -= _movinstance.Deceleration.X;
                                else
                                    _tempvect.X = 0;
                            if (_movinstance.Velocity.X < 0)
                                if (_movinstance.Velocity.X <= -_movinstance.Deceleration.X)
                                    _tempvect.X += _movinstance.Deceleration.X;
                                else
                                    _tempvect.X = 0;
                            if (_movinstance.Velocity.Y > 0)
                                if (_movinstance.Velocity.Y >= _movinstance.Deceleration.X)
                                    _tempvect.Y -= _movinstance.Deceleration.Y;
                                else
                                    _tempvect.Y = 0;
                            if (_movinstance.Velocity.Y < 0)
                                if (_movinstance.Velocity.Y <= -_movinstance.Deceleration.Y)
                                    _tempvect.Y += _movinstance.Deceleration.Y;
                                else
                                    _tempvect.Y = 0;

                            _movinstance.Velocity = _tempvect;
                        }
                    }
                }
            }
        }


        protected static Rectangle _advancedSource = new Rectangle(0, 0, 0, 0);
        public static Rectangle AdvancedSource
        {
            get
            {
                return BasicSpriteRegistry.AdvancedSource;
            }
            set
            {
                BasicSpriteRegistry.AdvancedSource = value;
            }
        }

        protected static Rectangle _drawRect = new Rectangle(0, 0, 0, 0);
        public static Rectangle DrawRectangle
        {
            get
            {
                return BasicSpriteRegistry.DrawRectangle;
            }
            set
            {
                BasicSpriteRegistry.DrawRectangle = value;
            }
        }

        public static void Draw()
        {
            BasicSpriteRegistry.Draw();

            foreach (IMultiSpriteInstance inst in _multiInstances)
            {
                foreach (IMovingSpriteInstance movinst in inst.Sprites)
                {
                    if (movinst.Drawing)
                    {
                        _advancedSource = movinst.Animation.StartingSource;
                        _advancedSource.X += _advancedSource.Width * movinst.Frame;
                        _drawRect = new Rectangle((int)movinst.Location.X, (int)(movinst.Location.Y), (int)(_advancedSource.Width * movinst.Scale.X), (int)(_advancedSource.Height * movinst.Scale.Y));
                        if (movinst.SpriteBase.Draw(movinst, _drawRect) == 0)
                        {
                            UniversalVariables.spriteBatch.Draw(_advancedInstance.Animation.SpriteSheet, _drawRect, _advancedSource, movinst.DrawColor, movinst.Rotation, movinst.SpriteBase.Origin, movinst.DrawEffects, movinst.Depth);
                        }
                    }
                }
            }
        }

        public static void Add(ISpriteText text, ContentManager content)
        {
            BasicSpriteRegistry.Add(text, content);
        }

        public static void LoadText(string TextID, ContentManager content)
        {
            BasicSpriteRegistry.LoadText(TextID, content);
        }

        public static void UnloadText(string TextID)
        {
            BasicSpriteRegistry.UnloadText(TextID);
        }

        public static void LoadAllText(ContentManager content)
        {
            BasicSpriteRegistry.LoadAllText(content);
        }

        public static void UnloadAllText()
        {
            BasicSpriteRegistry.UnloadAllText();
        }

        public static void AddInstance(ISpriteTextInstance instance)
        {
            BasicSpriteRegistry.AddInstance(instance);
        }

        public static void Add(ISprite2 sprite, ContentManager content)
        {
            BasicSpriteRegistry.Add(sprite, content);
        }

        public static void Load(string spriteID, ContentManager content)
        {
            BasicSpriteRegistry.Load(spriteID, content);
        }

        public static void LoadAllSprites(ContentManager content)
        {
            BasicSpriteRegistry.LoadAllSprites(content);
        }

        public static void UnLoad(string spriteID)
        {
            BasicSpriteRegistry.UnLoad(spriteID);
        }

        public static void UnLoadAll()
        {
            BasicSpriteRegistry.UnLoadAll();
        }

        public static bool CheckCollision(string instancename1, string instancename2, bool AllowTranslucent1, bool AllowTranslucent2)
        {
            return BasicSpriteRegistry.CheckCollision(instancename1, instancename2, AllowTranslucent1, AllowTranslucent2);
        }

        public static void AddInstance(ISpriteInstance instance)
        {
            BasicSpriteRegistry.AddInstance(instance);
        }
    }
}
