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

namespace com.dbtgaming.Library.Engine2D.Collision
{
    public class CollisionUtils
    {
        /// <summary>
        /// Get the colors from the Collision map for perpixel collision
        /// </summary>
        /// <param name="CAS">the sprite</param>
        /// <param name="SpriteSheet">the sprite sheet</param>
        /// <returns></returns>
        public static void GetColColors(CollidingAdvancedSprite CAS)
        {
            CAS.Colors = new Color[(int)CAS.CurrentAnim.ColSize.X * (int)CAS.CurrentAnim.ColSize.Y];
            CAS.Tex.GetData<Color>(0, CAS.CollisionTextureLoc, CAS.Colors, 0, (int)CAS.CurrentAnim.ColSize.X * (int)CAS.CurrentAnim.ColSize.Y);
        }

        /// <summary>
        /// Get the colors from the sprite map for perpixel collision
        /// </summary>
        /// <param name="CAS">the sprite</param>
        /// <param name="SpriteSheet">the sprite sheet</param>
        /// <returns></returns>
        public static Color[] GetColors(CollidingAdvancedSprite CAS)
        {
            CAS.Colors = new Color[(int)CAS.CurrentAnim.SpriteSize.X * (int)CAS.CurrentAnim.SpriteSize.Y];
            CAS.Tex.GetData<Color>(0, CAS.sourcerect, CAS.Colors, 0, (int)CAS.CurrentAnim.SpriteSize.X * (int)CAS.CurrentAnim.SpriteSize.Y);
            return CAS.Colors;
        }

        public static Color[] GetColors(Rectangle sourcerect, Texture2D spritesheet)
        {
            Color[] colors = new Color[sourcerect.Height * sourcerect.Width];
            spritesheet.GetData<Color>(0, sourcerect, colors, 0, sourcerect.Height * sourcerect.Width);
            return colors;
        }

        /// <summary>
        /// Create the transformation Matrix for the colliding sprite
        /// </summary>
        /// <param name="CAS">The Sprite</param>
        public static void CreateCollidingSpriteTransformMatrix(CollidingAdvancedSprite CAS)
        {
            CAS._transformMatrix = Matrix.CreateTranslation(new Vector3(CAS.RotationOrigin, 0f)) * Matrix.CreateScale(new Vector3(CAS.Scale, 0f)) * Matrix.CreateRotationZ(CAS.Rotation) * Matrix.CreateTranslation(new Vector3(-CAS.RotationOrigin * CAS.Scale, 0));
        }

        /// <summary>
        /// Calculates an axis aligned rectangle which fully contains an arbitrarily
        /// transformed axis aligned rectangle.
        /// 
        /// From XNA Transformed Collision Tutorial
        /// </summary>
        /// <param name="rectangle">Original bounding rectangle.</param>
        /// <param name="transform">World transform of the rectangle.</param>
        /// <returns>A new rectangle which contains the trasnformed rectangle.</returns>
        public static Rectangle CalculateBoundingRectangle(Rectangle rectangle,
                                                           Matrix transform)
        {
            // Get all four corners in local space
            Vector2 leftTop = new Vector2(rectangle.Left, rectangle.Top);
            Vector2 rightTop = new Vector2(rectangle.Right, rectangle.Top);
            Vector2 leftBottom = new Vector2(rectangle.Left, rectangle.Bottom);
            Vector2 rightBottom = new Vector2(rectangle.Right, rectangle.Bottom);

            // Transform all four corners into work space
            Vector2.Transform(ref leftTop, ref transform, out leftTop);
            Vector2.Transform(ref rightTop, ref transform, out rightTop);
            Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

            // Find the minimum and maximum extents of the rectangle in world space
            Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop),
                                      Vector2.Min(leftBottom, rightBottom));
            Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop),
                                      Vector2.Max(leftBottom, rightBottom));

            // Return that as a rectangle
            return new Rectangle((int)min.X, (int)min.Y,
                                 (int)(max.X - min.X), (int)(max.Y - min.Y));
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
