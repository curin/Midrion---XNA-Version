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
using com.dbtgaming.Library.Engine2D.ActorProperties;
using com.dbtgaming.Library.UI;

namespace com.dbtgaming.Library.Engine2D.Actors
{
    public class NPC : Entity, IIntellegent
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
        /// <param name="deceleration">Decelartion per update</param>
        /// <param name="maxvelocity">Maximum Velocity</param>
        /// <param name="startingVelocity">Velocity at Creation</param>
        /// <param name="expirence">Expirence of Entity</param>
        /// <param name="health">Max Health of Entity</param>
        /// <param name="level">Level of Entity</param>
        /// <param name="attributes">Attributes of Entity (strength, dexterity, etc.)</param>
        /// <param name="perks"> perks of Entity(Abilities)(used for a sort of perk tree)</param>
        /// <param name="magicalPotential">Ability to Cast Spells</param>
        /// <param name="taint"> A Darkness that grows with Void and Dark Spells</param>
        public NPC(string name, Vector2 location, Texture2D tex, Color col, 
            SpriteEffects effects, float rotation, Vector2 rotationOrigin, float depth, string startinganimation,
            int startingframe, bool updating, Animation2D startingAnim, bool drawing,
            Vector2 startingVelocity, Vector2 deceleration, List<GameAttribute> attributes, List<Perk> perks, IArtificialIntellegence AI,
            float maxvelocity = float.MaxValue, int health = 100, int level = 1, int expirence = 0, int taint = 0,
            int magicalPotential = 0)
            :base(name, location, tex, col, effects, rotation, rotationOrigin, depth, startinganimation, 
            startingframe, updating, startingAnim, drawing, startingVelocity, deceleration, attributes, perks,
            maxvelocity, health, level, expirence, taint, magicalPotential)
        {
            _intellect = AI;
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
        /// <param name="expirence">Expirence of Entity</param>
        /// <param name="health">Max Health of Entity</param>
        /// <param name="level">Level of Entity</param>
        /// <param name="attributes">Attributes of Entity (strength, dexterity, etc.)</param>
        /// <param name="perks"> perks of Entity(Abilities)(used for a sort of perk tree)</param>
        /// <param name="magicalPotential">Ability to Cast Spells</param>
        /// <param name="taint"> A Darkness that grows with Void and Dark Spells</param>
        public NPC(string name, Vector2 location, Texture2D tex, Color col,
            SpriteEffects effects, float rotation, Vector2 rotationOrigin, float depth, string startinganimation,
            int startingframe, bool updating, Dictionary<string, Animation2D> animations, bool drawing,
            Vector2 startingVelocity, Vector2 deceleration, List<GameAttribute> attributes, List<Perk> perks, IArtificialIntellegence AI,
            float maxvelocity = float.MaxValue, int health = 100, int level = 1, int expirence = 0, int taint = 0,
            int magicalPotential = 0)
            : base(name, location, tex, col, effects, rotation, rotationOrigin, depth, startinganimation,
             startingframe, updating, animations, drawing, startingVelocity, deceleration, attributes, perks, maxvelocity,
            health, level, expirence, taint, magicalPotential)
        {
            _intellect = AI;
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
        /// <param name="expirence">Expirence of Entity</param>
        /// <param name="health">Max Health of Entity</param>
        /// <param name="level">Level of Entity</param>
        /// <param name="attributes">Attributes of Entity (strength, dexterity, etc.)</param>
        /// <param name="perks"> perks of Entity(Abilities)(used for a sort of perk tree)</param>
        /// <param name="magicalPotential">Ability to Cast Spells</param>
        /// <param name="taint"> A Darkness that grows with Void and Dark Spells</param>
        /// <param name="startingVector">Vector to be used int Deceleration or Velocity</param>
        /// <param name="velocity">Is the Starting Vector for Velocity?</param>
        public NPC(string name, Vector2 location, Texture2D tex, Color col,
            SpriteEffects effects, float rotation, Vector2 rotationOrigin, float depth, string startinganimation,
            int startingframe, bool updating, Animation2D startingAnim, bool drawing,
            Vector2 startingVector, List<GameAttribute> attributes, List<Perk> perks, IArtificialIntellegence AI,
            float maxvelocity = float.MaxValue, int health = 100, int level = 1, int expirence = 0, int taint = 0,
            int magicalPotential = 0, bool velocity = true)
            : base(name, location, tex, col, effects, rotation, rotationOrigin, depth, startinganimation,
            startingframe, updating, startingAnim, drawing, startingVector, attributes, perks,
            maxvelocity, health, level, expirence, taint, magicalPotential, velocity)
        {
            _intellect = AI;
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
        /// <param name="expirence">Expirence of Entity</param>
        /// <param name="health">Max Health of Entity</param>
        /// <param name="level">Level of Entity</param>
        /// <param name="attributes">Attributes of Entity (strength, dexterity, etc.)</param>
        /// <param name="perks"> perks of Entity(Abilities)(used for a sort of perk tree)</param>
        /// <param name="magicalPotential">Ability to Cast Spells</param>
        /// <param name="taint"> A Darkness that grows with Void and Dark Spells</param>
        /// <param name="startingVector">Vector to be used int Deceleration or Velocity</param>
        /// <param name="velocity">Is the Starting Vector for Velocity?</param>
        public NPC(string name, Vector2 location, Texture2D tex, Color col,
            SpriteEffects effects, float rotation, Vector2 rotationOrigin, float depth, string startinganimation,
            int startingframe, bool updating, Dictionary<string, Animation2D> animations, bool drawing,
            Vector2 startingVector, List<GameAttribute> attributes, List<Perk> perks, IArtificialIntellegence AI,
            float maxvelocity = float.MaxValue, int health = 100, int level = 1, int expirence = 0, int taint = 0,
            int magicalPotential = 0, bool velocity = true)
            : base(name, location, tex, col, effects, rotation, rotationOrigin, depth, startinganimation,
             startingframe, updating, animations, drawing, startingVector, attributes, perks,
            maxvelocity, health, level, expirence, taint, magicalPotential, velocity)
        {
            _intellect = AI;
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
        /// <param name="expirence">Expirence of Entity</param>
        /// <param name="health">Max Health of Entity</param>
        /// <param name="level">Level of Entity</param>
        /// <param name="attributes">Attributes of Entity (strength, dexterity, etc.)</param>
        /// <param name="perks"> perks of Entity(Abilities)(used for a sort of perk tree)</param>
        /// <param name="magicalPotential">Ability to Cast Spells</param>
        /// <param name="taint"> A Darkness that grows with Void and Dark Spells</param>
        public NPC(string name, Vector2 location, Texture2D tex, Color col,
            SpriteEffects effects, float rotation, Vector2 rotationOrigin, float depth, string startinganimation,
            int startingframe, bool updating, Animation2D startingAnim, bool drawing,
            List<GameAttribute> attributes, List<Perk> perks, IArtificialIntellegence AI, float maxvelocity = float.MaxValue,
            int health = 100, int level = 1, int expirence = 0, int taint = 0,
            int magicalPotential = 0)
            : base(name, location, tex, col, effects, rotation, rotationOrigin, depth, startinganimation,
            startingframe, updating, startingAnim, drawing, attributes, perks,
            maxvelocity, health, level, expirence, taint, magicalPotential)
        {
            _intellect = AI;
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
        /// <param name="expirence">Expirence of Entity</param>
        /// <param name="health">Max Health of Entity</param>
        /// <param name="level">Level of Entity</param>
        /// <param name="attributes">Attributes of Entity (strength, dexterity, etc.)</param>
        /// <param name="perks"> perks of Entity(Abilities)(used for a sort of perk tree)</param>
        /// <param name="magicalPotential">Ability to Cast Spells</param>
        /// <param name="taint"> A Darkness that grows with Void and Dark Spells</param>
        public NPC(string name, Vector2 location, Texture2D tex, Color col,
            SpriteEffects effects, float rotation, Vector2 rotationOrigin, float depth, string startinganimation,
            int startingframe, bool updating, Dictionary<string, Animation2D> animations, bool drawing,
            List<GameAttribute> attributes, List<Perk> perks, IArtificialIntellegence AI, float maxvelocity = float.MaxValue,
            int health = 100, int level = 1, int expirence = 0, int taint = 0,
            int magicalPotential = 0)
            : base(name, location, tex, col, effects, rotation, rotationOrigin, depth, startinganimation,
             startingframe, updating, animations, drawing, attributes, perks,
            maxvelocity, health, level, expirence, taint, magicalPotential)
        {
            _intellect = AI;
        }
        #endregion

        public IArtificialIntellegence Intellect 
        { 
            get
            {
                return _intellect;
            }
            set
            {
                _intellect = value;
            }
        }
        IArtificialIntellegence _intellect;

        public override void Update()
        {
            base.Update();
            _intellect.Update();
        }
    }
}
