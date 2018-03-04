using com.dbtgaming.Library;
using com.dbtgaming.Library.Engine2D.Actors;
using com.dbtgaming.Library.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace com.dbtgaming.Game.Library.Magic
{
    public class ShapeRegistry
    {
        protected static Rectangle _tempSource;
        protected static Rectangle _tempDrawCon;
        protected static Vector3 _tempLocRect = Vector3.Zero;
        protected static int _instabilityChoice = 0;
        protected static Vector2 _moveToChange;
        protected static Vector2 _normalizedMoveTo;
        protected static float _internalSpeed = 0;
        protected static int _internalFrameReset = 1;
        protected static int _internalFrame = 0;
        protected static Vector2 _posChange = new Vector2(0, 0);

        protected static List<MagicPoint> _pointsOnScreen = new List<MagicPoint>();
        /// <summary>
        /// List of Points on screen currently
        /// </summary>
        public static List<MagicPoint> PointsOnScreen
        {
            get
            {
                return _pointsOnScreen;
            }
            set
            {
                _pointsOnScreen = value;
            }
        }

        /// <summary>
        /// Last Shape in Shapes list
        /// </summary>
        public static IShape LastShape
        {
            get
            {
                return _shapes[_shapes.Count - 1];
            }
            set
            {
                _shapes[_shapes.Count - 1] = value;
            }
        }

        protected static List<InstabilityProduct> _instabilityProducts = new List<InstabilityProduct>();
        /// <summary>
        /// The Methods built to cause the products of instability
        /// </summary>
        public static List<InstabilityProduct> InstabilityProducts
        {
            get
            {
                return _instabilityProducts;
            }
            set
            {
                _instabilityProducts = value;
            }
        }

        protected static Dictionary<IDefinedShape, int> _definedShapes = new Dictionary<IDefinedShape, int>();
        /// <summary>
        /// List to help decide what shape is defined in control
        /// </summary>
        public static Dictionary<IDefinedShape, int> DefinedShapes
        {
            get
            {
                return _definedShapes;
            }
            set
            {
                _definedShapes = value;
            }
        }

        protected internal static Rectangle _magicPntSource;
        /// <summary>
        /// Source rectangle for magic point from spritesheet
        /// </summary>
        public static Rectangle MagicPointSource
        {
            get
            {
                return _magicPntSource;
            }
            set
            {
                _magicPntSource = value;
            }
        }

        protected internal static Rectangle _magicConSource;
        /// <summary>
        /// Source rectangle for magic connection from spritesheet
        /// </summary>
        public static Rectangle MagicConnectionSource
        {
            get
            {
                return _magicConSource;
            }
            set
            {
                _magicConSource = value;
            }
        }

        protected static Vector2 _pntSize = new Vector2(24, 24);
        /// <summary>
        /// Size of points on screen
        /// </summary>
        public static Vector2 PointSize
        {
            get
            {
                return _pntSize;
            }
            set
            {
                _pntSize = value;
            }
        }

        protected static Vector2 _pntOrigin;
        /// <summary>
        /// Center of each point
        /// </summary>
        public static Vector2 PntOrigin
        {
            get
            {
                return _pntOrigin;
            }
            set
            {
                _pntOrigin = value;
            }
        }

        protected static List<IShape> _shapes = new List<IShape>();
        /// <summary>
        /// _shapes that exist in the world
        /// </summary>
        public static List<IShape> Shapes
        {
            get
            {
                return _shapes;
            }
            set
            {
                _shapes = value;
            }
        }

        protected static int _frame = 0;
        /// <summary>
        /// Current frame each animation is on
        /// </summary>
        public static int Frame
        {
            get
            {
                return _frame;
            }
            set
            {
                _frame = value;
            }
        }

        protected static float _rotation = 0;
        /// <summary>
        /// Rotation for each of the shapes
        /// </summary>
        public static float Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
            }
        }

        protected static float _speed = .1f;
        /// <summary>
        /// speed at which the animations are going
        /// </summary>
        public static float Speed
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

        #region Methods
        /// <summary>
        /// Create method list for instability list
        /// </summary>
        public static void BuildInstabilityList()
        {
            InstabilityProduct prod = new InstabilityProduct();
            _instabilityProducts = new List<InstabilityProduct>();
            prod.AddInstability = InstabilityMethods.MovePoint;
            prod.Cost = 8192;
            _instabilityProducts.Add(prod);
            prod = new InstabilityProduct();
            prod.AddInstability = InstabilityMethods.AddPoint;
            prod.Cost = 32768;
            _instabilityProducts.Add(prod);
            prod = new InstabilityProduct();
            prod.AddInstability = InstabilityMethods.AddConnection;
            prod.Cost = 4096;
            _instabilityProducts.Add(prod);
        }

        /// <summary>
        /// Updates Each shape
        /// </summary>
        public static void Update(List<Entity> entities, GameCam cam, int lastPointClicked, int lastShapeClicked, bool movingPnt, Random _rand)
        {
            foreach (IShape sh in _shapes)
            {
                if (Shapes.IndexOf(sh) == lastShapeClicked && movingPnt)
                {
                    UpdateEntity(entities, _pntSize, cam);
                    UpdateShape(true, lastPointClicked, _rand, sh);
                }
                else
                {
                    UpdateEntity(entities, _pntSize, cam);
                    UpdateShape(false, 0, _rand, sh);
                }
            }
        }

        /// <summary>
        /// Create pointsOnScreen list in shape
        /// </summary>
        /// <param name="ShapeID">shape id</param>
        /// <param name="cam">camera</param>
        public static void isonscreen(int ShapeID, GameCam cam)
        {
            _pointsOnScreen = new List<MagicPoint>();
            foreach (MagicPoint pnt in _shapes[ShapeID].Points)
            {
                if (pnt.OnScreen(cam, _pntSize))
                {
                    _pointsOnScreen.Add(pnt);
                }
            }
        }

        /// <summary>
        /// Create pointsOnScreen list in shape
        /// </summary>
        /// <param name="sh">shape object</param>
        /// <param name="cam">camera</param>
        public static void isonscreen(IShape sh, GameCam cam)
        {
            _pointsOnScreen = new List<MagicPoint>();
            foreach (MagicPoint pnt in sh.Points)
            {
                if (pnt.OnScreen(cam, _pntSize))
                {
                    _pointsOnScreen.Add(pnt);
                }
            }
        }

        /// <summary>
        /// Update Entity which shape is attached to
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="pntsize"></param>
        /// <param name="cam"></param>
        public static void UpdateEntity(List<Entity> entities, Vector2 pntsize, GameCam cam)
        {
            foreach (IShape sh in _shapes)
            {
                if (sh.Target != null)
                {
                    foreach (Entity ent in entities)
                    {
                        if (ent.Name == sh.Target.Name)
                        {
                            sh.Target = ent;
                        }
                    }

                    if (sh.FinishedId.Contains(sh.PointAttachment))
                    {
                        if (!sh.Points[sh.PointAttachment].intersects(sh.Target, pntsize, cam))
                        {
                            sh.Target = null;
                        }
                    }
                }
                else
                {
                    foreach (MagicPoint pnt in sh.Points)
                    {
                        if (sh.Target == null)
                        {
                            if (pnt.Location == pnt.MoveToLocation && sh.FinishedId.Contains(sh.Points.IndexOf(pnt)))
                            {
                                foreach (Entity ent in entities)
                                {
                                    if (pnt.intersects(ent, pntsize, cam))
                                    {
                                        sh.Target = ent;
                                        sh.PreviousLoc = ent.Location;
                                        sh.PointAttachment = sh.Points.IndexOf(pnt);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Update Method for the Shape Changes many important variables
        /// </summary>
        public static void UpdateShape(bool pntmove, int pntindex, Random rand, IShape sh)
        {
            sh.FinishedId = new List<int>();
            Rotation += .01f;
            sh.Complexity = sh.Points.Count * 10 + sh.Connections * 2;
            sh.InstabilityPoints += sh.Complexity;
            int random = (rand.Next(0, (sh.InstabilityPoints * sh.InstabilityPoints)));
            foreach (IDefinedShape defshpe in sh.IdentifiedShapes)
                defshpe.UpdateShape(sh);
            if (random > (32768 * sh.Complexity))
            {
                _instabilityChoice = rand.Next(0, _instabilityProducts.Count - 1 + (_instabilityProducts.Count));
                if (_instabilityChoice <= _instabilityProducts.Count - 1)
                {
                    if (sh.InstabilityPoints >= _instabilityProducts[_instabilityChoice].Cost)
                    {
                        sh.InstabilityPoints -= _instabilityProducts[_instabilityChoice].Cost;
                        _instabilityProducts[_instabilityChoice].AddInstability(rand, sh.InstabilityPoints, sh);
                    }
                }
                else if (_instabilityChoice < _instabilityProducts.Count + 2)
                {
                    _instabilityChoice -= _instabilityProducts.Count - 1;
                    sh.InstabilityPoints -= sh.Complexity * 3 * _instabilityChoice;
                }
            }
            //Change Frame
            if (Speed > 1)
            {
                _frame += (int)_speed;
                _internalSpeed = _speed - (int)_speed;
                if (_internalSpeed > 0)
                {
                    _internalFrameReset = (int)(1 / _internalSpeed);
                    _internalFrame++;
                    if (_internalFrameReset <= _internalFrame)
                    {
                        _internalFrame = 0;
                        _frame++;
                    }
                }
            }
            else if (_speed == 1)
            {
                _frame++;
            }
            else
            {
                _internalFrame++;
                if (_internalFrame >= (1 / Speed))
                {
                    _internalFrame = 0;
                    _frame++;
                }
            }

            if (sh.Target != null && !pntmove && sh.Points[sh.PointAttachment].MoveToLocation == sh.Points[sh.PointAttachment].Location
                || pntindex != sh.PointAttachment && sh.Target != null && sh.Points[sh.PointAttachment].MoveToLocation == sh.Points[sh.PointAttachment].Location)
            {
                if (sh.FirstTarget)
                {
                    _posChange = sh.PreviousLoc - sh.Target.Location;
                    if (_posChange != Vector2.Zero)
                    {
                        for (int i = 0; i < sh.Points.Count; i++)
                        {
                            sh.Points[i]._location -= _posChange;
                            sh.Points[i].MoveToLocation = sh.Points[i].Location;
                        }
                    }
                    _posChange = new Vector2(0, 0);
                }
                else
                {
                    sh.FirstTarget = true;
                }
                sh.PreviousLoc = sh.Target.Location;
            }

            foreach (MagicPoint pnt in sh.Points)
            {
                if (pnt.Location != pnt.MoveToLocation)
                {
                    _moveToChange = (pnt.MoveToLocation - pnt.Location);
                    _normalizedMoveTo = _moveToChange;
                    _normalizedMoveTo.Normalize();

                    if (_moveToChange.X > _normalizedMoveTo.X * pnt.Speed.X)
                    {
                        pnt._location.X += _normalizedMoveTo.X * pnt.Speed.X;
                    }
                    else
                    {
                        pnt._location.X = pnt.MoveToLocation.X;
                    }

                    if (_moveToChange.Y > _normalizedMoveTo.Y * pnt.Speed.Y)
                    {
                        pnt._location.Y += _normalizedMoveTo.Y * pnt.Speed.Y;
                    }
                    else
                    {
                        pnt._location.Y = pnt.MoveToLocation.Y;
                    }

                    if (pnt.MoveToLocation == pnt.Location)
                        sh.FinishedId.Add(sh.Points.IndexOf(pnt));
                }
            }
        }

        static List<int> _completedConnections;
        static float _distance;

        /// <summary>
        /// Method used for Drawing Shape
        /// </summary>
        public static void Draw(ICamera cam, Texture2D SpriteSheet, bool current, int _currentpnt, IShape sh)
        {
            _completedConnections = new List<int>();
            _pntOrigin = new Vector2(_magicPntSource.Width / 2, _magicPntSource.Height / 2);

            foreach (MagicPoint pnt in sh.Points)
            {
                foreach (int i in pnt._connections)
                {
                    _distance = Vector2.Distance(pnt.Location,
                    sh.Points[i].Location);

                    // only draw correct for -90 to 90 degrees
                    if (pnt.Location.X < sh.Points[i].Location.X)
                    {
                        _tempLocRect.X = pnt._location.X;
                        _tempLocRect.Y = pnt._location.Y;
                        _tempDrawCon = new Rectangle((int)pnt.Location.X - (int)cam.Location.X, (int)pnt.Location.Y - (int)_pntOrigin.Y / 2 - (int)cam.Location.Y, (int)_distance, (int)_pntSize.Y / 4);
                        UniversalVariables.spriteBatch.Draw(SpriteSheet, _tempDrawCon, _magicConSource, Color.White, (float)Math.Asin(-(pnt.Location.Y - sh.Points[i].Location.Y) / _distance), Vector2.Zero, SpriteEffects.None, .749f);
                    }
                    else
                    {
                        _tempLocRect.X = sh.Points[i]._location.X;
                        _tempLocRect.Y = sh.Points[i]._location.Y;
                        _tempDrawCon = new Rectangle((int)sh.Points[i].Location.X - (int)cam.Location.X, (int)sh.Points[i].Location.Y - (int)_pntOrigin.Y / 2 - (int)cam.Location.Y, (int)_distance, (int)_pntSize.Y / 4);
                        UniversalVariables.spriteBatch.Draw(SpriteSheet, _tempDrawCon, _magicConSource, Color.White, (float)Math.Asin(-(sh.Points[i].Location.Y - pnt.Location.Y) / _distance), Vector2.Zero, SpriteEffects.None, .749f);
                    }
                }

                pnt.rectonScreen = new Rectangle((int)pnt.Location.X - (int)cam.Location.X, (int)pnt.Location.Y - (int)cam.Location.Y, (int)_pntSize.X, (int)_pntSize.Y);
                //spriteBatch.Draw(SpriteSheet, pnt._tempIntersects, new Rectangle(2042, 2042, 4, 4), Color.Green);
                _tempSource = new Rectangle(_magicPntSource.X + ((_frame + pnt.FrameMod) % 6) * _magicPntSource.Width, _magicPntSource.Y, _magicPntSource.Width, _magicPntSource.Height);
                if (!current && pnt.ID == _currentpnt)
                    UniversalVariables.spriteBatch.Draw(SpriteSheet, pnt.rectonScreen, _tempSource, Color.Yellow, MathHelper.WrapAngle(pnt._rotdir * (_rotation + pnt.RotationMod)), _pntOrigin, SpriteEffects.None, .7499f);
                else if (current)
                    UniversalVariables.spriteBatch.Draw(SpriteSheet, pnt.rectonScreen, _tempSource, Color.BlueViolet, MathHelper.WrapAngle(pnt._rotdir * (_rotation + pnt.RotationMod)), _pntOrigin, SpriteEffects.None, .7499f);
                else
                    UniversalVariables.spriteBatch.Draw(SpriteSheet, pnt.rectonScreen, _tempSource, Color.White, MathHelper.WrapAngle(pnt._rotdir * (_rotation + pnt.RotationMod)), _pntOrigin, SpriteEffects.None, .7499f);
            }
        }

        /// <summary>
        /// Basic Cast Method
        /// </summary>
        public static void Cast(int ShapeID)
        {
            Cast(_shapes[ShapeID]);
        }

        public static void Cast(IShape shpe)
        {

        }

        static Vector2 _temploc;
        public static void Addshape(Vector2 Location, ICamera cam, Random rand, List<Entity> entities, bool entitySearch)
        {
            _temploc = new Vector2(Location.X + cam.Location.X, Location.Y + cam.Location.Y);
            Addshape(_temploc, rand, entities, entitySearch);
        }

        public static void Addshape(Vector2 Location, Random rand, List<Entity> entities, bool entitySearch)
        {
            Shapes.Add(new Shape(Location, rand));
            if (entitySearch)
                CheckEntityCollision(LastShape, entities);
        }

        public static void CheckEntityCollision(IShape sh, List<Entity> entities)
        {
            foreach (MagicPoint pnt in sh.Points)
            {
                foreach (Entity ent in entities)
                    if (ent.BoundingBox.Intersects(Utilities.Vector2ToRect(pnt.Location, (int)_pntSize.X, (int)_pntSize.Y)))
                    {
                        sh.Target = ent;
                        sh.PointAttachment = pnt.ID;
                        return;
                    }
            }
        }

        public static void CombineShape(IShape blank, int indexA, int indexB, int ConnectingPntA, int ConnectingPntB, List<Entity> entities, bool indexBInit)
        {
            if (!indexBInit)
                if (Shapes[indexA].Target == null)
                    CheckEntityCollision(_shapes[indexB], entities);

            Shapes.Add(IShapeMethods.Combine(blank, Shapes[indexA], Shapes[indexB], ConnectingPntA, ConnectingPntB));
            Shapes.RemoveAt(indexB);
            Shapes.RemoveAt(indexA);
        }

        public static void CombineShape(IShape blank, IShape shapeA, IShape shapeB, int ConnectingPntA, int ConnectingPntB, List<Entity> entities, bool indexBInit)
        {
            if (!indexBInit)
                if (shapeA.Target == null)
                    CheckEntityCollision(shapeB, entities);

            Shapes.Add(IShapeMethods.Combine(blank, shapeA, shapeB, ConnectingPntA, ConnectingPntB));
            Shapes.Remove(shapeB);
            Shapes.Remove(shapeA);
        }

        public static void AddInternalConnection(int pntA, int pntB, int shpeIndex)
        {
            AddInternalConnection(pntA, pntB, _shapes[shpeIndex]);
        }

        public static void AddInternalConnection(int pntA, int pntB, IShape shpe)
        {
            shpe.Points[pntA].Connections.Add(pntB);
            shpe.Points[pntB].Connections.Add(pntA);
            shpe.Connections++;
        }

        public static IShape RemovePoint(int pntID, IShape shpe)
        {
            foreach (MagicPoint pnt in shpe.Points)
            {
                if (pnt.ID != pntID)
                {
                    if (pnt.ID > pntID)
                        pnt.ID--;
                    if (pnt.Connections.Contains(pntID))
                        pnt.Connections.Remove(pntID);
                    for (int i = 0; i < pnt.Connections.Count; i++)
                        if (i > pntID)
                            pnt.Connections[i]--;
                }
            }

            if (shpe.PointAttachment > pntID)
                shpe.PointAttachment--;
            if (shpe.PointAttachment == pntID)
            {
                shpe.Target = null;
                shpe.PointAttachment = -1;
            }

            shpe.Points.RemoveAt(pntID);

            return shpe;
        }

        public static void removeConnection(int con, int pnt, IShape shpe)
        {
            shpe.Points[pnt].Connections.RemoveAt(con);
            shpe.Points[con].Connections.RemoveAt(pnt);
        }
        #endregion
    }
}
