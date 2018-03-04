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
using com.dbtgaming.Library.Engine2D.Actors;
using com.dbtgaming.Library.Engine2D.ActorProperties;
using com.dbtgaming.Library.Engine2D.Collision;
using com.dbtgaming.Library.UI;

namespace com.dbtgaming.Game.Library.Magic
{
    public struct Shape: IShape
    {
        public Shape(bool necessary) 
        {
            _target = null;
            _previousLoc = new Vector2(-1, -1);
            _pointAttachment = -1;
            _points = new List<MagicPoint>();
            _shapePriority = new Dictionary<Type, int>();
            _definedAttributes = new Dictionary<string, int>();
            _castColorMod = Color.White;
            _complexity = 0;
            _damages = new Dictionary<string, int>();
            _definedAttributes = new Dictionary<string, int>();
            _finishedId = new List<int>();
            _firstTarget = false;
            _frame = 0;
            _connections = 0;
            _identifiedShapes = new List<IDefinedShape>();
            _instabilityPoints = 0;
            _instabilityProducts = new List<InstabilityProduct>();
            _speed = .1f;
            _owners = new List<ICaster>();
            _rotation = 0;
        }

        public Shape(Vector2 startingpnt, Random random)
        {
            _target = null;
            _previousLoc = new Vector2(-1, -1);
            _pointAttachment = -1;
            _points = new List<MagicPoint>();
            _shapePriority = new Dictionary<Type, int>();
            _definedAttributes = new Dictionary<string, int>();
            _castColorMod = Color.White;
            _complexity = 0;
            _damages = new Dictionary<string, int>();
            _definedAttributes = new Dictionary<string, int>();
            _finishedId = new List<int>();
            _firstTarget = false;
            _frame = 0;
            _connections = 0;
            _identifiedShapes = new List<IDefinedShape>();
            _instabilityPoints = 0;
            _instabilityProducts = new List<InstabilityProduct>();
            _speed = .1f;
            _owners = new List<ICaster>();
            _rotation = 0;
            MagicPoint pnt = new MagicPoint();
            pnt.ID = 0;
            pnt.Connections = new List<int>();
            pnt.Location = startingpnt;
            pnt.FrameMod = random.Next(0, 5);
            pnt.RotationMod = (float)(2 * Math.PI * random.NextDouble());
            pnt._rotdir = random.Next(0, 2);
            if (pnt._rotdir == 0)
                pnt._rotdir = -1;
            else
                pnt._rotdir = 1;
            pnt.MoveToLocation = startingpnt;
            _points.Add(pnt);
        }

        List<MagicPoint> _points;
        /// <summary>
        /// List of every point in this shape
        /// </summary>
        public List<MagicPoint> Points 
        { 
            get
            {
                return _points;
            }
            set
            {
                _points = value;
            }
        }

        Dictionary<Type, int> _shapePriority;
        /// <summary>
        /// Priority values to identified shapes
        /// </summary>
        public Dictionary<Type, int> ShapePriority
        {
            get
            {
                return _shapePriority;
            }
            set
            {
                _shapePriority = value;
            }
        }

        internal Dictionary<string, int> _definedAttributes;
        /// <summary>
        /// Attributed used by Defined Shapes in Update and for cast
        /// </summary>
        public Dictionary<string, int> DefinedAttributes 
        { 
            get
            {
                return _definedAttributes;
            }
            set
            {
                _definedAttributes = value;
            }
        }

        internal Dictionary<string, int> _damages;
        /// <summary>
        /// A place to store damages of different types during the casting process
        /// </summary>
        public Dictionary<string, int> Damages 
        { 
            get
            {
                return _damages;
            }
            set
            {
                _damages = value;
            }
        }

        internal Color _castColorMod;
        public Color CastColorMod
        {
            get
            {
                return _castColorMod;
            }
            set
            {
                _castColorMod = value;
            }
        }

        List<ICaster> _owners;
        /// <summary>
        /// Who all can easily affect this shape
        /// </summary>
        public List<ICaster> Owners
        {
            get
            {
                return _owners;
            }
            set
            {
                _owners = value;
            }
        }

        List<IDefinedShape> _identifiedShapes;
        /// <summary>
        /// Defined Shapes that were found in this Shape
        /// </summary>
        public List<IDefinedShape> IdentifiedShapes 
        { 
            get
            {
                return _identifiedShapes;
            }
            set
            {
                _identifiedShapes = value;
            }
        }

        int _connections;
        /// <summary>
        /// Connection between each point
        /// </summary>
        public int Connections
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

        int _frame;
        /// <summary>
        /// Current frame each animation is on
        /// </summary>
        public int Frame
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

        float _rotation;
        public float Rotation
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

        float _speed;
        /// <summary>
        /// speed at which the animations are going
        /// </summary>
        public float Speed
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

        int _complexity;
        /// <summary>
        /// Complexity of this shape (will be added to by outside actions also)
        /// </summary>
        public int Complexity
        {
            get
            {
                return _complexity;
            }
            set
            {
                _complexity = value;
            }
        }

        int _instabilityPoints;
        /// <summary>
        /// The amount points avaible to create different instabilities
        /// </summary>
        public int InstabilityPoints
        {
            get
            {
                return _instabilityPoints;
            }
            set
            {
                _instabilityPoints = value;
            }
        }

        List<InstabilityProduct> _instabilityProducts;
        /// <summary>
        /// The Methods built to cause the products of instability
        /// </summary>
        public List<InstabilityProduct> InstabilityProducts
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

        Vector2 _previousLoc;
        /// <summary>
        /// Previous location of the mouse
        /// </summary>
        public Vector2 PreviousLoc
        {
            get
            {
                return _previousLoc;
            }
            set
            {
                _previousLoc = value;
            }
        }

        bool _firstTarget;
        public bool FirstTarget
        {
            get
            {
                return _firstTarget;
            }
            set
            {
                _firstTarget = value;
            }
        }

        List<int> _finishedId;
        public List<int> FinishedId
        {
            get
            {
                return _finishedId;
            }
            set
            {
                _finishedId = value;
            }
        }

        Entity _target;
        /// <summary>
        /// Entity Shape is following (only one can be followed at a time)
        /// </summary>
        public Entity Target
        {
            get
            {
                return _target;
            }
            set
            {
                _target = value;
            }
        }

        int _pointAttachment;
        /// <summary>
        /// Which point is attached to the entity
        /// </summary>
        public int PointAttachment
        {
            get
            {
                return _pointAttachment;
            }
            set
            {
                _pointAttachment = value;
            }
        }

        //protected internal Dictionary<Type, int> _shapePriority = new Dictionary<Type, int>();
        //public Dictionary<Type, int> ShapePriority
        //{
        //    get
        //    {
        //        return _shapePriority;
        //    }
        //    set
        //    {
        //        _shapePriority = value;
        //    }
        //}
    }
}
