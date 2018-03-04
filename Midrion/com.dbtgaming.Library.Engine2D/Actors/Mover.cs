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
using com.dbtgaming.Library.Engine2D.Collision;

namespace com.dbtgaming.Library.Engine2D.Actors
{
    public class Mover : CollidingAdvancedSprite
    {
        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="drawing">is this sprite drawing?</param>
        /// <param name="spriteBatch">spritebatch from game class</param>
        /// <param name="location">sprite location on screen</param>
        /// <param name="tex">texture</param>
        /// <param name="col">color overlay for texture</param>
        /// <param name="effects">spriteeffects to be applied at draw</param>
        /// <param name="rotation">sprite rotation</param>
        /// <param name="rotationOrigin">Origin of Rotation</param>
        /// <param name="depth">depth layer of sprite</param>
        /// <param name="startinganimation">name of starting animation</param>
        /// <param name="startingframe">frame to start on</param>
        /// <param name="updating">is the sprite updating?</param>
        /// <param name="startingAnim">the Animation that is used at start</param>
        ///<param name="deceleration">Decelartion per update</param>
        /// <param name="maxvelocity">Maximum Velocity</param>
        /// <param name="startingVelocity">Velocity at Creation</param>
        public Mover(string name, Vector2 location, Texture2D tex, Color col, 
            SpriteEffects effects, float rotation, Vector2 rotationOrigin, float depth, string startinganimation,
            int startingframe, bool updating, Animation2D startingAnim, bool drawing, 
            Vector2 startingVelocity, Vector2 deceleration, float maxvelocity = float.MaxValue)
            :base(name, location, tex, col, effects, rotation, rotationOrigin, depth, startinganimation, 
            startingframe, updating, startingAnim, drawing)
        {
            _velocity = startingVelocity;
            _deceleration = deceleration;
            _maxVelocity = maxvelocity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="drawing">is this sprite drawing? </param>
        /// <param name="spriteBatch">spritebatch from game class</param>
        /// <param name="location">sprite location on screen</param>
        /// <param name="tex">texture</param>
        /// <param name="col">color overlay for texture</param>
        /// <param name="effects">spriteeffects to be applied at draw</param>
        /// <param name="rotation">sprite rotation</param>
        /// <param name="rotationOrigin">Origin of Rotation</param>
        /// <param name="depth">depth layer of sprite</param>
        /// <param name="startinganimation">name of starting animation</param>
        /// <param name="startingframe">frame to start on</param>
        /// <param name="updating">is the sprite updating?</param>
        /// <param name="animations">Dictionary of animations and string names</param>
        /// <param name="deceleration">Decelartion per update</param>
        /// <param name="maxvelocity">Maximum Velocity</param>
        /// <param name="startingVelocity">Velocity at Creation</param>
        public Mover(string name, Vector2 location, Texture2D tex, Color col,
            SpriteEffects effects, float rotation, Vector2 rotationOrigin, float depth, string startinganimation,
            int startingframe, bool updating, Dictionary<string, Animation2D> animations, bool drawing, 
            Vector2 startingVelocity, Vector2 deceleration, float maxvelocity = float.MaxValue)
            : base(name, location, tex, col, effects, rotation, rotationOrigin, depth, startinganimation,
             startingframe, updating, animations, drawing)
        {
            _velocity = startingVelocity;
            _deceleration = deceleration;
            _maxVelocity = maxvelocity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="drawing">is this sprite drawing?</param>
        /// <param name="spriteBatch">spritebatch from game class</param>
        /// <param name="location">sprite location on screen</param>
        /// <param name="tex">texture</param>
        /// <param name="col">color overlay for texture</param>
        /// <param name="effects">spriteeffects to be applied at draw</param>
        /// <param name="rotation">sprite rotation</param>
        /// <param name="rotationOrigin">Origin of Rotation</param>
        /// <param name="depth">depth layer of sprite</param>
        /// <param name="startinganimation">name of starting animation</param>
        /// <param name="startingframe">frame to start on</param>
        /// <param name="updating">is the sprite updating?</param>
        /// <param name="startingAnim">the Animation that is used at start</param>
        /// <param name="maxvelocity">Maximum Velocity</param>
        /// <param name="startingVector">Vector to be used int Deceleration or Velocity</param>
        /// <param name="velocity">Is the Starting Vector for Velocity?</param>
        public Mover(string name, Vector2 location, Texture2D tex, Color col,
            SpriteEffects effects, float rotation, Vector2 rotationOrigin, float depth, string startinganimation,
            int startingframe, bool updating, Animation2D startingAnim, bool drawing,
            Vector2 startingVector, float maxvelocity = float.MaxValue, bool velocity = true)
            : base(name, location, tex, col, effects, rotation, rotationOrigin, depth, startinganimation,
            startingframe, updating, startingAnim, drawing)
        {
            if (velocity)
            {
                _velocity = startingVector;
                _deceleration = Vector2.One;
                _maxVelocity = maxvelocity;
            }
            else
            {
                _velocity = Vector2.Zero;
                _deceleration = startingVector;
                _maxVelocity = maxvelocity;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="drawing">is this sprite drawing? </param>
        /// <param name="spriteBatch">spritebatch from game class</param>
        /// <param name="location">sprite location on screen</param>
        /// <param name="tex">texture</param>
        /// <param name="col">color overlay for texture</param>
        /// <param name="effects">spriteeffects to be applied at draw</param>
        /// <param name="rotation">sprite rotation</param>
        /// <param name="rotationOrigin">Origin of Rotation</param>
        /// <param name="depth">depth layer of sprite</param>
        /// <param name="startinganimation">name of starting animation</param>
        /// <param name="startingframe">frame to start on</param>
        /// <param name="updating">is the sprite updating?</param>
        /// <param name="animations">Dictionary of animations and string names</param>
        /// <param name="maxvelocity">Maximum Velocity</param>
        /// <param name="startingVector">Vector to be used int Deceleration or Velocity</param>
        /// <param name="velocity">Is the Starting Vector for Velocity?</param>
        public Mover(string name, Vector2 location, Texture2D tex, Color col,
            SpriteEffects effects, float rotation, Vector2 rotationOrigin, float depth, string startinganimation,
            int startingframe, bool updating, Dictionary<string, Animation2D> animations, bool drawing,
            Vector2 startingVector, float maxvelocity = float.MaxValue, bool velocity = true)
            : base(name, location, tex, col, effects, rotation, rotationOrigin, depth, startinganimation,
             startingframe, updating, animations, drawing)
        {
            if (velocity)
            {
                _velocity = startingVector;
                _deceleration = Vector2.One;
                _maxVelocity = maxvelocity;
            }
            else
            {
                _velocity = Vector2.Zero;
                _deceleration = startingVector;
                _maxVelocity = maxvelocity;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="drawing">is this sprite drawing?</param>
        /// <param name="spriteBatch">spritebatch from game class</param>
        /// <param name="location">sprite location on screen</param>
        /// <param name="tex">texture</param>
        /// <param name="col">color overlay for texture</param>
        /// <param name="effects">spriteeffects to be applied at draw</param>
        /// <param name="rotation">sprite rotation</param>
        /// <param name="rotationOrigin">Origin of Rotation</param>
        /// <param name="depth">depth layer of sprite</param>
        /// <param name="startinganimation">name of starting animation</param>
        /// <param name="startingframe">frame to start on</param>
        /// <param name="updating">is the sprite updating?</param>
        /// <param name="startingAnim">the Animation that is used at start</param>
        /// <param name="maxvelocity">Maximum Velocity</param>
        /// <param name="startingVelocity">Velocity at Creation</param>
        public Mover(string name, Vector2 location, Texture2D tex, Color col,
            SpriteEffects effects, float rotation, Vector2 rotationOrigin, float depth, string startinganimation,
            int startingframe, bool updating, Animation2D startingAnim, bool drawing, float maxvelocity = float.MaxValue)
            : base(name, location, tex, col, effects, rotation, rotationOrigin, depth, startinganimation,
            startingframe, updating, startingAnim, drawing)
        {
            _velocity = Vector2.Zero;
            _deceleration = Vector2.One;
            _maxVelocity = maxvelocity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="drawing">is this sprite drawing? </param>
        /// <param name="spriteBatch">spritebatch from game class</param>
        /// <param name="location">sprite location on screen</param>
        /// <param name="tex">texture</param>
        /// <param name="col">color overlay for texture</param>
        /// <param name="effects">spriteeffects to be applied at draw</param>
        /// <param name="rotation">sprite rotation</param>
        /// <param name="rotationOrigin">Origin of Rotation</param>
        /// <param name="depth">depth layer of sprite</param>
        /// <param name="startinganimation">name of starting animation</param>
        /// <param name="startingframe">frame to start on</param>
        /// <param name="updating">is the sprite updating?</param>
        /// <param name="animations">Dictionary of animations and string names</param>
        /// <param name="maxvelocity">Maximum Velocity</param>
        /// <param name="startingVelocity">Velocity at Creation</param>
        public Mover(string name, Vector2 location, Texture2D tex, Color col,
            SpriteEffects effects, float rotation, Vector2 rotationOrigin, float depth, string startinganimation,
            int startingframe, bool updating, Dictionary<string, Animation2D> animations, bool drawing, 
            float maxvelocity = float.MaxValue)
            : base(name, location, tex, col, effects, rotation, rotationOrigin, depth, startinganimation,
             startingframe, updating, animations, drawing)
        {
            _velocity = Vector2.Zero;
            _deceleration = Vector2.One;
            _maxVelocity = maxvelocity;
        }
        #endregion

        protected Vector2 _velocity = new Vector2(0, 0);
        /// <summary>
        /// How quickly the sprite moves per frame
        /// </summary>
        public Vector2 Velocity
        {
            get
            {
                return _velocity;
            }
            set
            {
                _velocity = value;
            }
        }

        protected Vector2 _deceleration = new Vector2(1f, 1f);
        /// <summary>
        /// Vector for how quickly velocity degrades
        /// </summary>
        public Vector2 Deceleration
        {
            get
            {
                return _deceleration;
            }
            set
            {
                _deceleration = value;
            }
        }

        /// <summary>
        /// Max velocity of your Mover
        /// </summary>
        protected float _maxVelocity;
        protected float _maxVelocityMod = 0f;
        /// <summary>
        /// Public Max Velocity Mod
        /// </summary>
        public float MaxVelocityMod
        {
            get
            {
                return _maxVelocityMod;
            }
            set
            {
                _maxVelocityMod = value;
            }
        }

        public override void Update()
        {
            //Causes derpy movements with rotations
            _location += new Vector2(_velocity.X * (float)Math.Cos(_rotation) + _velocity.Y * (float)Math.Sin(_rotation), _velocity.Y * (float)Math.Cos(_rotation) + _velocity.X * (float)Math.Sin(_rotation));
            if (Velocity.X > 0)
                _velocity.X -= _deceleration.X;
            if (Velocity.X < 0)
                _velocity.X += _deceleration.X;
            if (Velocity.Y > 0)
                _velocity.Y -= _deceleration.Y;
            if (Velocity.Y < 0)
                _velocity.Y += _deceleration.Y;
            //Make sure that we don't exceed Max Velocity in either direction
            if (Velocity.X > _maxVelocity + _maxVelocityMod)
                _velocity.X = _maxVelocity + _maxVelocityMod;
            if (Velocity.Y > _maxVelocity + _maxVelocityMod)
                _velocity.Y = _maxVelocity + _maxVelocityMod;
            if (Velocity.X < -_maxVelocity - _maxVelocityMod)
                _velocity.X = -_maxVelocity - _maxVelocityMod;
            if (Velocity.Y < -_maxVelocity - _maxVelocityMod)
                _velocity.Y = -_maxVelocity - _maxVelocityMod;
            base.Update();
        }

        /// <summary>
        /// Increases the Velocity of the sprite
        /// </summary>
        /// <param name="MoveVector">Increase vector for velocity</param>
        public virtual void Move(Vector2 MoveVector)
        {
            Velocity += MoveVector;
        }

        /// <summary>
        /// Move to Location
        /// </summary>
        /// <param name="Location">Location to Move to</param>
        public virtual void MoveTo(Vector2 Location)
        {
            _location = Location;
        }

        public void AllStop()
        {
            _velocity = Vector2.Zero;
        }

        public new void Dispose()
        {
            base.Dispose();
        }
    }

    public interface IMover
    {
        void AllStop();
        void MoveTo();
        void Move();
        Vector2 MaxVelocityMod { get; set; }
        Vector2 Deceleration { get; set; }
        Vector2 Velocity { get; set; }
    }
}
