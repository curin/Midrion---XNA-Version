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

namespace com.dbtgaming.Library.Engine2D.Collision
{
    public class CollidingAdvancedSprite : AdvancedSprite, ICollider
    {
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
        public CollidingAdvancedSprite(string name, Vector2 location, Texture2D tex, Color col, 
            SpriteEffects effects, float rotation, Vector2 rotationOrigin, float depth, string startinganimation, 
            int startingframe, bool updating, Animation2D startingAnim, bool drawing)
            :base(name, location, tex, col, effects, rotation, rotationOrigin, depth, startinganimation, 
            startingframe, updating, startingAnim, drawing)
        {
            
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
        public CollidingAdvancedSprite(string name, Vector2 location, Texture2D tex, Color col,
            SpriteEffects effects, float rotation, Vector2 rotationOrigin, float depth, string startinganimation,
            int startingframe, bool updating, Dictionary<string, Animation2D> animations, bool drawing)
            : base(name, location, tex, col, effects, rotation, rotationOrigin, depth, startinganimation,
             startingframe, updating, animations, drawing)
        {
            
        }

        protected Rectangle _boundingBox;
        /// <summary>
        /// This is the basic bounding box for the current frame
        /// </summary>
        public Rectangle BoundingBox
        {
            get
            {
                _boundingBox = tempdrawrect;
                _boundingBox.X -= tempdrawrect.Width / 2;
                _boundingBox.Y -= tempdrawrect.Height / 2;
                return _boundingBox;
            }
        }

        protected Rectangle _colBoundingRectangle;
        /// <summary>
        /// This is the basic bounding box for the current frame
        /// </summary>
        public Rectangle ColBoundingBox
        {
            get
            {
                _colBoundingRectangle = new Rectangle(tempdrawrect.X, tempdrawrect.Y, (int)currentAnim.ColSize.X, (int)currentAnim.ColSize.Y);
                return _colBoundingRectangle;
            }
        }

        internal Matrix _transformMatrix;
        public Matrix TransformMatrix
        {
            get
            {
                return _transformMatrix;
            }
        }

        public override void Update()
        {
            CollisionUtils.CreateCollidingSpriteTransformMatrix(this);
            base.Update();
        }

        private bool _ignoreTranslucentCol = false;
        public bool IgnoreTranslucentCol
        {
            get
            {
                return _ignoreTranslucentCol;
            }
            set
            {
                _ignoreTranslucentCol = value;
            }
        }


        /// <summary>
        /// temp source loc for collision texture
        /// </summary>
        protected Rectangle tempcolsourceloc;
        /// <summary>
        /// current texture used for per pixel collision
        /// </summary>
        public Rectangle CollisionTextureLoc
        {
            get
            {
                tempcolsourceloc = new Rectangle((int)(currentAnim.SpriteSheetColLoc.X + (_frame * currentAnim.ColSize.X)), (int)(currentAnim.SpriteSheetColLoc.Y), (int)currentAnim.ColSize.X, (int)currentAnim.ColSize.Y);
                return tempcolsourceloc;
            }
        }

        public Vector2 CurrentColCorrection
        {
            get
            {
                return currentAnim.ColCorrection;
            }
        }

        internal Rectangle sourcerect
        {
            get
            {
                return tempsourcerect;
            }
        }

        internal Rectangle colltemp;
        internal Color[] Colors;

        public bool FullObjectCollision(CollidingAdvancedSprite CAS)
        {
            CollisionUtils.CreateCollidingSpriteTransformMatrix(CAS);
            CollisionUtils.CreateCollidingSpriteTransformMatrix(this);
            if (Rotation == 0 || Rotation == Math.PI)
            {
                if (Intersects(CAS))
                {
                    CollisionUtils.CreateCollidingSpriteTransformMatrix(CAS);
                    CollisionUtils.CreateCollidingSpriteTransformMatrix(this);
                    CollisionUtils.GetColors(this);
                    CollisionUtils.GetColors(CAS);
                    return CollisionUtils.IntersectPixels(this.TransformMatrix, this.BoundingBox.Width, this.BoundingBox.Height, Colors, this.IgnoreTranslucentCol, CAS.TransformMatrix, CAS.BoundingBox.Width, CAS.BoundingBox.Height, CAS.Colors, CAS.IgnoreTranslucentCol);
                }
            }
            else
            {
                CollisionUtils.CreateCollidingSpriteTransformMatrix(CAS);
                CollisionUtils.CreateCollidingSpriteTransformMatrix(this);
                colltemp = CollisionUtils.CalculateBoundingRectangle(tempdrawrect, _transformMatrix);
                if (colltemp.Intersects(CollisionUtils.CalculateBoundingRectangle(CAS.tempdrawrect, CAS._transformMatrix)))
                {
                    CollisionUtils.GetColors(this);
                    CollisionUtils.GetColors(CAS);
                    return CollisionUtils.IntersectPixels(this.TransformMatrix , this.BoundingBox.Width, this.BoundingBox.Height, Colors, this.IgnoreTranslucentCol, CAS.TransformMatrix , CAS.BoundingBox.Width, CAS.BoundingBox.Height, CAS.Colors, CAS.IgnoreTranslucentCol);
                }
            }
            return false;
        }

        public bool CollisionDetection(CollidingAdvancedSprite CAS)
        {
            CollisionUtils.CreateCollidingSpriteTransformMatrix(CAS);
            CollisionUtils.CreateCollidingSpriteTransformMatrix(this);
            if (Rotation == 0 || Rotation == Math.PI)
            {
                if (Intersects(CAS))
                {
                    CollisionUtils.CreateCollidingSpriteTransformMatrix(CAS);
                    CollisionUtils.CreateCollidingSpriteTransformMatrix(this);
                    CollisionUtils.GetColColors(this);
                    CollisionUtils.GetColColors(CAS);
                    return CollisionUtils.IntersectPixels(this.TransformMatrix * Matrix.CreateTranslation(new Vector3(this.currentAnim.ColCorrection,0f)), this.ColBoundingBox.Width, this.ColBoundingBox.Height, Colors, this.IgnoreTranslucentCol, CAS.TransformMatrix * Matrix.CreateTranslation(new Vector3(CAS.currentAnim.ColCorrection,0f)), CAS.ColBoundingBox.Width, CAS.ColBoundingBox.Height, CAS.Colors, CAS.IgnoreTranslucentCol);
                }
            }
            else
            {
                CollisionUtils.CreateCollidingSpriteTransformMatrix(CAS);
                CollisionUtils.CreateCollidingSpriteTransformMatrix(this);
                colltemp = CollisionUtils.CalculateBoundingRectangle(tempdrawrect, _transformMatrix);
                if (colltemp.Intersects(CollisionUtils.CalculateBoundingRectangle(CAS.tempdrawrect, CAS._transformMatrix)))
                {
                    CollisionUtils.GetColColors(this);
                    CollisionUtils.GetColColors(CAS);
                    return CollisionUtils.IntersectPixels(this.TransformMatrix * Matrix.CreateTranslation(new Vector3(this.currentAnim.ColCorrection, 0f)), this.ColBoundingBox.Width, this.ColBoundingBox.Height, Colors, this.IgnoreTranslucentCol, CAS.TransformMatrix * Matrix.CreateTranslation(new Vector3(CAS.currentAnim.ColCorrection, 0f)), CAS.ColBoundingBox.Width, CAS.ColBoundingBox.Height, CAS.Colors, CAS.IgnoreTranslucentCol);
                }
            }
            return false;
        }

        /// <summary>
        /// Do you intersect with another ICollider
        /// </summary>
        /// <param name="intersector">what you are checking against</param>
        /// <returns>If you intersect</returns>
        public bool Intersects(ICollider intersector)
        {
            return BoundingBox.Intersects(intersector.BoundingBox);
        }

        public new void Dispose()
        {
            base.Dispose();
        }
    }

    public interface ICollider
    {
        Rectangle BoundingBox {get;}
        bool IgnoreTranslucentCol { get; set; }
        bool Intersects(ICollider intersector);
    }
}
