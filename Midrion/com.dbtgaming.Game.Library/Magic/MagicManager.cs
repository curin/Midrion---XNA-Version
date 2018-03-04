using com.dbtgaming.Game.Library.MapObjects;
using com.dbtgaming.Library;
using com.dbtgaming.Library.Engine2D.Actors;
using com.dbtgaming.Library.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace com.dbtgaming.Game.Library.Magic
{
    public class MagicManager
    {
        public MagicManager(Texture2D Spritesheet, Rectangle _magicModesource,
            Rectangle magicpointsource, Rectangle magicconnectionsource)
        {
            _magicSpriteSheet = Spritesheet;
            _mmSource = _magicModesource;
            ShapeRegistry._magicPntSource = magicpointsource;
            ShapeRegistry._magicConSource = magicconnectionsource;
            ShapeRegistry.BuildInstabilityList();
            BuildCast();
            RegisterKeybindings();
        }

        public void RegisterKeybindings()
        {
            InputManager.RegisterKeyBinding("Cast Spell", "Play.Magic Manager", new Keys[] { Keys.C }, new string[0], false);
            InputManager.RegisterKeyBinding("Open Magic Mode", "Play", new Keys[] { Keys.Q }, new string[0], false);
            InputManager.RegisterKeyBinding("Add/Select Spell Point", "Play.Magic Manager", new Keys[] { }, new string[] { "mou1" }, false);
            InputManager.RegisterKeyBinding("Remove Spell Points/Connections", "Play.Magic Manager", new Keys[] { }, new string[] { "mou2" }, false);
            InputManager.RegisterKeyBinding("Clear Spells", "Play.Magic Manager.Admin", new Keys[] { }, new string[] { "mou3" }, false);
            InputManager.RegisterKeyBinding("Speed Creation Mode", "Play.Magic Manager", new Keys[] { Keys.LeftControl }, new string[] { }, false);
            InputManager.RegisterKeyBinding("Spell Separation", "Play.Magic Manager", new Keys[] { Keys.LeftShift }, new string[] { }, false);
        }

        public void BuildCast()
        {
            _castPrefs = new Dictionary<Type, int>();
            _castPrefs.Add(typeof(FireShape), 1);
        }

        Texture2D _magicSpriteSheet;
        /// <summary>
        /// Spritesheet for all magic elements
        /// </summary>
        public Texture2D MagicSpriteSheet
        {
            get
            {
                return _magicSpriteSheet;
            }
            set
            {
                _magicSpriteSheet = value;
            }
        }

        Dictionary<Type, int> _castPrefs = new Dictionary<Type, int>();
        /// <summary>
        /// List to help decide what shape is defined in control
        /// </summary>
        public Dictionary<Type, int> CastPreferences
        {
            get
            {
                return _castPrefs;
            }
            set
            {
                _castPrefs = value;
            }
        }

        Rectangle _mmSource;
        /// <summary>
        /// _magicMode Overlay Source rectangle from spritesheet
        /// </summary>
        public Rectangle _magicModeSource
        {
            get
            {
                return _mmSource;
            }
            set
            {
                _mmSource = value;
            }
        }

        Random _rand = new Random();

        List<IDefinedShape> _definedShapes = new List<IDefinedShape>();
        /// <summary>
        /// _shapes to check against for casting
        /// </summary>
        public List<IDefinedShape> DefinedShapes
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

        List<RecognizedShape> _tempRecognized = new List<RecognizedShape>();

        /// <summary>
        /// Updates Each _shape
        /// </summary>
        public void Update(List<Entity> entities, GameCam cam)
        {
            ShapeRegistry.Update(entities, cam, _lastPointClicked, _lastShapeClicked, _movingPnt, _rand);
        }

        /// <summary>
        /// All temp variables to deal with input
        /// </summary>
        protected bool _magicMode = false;
        protected bool _clicked = false;
        protected int _lastPointClicked = -1;
        protected int _lastShapeClicked = -1;
        protected int _curCombineShpe = -1;
        protected int _curCombinePnt = -1;
        protected IShape _tempShape;
        protected bool _heldControl = false;
        protected int _removeShape = -1;
        protected int _removePnt = -1;
        protected bool _movingPnt = false;
        protected int _moveWait = 0;
        protected Vector2 _lastMouseLoc = new Vector2(0, 0);
        protected IShape _sh;
        protected IShape _temSh;
        protected List<int> _pointsInShape = new List<int>();
        protected List<int> _consInShape = new List<int>();
        protected Dictionary<int, int> _pntDic = new Dictionary<int, int>();
        protected bool _conClicked = false;
        protected int _tempindex = -1;

        /// <summary>
        /// Update based on input
        /// </summary>
        /// <param name="input">input manager</param>
        /// <param name="mous">mous</param>
        /// <param name="cam">Camera</param>
        /// <param name="entities">all entities</param>
        public void InputUpdate(CompMouse mous, GameCam cam, List<Entity> entities)
        {

            if (InputManager.CheckButtonDown("Cast Spell", false, false) && _lastShapeClicked >= 0)
            {
                Identify(ShapeRegistry.Shapes[_lastShapeClicked]);
                ShapeRegistry.Cast(_lastShapeClicked);
                ShapeRegistry.Shapes.RemoveAt(_lastShapeClicked);
                _lastShapeClicked = _lastPointClicked = -1;
            }

            InputManager.CheckButtonDown("Open Magic Mode", false, false, true);
            _magicMode = InputManager.getToggle("Open Magic Mode");

            if (_movingPnt)
                if (!InputManager.CheckButtonDown("Add/Select Spell Point", false, false))
                {
                    Identify(ShapeRegistry.Shapes[_lastShapeClicked]);
                    //if (!_heldControl)
                    //     _lastPointClicked = -1;
                    _movingPnt = false;
                    _moveWait = 0;
                }

            InputManager.CheckButtonDown("Speed Creation Mode", false, true);

            if (InputManager.CheckButtonDown("Add/Select Spell Point", false, false))
            {
                if (!_movingPnt)
                {
                    if (_lastPointClicked >= 0)
                    {
                        if (ShapeRegistry.Shapes[_lastShapeClicked].Points[_lastPointClicked].intersects(mous, ShapeRegistry.PointSize, cam))
                        {
                            if (_moveWait < 5)
                                _moveWait++;
                            else
                            {
                                _moveWait = 0;
                                _movingPnt = true;
                                _lastMouseLoc = mous.Location;
                            }
                        }
                    }
                }
                else
                {
                    ShapeRegistry.Shapes[_lastShapeClicked].Points[_lastPointClicked]._location -= _lastMouseLoc - mous.Location;
                    ShapeRegistry.Shapes[_lastShapeClicked].Points[_lastPointClicked].MoveToLocation = ShapeRegistry.Shapes[_lastShapeClicked].Points[_lastPointClicked]._location;

                    if (ShapeRegistry.Shapes[_lastShapeClicked].Target == null)
                    {
                        foreach (Entity ent in entities)
                        {
                            if (ShapeRegistry.Shapes[_lastShapeClicked].Points[_lastPointClicked].intersects(ent, ShapeRegistry.PointSize, cam))
                            {
                                ShapeRegistry.Shapes[_lastShapeClicked].Target = ent;
                                ShapeRegistry.Shapes[_lastShapeClicked].PointAttachment = _lastPointClicked;
                            }
                        }
                    }
                    else
                    {
                        if (!ShapeRegistry.Shapes[_lastShapeClicked].Points[_lastPointClicked].intersects(ShapeRegistry.Shapes[_lastShapeClicked].Target, ShapeRegistry.PointSize, cam) && ShapeRegistry.Shapes[_lastShapeClicked].PointAttachment == _lastPointClicked)
                        {
                            ShapeRegistry.Shapes[_lastShapeClicked].Target = null;
                            (ShapeRegistry.Shapes[_lastShapeClicked]).FirstTarget = false;
                        }
                    }

                    _lastMouseLoc = mous.Location;
                }
            }

            if (InputManager.CheckButtonDown("Add/Select Spell Point", true, false) && _magicMode)
            {
                _clicked = false;
                _curCombineShpe = -1;
                _curCombinePnt = -1;
                foreach (IShape shpe in ShapeRegistry.Shapes)
                {
                    _temSh = shpe;
                    _tempindex = ShapeRegistry.Shapes.IndexOf(shpe);
                    ShapeRegistry.isonscreen(shpe, cam);
                    if (ShapeRegistry.PointsOnScreen.Count > 0)
                    {
                        foreach (MagicPoint pnt in ShapeRegistry.PointsOnScreen)
                        {
                            if (pnt.intersects(mous, ShapeRegistry.PointSize, cam))
                            {
                                if (_lastPointClicked >= 0)
                                {
                                    if (_lastShapeClicked == ShapeRegistry.Shapes.IndexOf(shpe))
                                    {
                                        bool exists = false;

                                        if (shpe.Points[_lastPointClicked].Connections.Contains(pnt.ID))
                                            exists = true;

                                        if (!exists && _lastPointClicked != shpe.Points.IndexOf(pnt))
                                        {
                                            ShapeRegistry.AddInternalConnection(_lastPointClicked, pnt.ID, shpe);
                                            Identify(shpe);
                                            if (InputManager.isBeingHeld("Speed Creation Mode"))
                                            {
                                                _heldControl = true;
                                                _lastPointClicked = pnt.ID;
                                            }
                                            else
                                                _lastPointClicked = -1;
                                        }
                                        else
                                        {
                                            _lastPointClicked = pnt.ID;
                                        }
                                    }
                                    else
                                    {
                                        _curCombineShpe = ShapeRegistry.Shapes.IndexOf(shpe);
                                        _curCombinePnt = pnt.ID;
                                    }
                                }
                                else
                                {
                                    _lastPointClicked = pnt.ID;
                                    _lastShapeClicked = ShapeRegistry.Shapes.IndexOf(shpe);

                                }
                                _clicked = true;
                            }
                        }
                    }
                }
                if (_tempindex >= 0)
                    ShapeRegistry.Shapes[_tempindex] = _temSh;

                if (!_clicked)
                {
                    if (_heldControl && _lastPointClicked != -1)
                    {
                        ShapeRegistry.Addshape(mous.Location, _rand, entities, false);
                        ShapeRegistry.CombineShape(new Shape(true), _lastShapeClicked, ShapeRegistry.Shapes.Count - 1, _lastPointClicked, 0, entities, false);
                    }
                    else
                    {
                        ShapeRegistry.Addshape(mous.Location, _rand, entities, true);
                    }
                    _lastShapeClicked = ShapeRegistry.Shapes.Count - 1;
                    _lastPointClicked = 0;
                    Identify(ShapeRegistry.Shapes[_lastShapeClicked]);

                    if (InputManager.isBeingHeld("Speed Creation Mode"))
                    {
                        _heldControl = true;
                        _lastPointClicked = ShapeRegistry.Shapes[_lastShapeClicked].Points.Count - 1;
                    }
                    else
                        _curCombinePnt = _curCombineShpe = _lastPointClicked = -1;
                }

                if (_lastShapeClicked != -1 && _lastPointClicked != -1 && _curCombinePnt != -1 && _curCombineShpe != -1)
                {
                    int i = _curCombinePnt + ShapeRegistry.Shapes[_lastShapeClicked].Points.Count;
                    ShapeRegistry.CombineShape(new Shape(true), ShapeRegistry.Shapes[_lastShapeClicked], ShapeRegistry.Shapes[_curCombineShpe], _lastPointClicked, _curCombinePnt, entities, true);

                    _lastShapeClicked = ShapeRegistry.Shapes.Count - 1;
                    if (InputManager.isBeingHeld("Speed Creation Mode"))
                    {
                        _heldControl = true;
                        _lastPointClicked = i;
                        _curCombinePnt = _curCombineShpe = -1;
                    }
                    else
                        _curCombinePnt = _curCombineShpe = _lastPointClicked = -1;
                    Identify(ShapeRegistry.Shapes[_lastShapeClicked]);
                }
            }

            if (InputManager.CheckButtonDown("Remove Spell Points/Connections", true, false) && _magicMode)
            {
                _clicked = false;
                foreach (IShape shpe in ShapeRegistry.Shapes)
                {
                    ShapeRegistry.isonscreen(shpe, cam);
                    if (ShapeRegistry.PointsOnScreen.Count > 0)
                    {
                        foreach (MagicPoint pnt in ShapeRegistry.PointsOnScreen)
                        {
                            if (pnt.intersects(mous, ShapeRegistry.PointSize, cam))
                            {
                                _removePnt = pnt.ID;
                                _removeShape = ShapeRegistry.Shapes.IndexOf(shpe);
                                Identify(shpe);


                                _clicked = true;
                            }
                        }

                    }
                }

                if (_removePnt != -1)
                    ShapeRegistry.Shapes[_removeShape] = ShapeRegistry.RemovePoint(_removePnt, ShapeRegistry.Shapes[_removeShape]);
                if (ShapeRegistry.Shapes[_removeShape].Points.Count == 0)
                {
                    if (_lastShapeClicked == _removeShape)
                        _lastShapeClicked = -1;
                    _lastPointClicked = -1;
                    ShapeRegistry.Shapes.RemoveAt(_removeShape);
                }

                if (_removePnt != -1)
                {
                    if (_lastPointClicked == _removePnt && _lastShapeClicked == _removeShape)
                        _lastPointClicked = -1;
                    _movingPnt = false;
                    _moveWait = 0;
                    _removePnt = -1;
                }

                if (!_clicked)
                {
                    int temin = -1;
                    foreach (Shape shpe in ShapeRegistry.Shapes)
                    {
                        int rem = -1;
                        int pntID = -1;
                        bool done = false;
                        foreach (MagicPoint pnt in shpe.Points)
                        {
                            foreach (int i in pnt.Connections)
                            {
                                if (!done)
                                    if (IShapeMethods.intersectCon(mous, shpe.Points[pnt.ID]._location, shpe.Points[i]._location, ShapeRegistry.PointSize, cam))
                                    {
                                        _temSh = shpe;
                                        temin = ShapeRegistry.Shapes.IndexOf(_temSh);
                                        rem = i;
                                        _clicked = true;
                                        _lastPointClicked = -1;
                                        _movingPnt = false;
                                        _moveWait = 0;
                                        pntID = pnt.ID;
                                        if (InputManager.CheckButtonDown("Spell Separation", false, false))
                                            _lastShapeClicked = -1;
                                        done = true;
                                    }
                            }

                        }

                        if (InputManager.CheckButtonDown("Spell Separation", false, false) && temin > -1)
                        {
                            _pointsInShape.RemoveAll(RemoveAllin);
                            _pointsInShape.Add(rem);
                            foreach (int i in shpe.Points[rem]._connections)
                            {
                                _pointsInShape.Add(rem);
                            }

                            if (_pointsInShape.Count > 1)
                            {
                                CheckforSeparation(_pointsInShape, _temSh, 1, rem);
                            }

                            if (_pointsInShape.Count < _temSh.Points.Count)
                            {
                                _pntDic = new Dictionary<int, int>();
                                _sh = new Shape(true);
                                foreach (int i in _pointsInShape)
                                {
                                    _sh.Points.Add(_temSh.Points[i]);
                                }

                                for (int i = 0; i < _pointsInShape.Count; i++)
                                {
                                    _pntDic.Add(_pointsInShape[i], _sh.Points.IndexOf(_temSh.Points[_pointsInShape[i]]));
                                }

                                for (int i = 0; i < _pointsInShape.Count; i++)
                                {
                                    _temSh.Points.RemoveAt(_pointsInShape[i]);
                                    for (int n = i; n < _pointsInShape.Count; n++)
                                    {
                                        if (_pointsInShape[n] > _pointsInShape[i])
                                            _pointsInShape[n]--;
                                    }
                                }
                            }
                        }
                        if (rem != -1)
                        {
                            ShapeRegistry.removeConnection(rem, pntID, _temSh);
                        }
                    }
                    if (temin > -1)
                    {
                        ShapeRegistry.Shapes[temin] = _temSh;
                        Identify(_temSh);
                        if (InputManager.CheckButtonDown("Spell Separation", false, false) && _sh != null)
                        {
                            Identify(_sh);
                            ShapeRegistry.Shapes.Add(_sh);
                        }
                    }
                }

                if (!_clicked)
                {
                    _lastPointClicked = _lastShapeClicked = -1;
                }
                else
                {
                    if (_removeShape != -1)
                    {
                        ShapeRegistry.Shapes.RemoveAt(_removeShape);
                    }
                    _removeShape = -1;
                }
            }

            if (InputManager.CheckButtonDown("Clear Spells", true, false) && _magicMode)
            {
                _clicked = false;
                foreach (Shape shpe in ShapeRegistry.Shapes)
                {
                    ShapeRegistry.isonscreen(shpe, cam);
                    if (ShapeRegistry.PointsOnScreen.Count > 0)
                    {
                        foreach (MagicPoint pnt in ShapeRegistry.PointsOnScreen)
                        {
                            if (pnt.intersects(mous, ShapeRegistry.PointSize, cam))
                            {
                                _removeShape = ShapeRegistry.Shapes.IndexOf(shpe);
                                _clicked = true;
                            }
                        }
                    }
                }

                if (!_clicked)
                {
                    foreach (Shape shpe in ShapeRegistry.Shapes)
                    {
                        _temSh = shpe;
                        _tempindex = ShapeRegistry.Shapes.IndexOf(shpe);
                        ShapeRegistry.isonscreen(shpe, cam);
                        if (ShapeRegistry.PointsOnScreen.Count > 0)
                        {
                            foreach (MagicPoint pnt in shpe.Points)
                            {
                                foreach (int i in pnt.Connections)
                                {
                                    if (IShapeMethods.intersectCon(mous, shpe.Points[pnt.ID]._location, shpe.Points[i]._location, ShapeRegistry.PointSize, cam))
                                    {
                                        _conClicked = true;
                                        _clicked = true;
                                    }
                                }
                            }
                            if (_conClicked)
                            {
                                _conClicked = false;
                            }
                        }
                    }
                    if (_tempindex >= 0)
                        ShapeRegistry.Shapes[_tempindex] = _temSh;
                }

                if (!_clicked)
                {
                    ShapeRegistry.Shapes = new List<IShape>();
                    _lastShapeClicked = _lastPointClicked = -1;
                }
                else if (_removeShape != -1)
                {
                    ShapeRegistry.Shapes.RemoveAt(_removeShape);
                    if (_removeShape == _lastShapeClicked)
                        _lastShapeClicked = _lastPointClicked = -1;
                    _removeShape = -1;
                    _movingPnt = false;
                    _moveWait = 0;
                }
            }
            _tempindex = -1;
        }

        public void AddShape()
        {

        }

        /// <summary>
        /// method used to empty list
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private static bool RemoveAllin(int i)
        {
            return true;
        }

        List<int> _tempints = new List<int>();

        /// <summary>
        /// Check to see if there is a break in the shape
        /// </summary>
        /// <param name="pntsShpe">Points in the shape</param>
        /// <param name="shpe">Shape object</param>
        /// <param name="start">start index</param>
        /// <param name="rem">rem int</param>
        public void CheckforSeparation(List<int> pntsShpe, IShape shpe, int start, int rem)
        {
            int count = pntsShpe.Count;
            _tempints.RemoveAll(RemoveAllin);
            for (int index = start; index < count; index++)
            {
                foreach (int i in pntsShpe)
                {
                    foreach (int con in shpe.Points[i].Connections)
                    {
                        if (!pntsShpe.Contains(con))
                            _tempints.Add(con);
                    }
                }

                pntsShpe.AddRange(_tempints);
                _tempints.RemoveAll(RemoveAllin);
            }

            if (count != pntsShpe.Count)
            {
                CheckforSeparation(pntsShpe, shpe, count, rem);
            }
        }

        /// <summary>
        /// Draws Each _shape
        /// </summary>
        public void Draw(ICamera cam, MagicTile[,] tiles)
        {
            if (_magicMode)
            {
                // Use MapTiles to draw Magic Powerfield
                UniversalVariables.spriteBatch.Draw(_magicSpriteSheet, new Rectangle(-50, -50, UniversalVariables.graphics.PreferredBackBufferWidth + 100,
    UniversalVariables.graphics.PreferredBackBufferHeight + 100), _mmSource, Color.White, 0, Vector2.Zero, SpriteEffects.None, .748f);
                foreach (Shape _sh in ShapeRegistry.Shapes)
                {
                    ShapeRegistry.Draw(cam, _magicSpriteSheet, _lastShapeClicked != ShapeRegistry.Shapes.IndexOf(_sh), _lastPointClicked, _sh);
                }
            }
        }

        List<RecognizedShape> _tempIdent = new List<RecognizedShape>();


        IPrimalShape _tempPrime;
        /// <summary>
        /// identify the defined _shapes in this IShape
        /// </summary>
        /// <param name="shape">shape to be identified</param>
        public void Identify(IShape shape)
        {
            shape.IdentifiedShapes = new List<IDefinedShape>();
            _tempRecognized = new List<RecognizedShape>();
            int count = 0;
            //Get each ShapeRecognized
            foreach (IDefinedShape defsh in _definedShapes)
            {
                _tempPrime = defsh as IPrimalShape;
                if (_tempPrime != null)
                {
                    count = _tempRecognized.Count;
                    _tempRecognized.AddRange(_tempPrime.Recognize(shape));
                }
            }

            int index = 0;
            //_remove Each Shape which is entirely consumed by another
            while (index < _tempRecognized.Count)
            {
                bool _removed = false;
                for (int i = index + 1; i < _tempRecognized.Count; i++)
                {
                    if (_tempRecognized.Count > i)
                    {
                        _tempIdent = IShapeMethods.Compare(_tempRecognized[index], _tempRecognized[i]);
                        if (_tempIdent.Count == 1 && !_removed)
                        {
                            if (!_tempIdent.Contains(_tempRecognized[index]))
                                _removed = true;
                            else if (!_tempIdent.Contains(_tempRecognized[i]))
                            {
                                _tempRecognized.RemoveAt(i);
                                i--;
                            }
                        }
                    }
                }
                if (_removed)
                {
                    _tempRecognized.RemoveAt(index);
                }
                else
                {
                    index++;
                }
            }
            // Add _shapes to _shape's Identified List
            foreach (RecognizedShape rShape in _tempRecognized)
            {
                shape.IdentifiedShapes.Add(rShape.DefinedShape);
            }
            IShapeMethods.CalculateShapePriority(shape, _castPrefs);
        }

        /// <summary>
        /// Recognizes Secondary Shapes in a magic shape
        /// </summary>
        /// <param name="_shpe">Magic Shape</param>
        /// <param name="SecShape">Secondary shape to be identified</param>
        /// <param name="_rshapes">Recognized shapes found from primal recognition</param>
        /// <returns>Recognized shapes with Secondary shape</returns>
        public List<RecognizedShape> RecognizeSecondary(IShape _shpe, ISecondaryShape SecShape, List<RecognizedShape> _rshapes)
        {
            bool found1 = false;
            bool found2 = false;
            List<RecognizedShape> involvedShape1s = new List<RecognizedShape>();
            List<RecognizedShape> involvedShape2s = new List<RecognizedShape>();
            // Check if Secondary Shape's required primals are in the Ishape
            foreach (RecognizedShape rshape in _rshapes)
            {
                if (rshape.DefinedShape == SecShape.shape1)
                {
                    found1 = true;
                    involvedShape1s.Add(rshape);
                }
                if (rshape.DefinedShape == SecShape.shape2)
                {
                    found2 = true;
                    involvedShape2s.Add(rshape);
                }
            }

            if (found1 && found2)
            {
                //Check that the required primals are touching or connected, if so add secondary shape to list of recognized shape
                RecognizedShape _rshape = new RecognizedShape(SecShape);
                List<int> usedR2 = new List<int>();
                int sh2 = 0;
                bool secondary = false;
                for (int sh1 = 0; sh1 < involvedShape1s.Count; sh1++)
                {
                    for (sh2 = 0; sh2 < involvedShape2s.Count; sh2++)
                    {
                        if (!usedR2.Contains(sh2))
                        {
                            secondary = false;
                            foreach (int pnt in involvedShape1s[sh1].Points)
                            {
                                if (!secondary)
                                    if (involvedShape2s[sh2].Points.Contains(pnt))
                                        secondary = true;
                                if (!secondary)
                                    foreach (int con in _shpe.Points[pnt]._connections)
                                    {
                                        if (involvedShape2s[sh2].Points.Contains(con))
                                            secondary = true;
                                    }
                            }
                            if (secondary)
                            {
                                _rshape.Points = involvedShape1s[sh1].Points;
                                _rshape.OriginCon = involvedShape1s[sh1].OriginCon;
                                _rshape.DestCon = involvedShape1s[sh1].DestCon;
                                _rshape.Points.Concat(involvedShape2s[sh2].Points);
                                _rshape.OriginCon.Concat(involvedShape2s[sh2].OriginCon);
                                _rshape.DestCon.Concat(involvedShape2s[sh2].DestCon);
                                _rshapes.Remove(involvedShape2s[sh2]);
                                _rshapes.Remove(involvedShape1s[sh1]);
                                _rshapes.Add(_rshape);
                                usedR2.Add(sh2);
                                sh2 = -1;
                                sh1++;
                            }
                        }
                    }
                }
            }
            return _rshapes;
        }

        /// <summary>
        /// Recognizes Tertiary Shapes in a magic shape
        /// </summary>
        /// <param name="_shpe">Magic Shape</param>
        /// <param name="TerShape">Tertiary shape to be identified</param>
        /// <param name="_rshapes">Recognized shapes found from Secondary recognition</param>
        /// <returns>Recognized shapes with Tertiary shape</returns>
        public List<RecognizedShape> RecognizeTertiary(IShape _shpe, ITertiaryShape TerShape, List<RecognizedShape> _rshapes)
        {
            bool found1 = false;
            bool found2 = false;
            List<RecognizedShape> involvedShape1s = new List<RecognizedShape>();
            List<RecognizedShape> involvedShape2s = new List<RecognizedShape>();
            // Check if Tertiary Shape's required Shapes are in the Ishape
            foreach (RecognizedShape rshape in _rshapes)
            {
                if (rshape.DefinedShape == TerShape.shape1)
                {
                    found1 = true;
                    involvedShape1s.Add(rshape);
                }
                if (rshape.DefinedShape == TerShape.shape2)
                {
                    found2 = true;
                    involvedShape2s.Add(rshape);
                }
            }

            if (found1 && found2)
            {
                //Check that the required Shapes are touching or connected, if so add Tertiary shape to list of recognized shape
                RecognizedShape _rshape = new RecognizedShape(TerShape);
                List<int> usedR2 = new List<int>();
                int sh2 = 0;
                bool secondary = false;
                for (int sh1 = 0; sh1 < involvedShape1s.Count; sh1++)
                {
                    for (sh2 = 0; sh2 < involvedShape2s.Count; sh2++)
                    {
                        if (!usedR2.Contains(sh2))
                        {
                            secondary = false;
                            foreach (int pnt in involvedShape1s[sh1].Points)
                            {
                                if (!secondary)
                                    if (involvedShape2s[sh2].Points.Contains(pnt))
                                        secondary = true;
                                if (!secondary)
                                    foreach (int con in _shpe.Points[pnt]._connections)
                                    {
                                        if (involvedShape2s[sh2].Points.Contains(con))
                                            secondary = true;
                                    }
                            }
                            if (secondary)
                            {
                                _rshape.Points = involvedShape1s[sh1].Points;
                                _rshape.OriginCon = involvedShape1s[sh1].OriginCon;
                                _rshape.DestCon = involvedShape1s[sh1].DestCon;
                                _rshape.Points.Concat(involvedShape2s[sh2].Points);
                                _rshape.OriginCon.Concat(involvedShape2s[sh2].OriginCon);
                                _rshape.DestCon.Concat(involvedShape2s[sh2].DestCon);
                                _rshapes.Remove(involvedShape2s[sh2]);
                                _rshapes.Remove(involvedShape1s[sh1]);
                                _rshapes.Add(_rshape);
                                usedR2.Add(sh2);
                                sh2 = -1;
                                sh1++;
                            }
                        }
                    }
                }
            }
            return _rshapes;
        }

        /// <summary>
        /// Adds Primal Shapes to defined shapes list
        /// </summary>
        public void AddPrimalShapes()
        {
            _definedShapes.Add(new FireShape());
        }

        /// <summary>
        /// Add Secondary shapes to defined shapes list
        /// </summary>
        public void AddSecondaryShapes()
        {

        }
    }
}
