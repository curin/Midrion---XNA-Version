using com.dbtgaming.Library;
using com.dbtgaming.Library.Engine2D.ActorProperties;
using com.dbtgaming.Library.Engine2D.Actors;
using com.dbtgaming.Library.Engine2D.Collision;
using com.dbtgaming.Library.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace com.dbtgaming.Game.Library.Magic
{
    /// <summary>
    /// Basic Shape Template
    /// </summary>
    public interface IShape
    {
        List<MagicPoint> Points { get; set; }
        int Frame { get; set; }
        float Speed { get; set; }
        int InstabilityPoints { get; set; }
        int Complexity { get; set; }
        Dictionary<string, int> DefinedAttributes { get; set; }
        Dictionary<string, int> Damages { get; set; }
        Color CastColorMod { get; set; }
        List<int> FinishedId { get; set; }
        bool FirstTarget { get; set; }
        Entity Target { get; set; }
        int PointAttachment { get; set; }
        List<ICaster> Owners { get; set; }
        List<IDefinedShape> IdentifiedShapes { get; set; }
        Vector2 PreviousLoc { get; set; }
        Dictionary<Type, int> ShapePriority { get; set; }
        int Connections { get; set; }
    }

    public struct IShapeMethods
    {
        public static void CalculateShapePriority(IShape shpe, Dictionary<Type, int> castPrefs)
        {
            Type tp;
            foreach (IDefinedShape def in shpe.IdentifiedShapes)
            {
                tp = def.GetType();
                if (shpe.ShapePriority.ContainsKey(tp))
                {
                    shpe.ShapePriority[tp] += castPrefs[tp];
                }
                else
                {
                    shpe.ShapePriority.Add(tp, castPrefs[tp]);
                }
            }
        }

        public static bool intersectCon(CollidingAdvancedSprite CAS, Vector2 pointa, Vector2 pointb, Vector2 _magicPntSizeOnScreen, GameCam cam)
        {
            float x = (((CAS.BoundingBox.Y + cam.Location.Y) - pointa.Y) / ((pointb.Y - pointa.Y) / (pointb.X - pointa.X))) + pointa.X;
            if (x + (_magicPntSizeOnScreen.Y / 8) >= CAS.BoundingBox.X + cam.Location.X && x - (_magicPntSizeOnScreen.Y / 8) <= CAS.BoundingBox.X + cam.Location.X + CAS.BoundingBox.Width)
            {
                return true;
            }

            x = (((CAS.BoundingBox.Y + cam.Location.Y + CAS.BoundingBox.Height) - pointa.Y) / ((pointb.Y - pointa.Y) / (pointb.X - pointa.X))) + pointa.X;
            if (x + (_magicPntSizeOnScreen.Y / 8) >= CAS.BoundingBox.X + cam.Location.X && x - (_magicPntSizeOnScreen.Y / 8) <= CAS.BoundingBox.X + cam.Location.X + CAS.BoundingBox.Width)
            {
                return true;
            }

            x = ((pointb.Y - pointa.Y) / (pointb.X - pointa.X)) * (CAS.BoundingBox.X + cam.Location.X - pointa.X) + pointa.Y;
            if (x + (_magicPntSizeOnScreen.Y / 8) >= CAS.BoundingBox.Y + cam.Location.Y && x - (_magicPntSizeOnScreen.Y / 8) <= CAS.BoundingBox.Y + cam.Location.Y + CAS.BoundingBox.Height)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Method used for Combining Shapes
        /// </summary>
        /// <param name="ret">return variable</param>
        /// <param name="shapeA">First Shape</param>
        /// <param name="shapeB">Second Shape</param>
        /// <param name="connectingPointA">Point Connecting from First Shape</param>
        /// <param name="connectingPointB">Point Connecting from Second Shape</param>
        /// <param name="connectionType">What kind of connection is being made</param>
        /// <returns>Combined Shape</returns>
        public static IShape Combine(IShape ret, IShape shapeA, IShape shapeB, int connectingPointA, int connectingPointB)
        {
            //When a combine happens the shape moves
            ret.Connections = shapeA.Connections + shapeB.Connections + 1;
            ret.Points = new List<MagicPoint>();
            ret.Points.AddRange(shapeA.Points);
            ret.Points.AddRange(shapeB.Points);
            for (int i = shapeA.Points.Count; i < ret.Points.Count; i++)
            {
                ret.Points[i].ID += shapeA.Points.Count;
                for (int index = 0; index < ret.Points[i]._connections.Count; index++)
                {
                    ret.Points[i]._connections[index] += shapeA.Points.Count;
                }
            }
            ret.Points[connectingPointA]._connections.Add(shapeA.Points.Count + connectingPointB);
            ret.Points[shapeA.Points.Count + connectingPointB]._connections.Add(connectingPointA);
            ret.Owners = shapeA.Owners;
            ret.Owners.AddRange(shapeB.Owners);
            if (shapeA.Target != null)
            {
                ret.Target = shapeA.Target;
                ret.PointAttachment = shapeA.PointAttachment;
                ret.PreviousLoc = shapeA.PreviousLoc;
            }
            else
            {
                ret.Target = shapeB.Target;
                ret.PointAttachment = shapeA.Points.Count + shapeB.PointAttachment;
                ret.PreviousLoc = shapeB.PreviousLoc;
            }
            ret.InstabilityPoints = shapeA.InstabilityPoints + shapeB.InstabilityPoints;
            return ret;
        }

        /// <summary>
        /// A method used to compare Recognized Shapes
        /// </summary>
        /// <param name="ShapeA">First Shape</param>
        /// <param name="ShapeB">Second Shape</param>
        /// <returns></returns>
        public static List<RecognizedShape> Compare(RecognizedShape ShapeA, RecognizedShape ShapeB)
        {
            List<RecognizedShape> shapes = new List<RecognizedShape>();
            shapes.Add(ShapeA);
            shapes.Add(ShapeB);
            int ConsA = 0;
            int ConsB = 0;
            foreach (int i in ShapeA.OriginCon)
                ConsA++;
            foreach (int i in ShapeB.OriginCon)
                ConsB++;
            Dictionary<int, List<int>> Connects = new Dictionary<int, List<int>>();
            List<int> temp = new List<int>();
            //Check which Shape is Larger
            if (ShapeA.Points.Count() >= ShapeB.Points.Count() && ConsA >= ConsB)
            {
                foreach (int pnt in ShapeA.Points)
                {
                    temp = new List<int>();
                    for (int i = 0; i < ShapeA.OriginCon.Count(); i++)
                        if (ShapeA.OriginCon[i] == pnt)
                            temp.Add(ShapeA.DestCon[i]);

                    for (int i = 0; i < ShapeA.DestCon.Count(); i++)
                        if (ShapeA.DestCon[i] == pnt)
                            temp.Add(ShapeA.OriginCon[i]);

                    Connects.Add(pnt, temp);
                }
                bool consumed = true;
                //Check whether each point is unique
                foreach (int pnt in ShapeA.Points)
                {
                    if (consumed)
                    {
                        if (!ShapeB.Points.Contains(pnt))
                        {
                            consumed = false;
                        }
                        else
                        {
                            if (Connects.ContainsKey(pnt))
                            {
                                for (int index = 0; index < ShapeB.OriginCon.Count(); index++)
                                {
                                    if (!Connects[ShapeB.OriginCon[index]].Contains(ShapeB.DestCon[index]))
                                        consumed = false;
                                }
                            }
                        }
                    }
                }

                //if there is no unique pieces of ShapeB, Remove it
                if (consumed)
                {
                    shapes.Remove(ShapeB);
                }
            }
            else if (ShapeA.Points.Count() <= ShapeB.Points.Count() && ConsA <= ConsB)
            {
                bool consumed = true;

                foreach (int pnt in ShapeB.Points)
                {
                    temp = new List<int>();
                    for (int i = 0; i < ShapeB.OriginCon.Count(); i++)
                        if (ShapeB.OriginCon[i] == pnt)
                            temp.Add(ShapeB.DestCon[i]);

                    for (int i = 0; i < ShapeB.DestCon.Count(); i++)
                        if (ShapeB.DestCon[i] == pnt)
                            temp.Add(ShapeB.OriginCon[i]);

                    Connects.Add(pnt, temp);
                }

                //Check whether each point is unique
                foreach (int pnt in ShapeB.Points)
                {
                    if (consumed)
                    {
                        if (!ShapeA.Points.Contains(pnt))
                        {
                            consumed = false;
                        }
                        else
                        {
                            if (Connects.ContainsKey(pnt))
                            {
                                for (int index = 0; index < ShapeA.OriginCon.Count(); index++)
                                {
                                    if (!Connects[ShapeA.OriginCon[index]].Contains(ShapeA.DestCon[index]))
                                        consumed = false;
                                }
                            }
                        }
                    }
                }

                //if there is no unique pieces of ShapeA, Remove it
                if (consumed)
                {
                    shapes.Remove(ShapeA);
                }
            }

            return shapes;
        }
    }

    public struct InstabilityProduct
    {
        private Func<Random, int, IShape, int> _addInstability;
        private int _cost;

        public Func<Random, int, IShape, int> AddInstability
        {
            get
            {
                return _addInstability;
            }
            set
            {
                _addInstability = value;
            }
        }

        public int Cost
        {
            get
            {
                return _cost;
            }
            set
            {
                _cost = value;
            }
        }
    }

    /// <summary>
    /// Basic Point Used in Shapes
    /// </summary>
    public class MagicPoint
    {
        internal int _iD;
        public int ID
        {
            get
            {
                return _iD;
            }
            set
            {
                _iD = value;
            }
        }

        /// <summary>
        /// Check if point is within Camera bounds
        /// </summary>
        /// <param name="cam">camera object</param>
        /// <param name="pntSizeOnScreen">pnt size on screen</param>
        /// <returns></returns>
        public bool OnScreen(ICamera cam, Vector2 pntSizeOnScreen)
        {
            _tempIntersects = new Rectangle((int)Location.X - (int)pntSizeOnScreen.X / 2, (int)Location.Y - (int)pntSizeOnScreen.Y / 2, (int)pntSizeOnScreen.X,
                (int)pntSizeOnScreen.Y);
            return Utilities.Vector2ToRect(cam.Location, cam.Viewport.Width, cam.Viewport.Height).Intersects(_tempIntersects);
        }

        internal Rectangle _tempIntersects;
        /// <summary>
        /// Collision detection for Magic Points and other objects
        /// </summary>
        /// <param name="CAS">Other Object</param>
        /// <param name="pntSizeOnScreen">Size of Point on Screen</param>
        /// <param name="cam">Camera Object</param>
        /// <returns>If object is colliding with Point</returns>
        public bool intersects(CollidingAdvancedSprite CAS, Vector2 pntSizeOnScreen, ICamera cam)
        {
            _tempIntersects = new Rectangle((int)Location.X - (int)cam.Location.X - (int)pntSizeOnScreen.X / 2, (int)Location.Y - (int)cam.Location.Y - (int)pntSizeOnScreen.Y / 2, (int)pntSizeOnScreen.X,
                (int)pntSizeOnScreen.Y);
            return CAS.BoundingBox.Intersects(_tempIntersects);
        }

        internal List<int> _connections;
        public List<int> Connections
        {
            get
            {
                return _connections;
            }
            set
            {
                _connections = value;
            }
        }

        internal Vector2 _location;

        internal int _rotdir = 1;
        public int RotationDirection
        {
            get
            {
                return _rotdir;
            }
            set
            {
                _rotdir = value;
            }
        }

        internal Rectangle rectonScreen;

        private int _frameMod;
        /// <summary>
        /// A modifier to offset the frame for each point
        /// </summary>
        public int FrameMod
        {
            get
            {
                return _frameMod;
            }
            set
            {
                _frameMod = value;
            }
        }

        private float _rotMod;
        /// <summary>
        /// A modifier to offset the rotation for each point
        /// </summary>
        public float RotationMod
        {
            get
            {
                return _rotMod;
            }
            set
            {
                _rotMod = value;
            }
        }

        /// <summary>
        /// point Location in World
        /// </summary>
        public Vector2 Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
            }
        }

        internal Vector2 _moveToLocation;
        /// <summary>
        /// Location to move point to
        /// </summary>
        public Vector2 MoveToLocation
        {
            get
            {
                return _moveToLocation;
            }
            set
            {
                _moveToLocation = value;
            }
        }

        internal Vector2 _speed;
        /// <summary>
        /// Multiplier to movement of pixels per frame
        /// </summary>
        public Vector2 Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
            }
        }
    }
}
