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

namespace com.dbtgaming.Game.Library.Magic
{
    public class FireShape : IPrimalShape
    {
        List<RecognizedShape> _temp = new List<RecognizedShape>();
        List<List<int>> connections = new List<List<int>>();
        Shape newshape = new Shape(true);
        public bool Independant
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        /// Method used to change shape when found inside
        /// </summary>
        /// <param name="shpe">Shape found in</param>
        public void UpdateShape(IShape shpe)
        {

        }

        int[] _tempArray;
        List<int[]> _trianglesfound;
        bool _found = false;
        RecognizedShape _recShape;

        /// <summary>
        /// Method used to Recognize Fire
        /// </summary>
        /// <param name="shape">Shape to recognize</param>
        /// <returns>List of Fire symbols found</returns>
        public List<RecognizedShape> Recognize(IShape shape)
        {
            _temp = new List<RecognizedShape>();
            _trianglesfound = new List<int[]>();
            foreach (MagicPoint pnt in shape.Points)
            {
                foreach (int pnt1 in pnt.Connections)
                {
                    foreach (int pnt2 in pnt.Connections)
                    {
                        if (pnt1 != pnt2)
                        {
                            if (shape.Points[pnt1].Connections.Contains(pnt2))
                            {
                                foreach (int[] array in _trianglesfound)
                                {
                                    if (!array.Contains(pnt.ID) && !array.Contains(pnt1) && !array.Contains(pnt2))
                                    {
                                        _recShape = new RecognizedShape(this);
                                        _tempArray = new int[3];
                                        _tempArray[0] = pnt.ID;
                                        _tempArray[1] = pnt1;
                                        _tempArray[2] = pnt2;
                                        _trianglesfound.Add(_tempArray);
                                        _recShape.Points = _tempArray;
                                        _tempArray = new int[3];
                                        _tempArray[0] = pnt.ID;
                                        _tempArray[1] = pnt.ID;
                                        _tempArray[2] = pnt1;
                                        _recShape.OriginCon = _tempArray;
                                        _tempArray = new int[3];
                                        _tempArray[0] = pnt1;
                                        _tempArray[1] = pnt2;
                                        _tempArray[2] = pnt2;
                                        _recShape.OriginCon = _tempArray;
                                        _temp.Add(_recShape);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return _temp;
        }

        //--------------
        //Replace Cast Methods for targeted and AOE
        //--------------

        public int CalculateDamage(IShape shpe)
        {
            return 0;
        }

        /// <summary>
        /// Basic Cast for this Defined Shape
        /// </summary>
        public void Cast(IShape shpe)
        {

        }

        public void BasicCastDraw()
        {

        }
    }
}
