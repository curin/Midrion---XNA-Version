using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace com.dbtgaming.Library
{
    public class Utilities
    {
        public static Rectangle Vector2ToRect(Vector2 vect, int width, int height)
        {
            return new Rectangle((int)vect.X, (int)vect.Y, width, height);
        }
    }
}
