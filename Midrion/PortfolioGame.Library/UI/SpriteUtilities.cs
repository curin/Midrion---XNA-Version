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
    public class SpriteUtilities
    {
        protected static Matrix _colMatrix1;
        protected static Matrix _colMatrix2;
        protected static IAdvancedSpriteInstance _advancedinstance;

        public static bool SpriteCollisionPixel(ISpriteInstance sprite, ISpriteInstance sprite2, Color[] colors, Color[] colors2, Rectangle rect1, Rectangle rect2, bool AllowTranslucent1, bool AllowTranslucent2)
        {
            _colMatrix1 = Matrix.CreateTranslation(sprite.Location.X + sprite.CollisionCorrection.X, sprite.Location.Y + sprite.CollisionCorrection.Y, 1) * Matrix.CreateScale(sprite.Scale.X, sprite.Scale.Y, 1) *
                Matrix.CreateRotationZ(sprite.Rotation);
            if (sprite.DrawEffects == SpriteEffects.FlipHorizontally)
                _colMatrix1 *= Matrix.CreateRotationY((float)Math.PI);
            if (sprite.DrawEffects == SpriteEffects.FlipVertically)
                _colMatrix1 *= Matrix.CreateRotationX((float)Math.PI);

            _colMatrix2 = Matrix.CreateTranslation(sprite2.Location.X + sprite2.CollisionCorrection.X, sprite2.Location.Y + sprite2.CollisionCorrection.Y, 1) * Matrix.CreateScale(sprite2.Scale.X, sprite2.Scale.Y, 1) *
                Matrix.CreateRotationZ(sprite2.Rotation);
            if (sprite2.DrawEffects == SpriteEffects.FlipHorizontally)
                _colMatrix2 *= Matrix.CreateRotationY((float)Math.PI);
            if (sprite2.DrawEffects == SpriteEffects.FlipVertically)
                _colMatrix2 *= Matrix.CreateRotationX((float)Math.PI);

            return IntersectPixels(_colMatrix1, rect1.Width, rect1.Height, colors, AllowTranslucent1, _colMatrix2, rect2.Width, rect2.Height, colors2, AllowTranslucent2);
        }

        protected static Color[] _tempColor;

        public static Color[] GetColColors(Texture2D spriteSheet, Rectangle sourcerect)
        {
            _tempColor = new Color[sourcerect.Width * sourcerect.Height];
            spriteSheet.GetData<Color>(0, sourcerect, _tempColor, 0, sourcerect.Width * sourcerect.Height);
            return _tempColor;
        }

        /// <summary>
        /// Determines if there is overlap of the non-transparent pixels between two
        /// sprites.
        /// 
        /// From XNA Transformed Collision Tutorial
        /// </summary>
        /// <param name="transformA">World transform of the first sprite.</param>
        /// <param name="widthA">Width of the first sprite's texture.</param>
        /// <param name="heightA">Height of the first sprite's texture.</param>
        /// <param name="dataA">Pixel color data of the first sprite.</param>
        /// <param name="transformB">World transform of the second sprite.</param>
        /// <param name="widthB">Width of the second sprite's texture.</param>
        /// <param name="heightB">Height of the second sprite's texture.</param>
        /// <param name="dataB">Pixel color data of the second sprite.</param>
        /// <param name="AllowTranslucentA">are translucent blocks ignored in A</param>
        /// <param name="AllowTranslucentB">are translucent blocks ignored in B</param>
        /// <returns>True if non-transparent pixels overlap; false otherwise</returns>
        public static bool IntersectPixels(
                            Matrix transformA, int widthA, int heightA, Color[] dataA, bool AllowTranslucentA,
                            Matrix transformB, int widthB, int heightB, Color[] dataB, bool AllowTranslucentB)
        {
            // Calculate a matrix which transforms from A's local space into
            // world space and then into B's local space
            Matrix transformAToB = transformA * Matrix.Invert(transformB);

            // When a point moves in A's local space, it moves in B's local space with a
            // fixed direction and distance proportional to the movement in A.
            // This algorithm steps through A one pixel at a time along A's X and Y axes
            // Calculate the analogous steps in B:
            Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
            Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

            // Calculate the top left corner of A in B's local space
            // This variable will be reused to keep track of the start of each row
            Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

            // For each row of pixels in A
            for (int yA = 0; yA < heightA; yA++)
            {
                // Start at the beginning of the row
                Vector2 posInB = yPosInB;

                // For each pixel in this row
                for (int xA = 0; xA < widthA; xA++)
                {
                    // Round to the nearest pixel
                    int xB = (int)Math.Round(posInB.X);
                    int yB = (int)Math.Round(posInB.Y);

                    // If the pixel lies within the bounds of B
                    if (0 <= xB && xB < widthB &&
                        0 <= yB && yB < heightB)
                    {
                        // Get the colors of the overlapping pixels
                        Color colorA = dataA[xA + yA * widthA];
                        Color colorB = dataB[xB + yB * widthB];
                        bool A = false;
                        bool B = false;
                        if (AllowTranslucentA && colorA.A == 1)
                            A = true;
                        if (AllowTranslucentB && colorB.A == 1)
                            B = true;
                        if (colorA.A != 0 && colorB.A != 0 || A && B)
                        {
                            // then an intersection has been found
                            return true;
                        }


                    }

                    // Move to the next pixel in the row
                    posInB += stepX;
                }

                // Move to the next row
                yPosInB += stepY;
            }

            // No intersection found
            return false;
        }

        /// <summary>
        /// Determines if there is overlap of the non-transparent pixels
        /// between two sprites.
        /// 
        /// From XNA Transformed Collision Tutorial
        /// </summary>
        /// <param name="rectangleA">Bounding rectangle of the first sprite</param>
        /// <param name="dataA">Pixel data of the first sprite</param>
        /// <param name="rectangleB">Bouding rectangle of the second sprite</param>
        /// <param name="dataB">Pixel data of the second sprite</param>
        /// <param name="AllowTranslucentA">are translucent blocks ignored in A</param>
        /// <param name="AllowTranslucentB">are translucent blocks ignored in B</param>
        /// <returns>True if non-transparent pixels overlap; false otherwise</returns>
        public static bool IntersectPixels(Rectangle rectangleA, Color[] dataA, bool AllowTranslucentA,
                                           Rectangle rectangleB, Color[] dataB, bool AllowTranslucentB)
        {
            // Find the bounds of the rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Get the color of both pixels at this point
                    Color colorA = dataA[(x - rectangleA.Left) +
                                         (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) +
                                         (y - rectangleB.Top) * rectangleB.Width];

                    bool A = false;
                    bool B = false;
                    if (AllowTranslucentA && colorA.A == 1)
                        A = true;
                    if (AllowTranslucentB && colorB.A == 1)
                        B = true;
                    if (colorA.A != 0 && colorB.A != 0 || A && B)
                    {
                        // then an intersection has been found
                        return true;
                    }
                }
            }

            // No intersection found
            return false;
        }
    }
}
