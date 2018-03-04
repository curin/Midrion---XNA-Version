using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.dbtgaming.Library.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace com.dbtgaming.Game.Library.MapObjects
{
    public struct MapRegion
    {
        private Vector2 _position;
        private string _regionID;
        private string _originalRegion;
        private List<ObjectChange> _ChangedObjects;
        private Dictionary<string, RegionObject> _regionObjects;
        private List<ISprite2> _usedSprites;
        private Dictionary<string, Doorway> _doors;
        private Rectangle _sourceRect;
        private Vector2 _scale;
        private Vector2 _colCorrection;
        private Color _drawColor;
        private Texture2D _Backgroundtex;
        private Rectangle _colSource;
        private Texture2D _coltex;
        private List<PlaceOfPower> _PoPs;
        private Vector2 _size;

        /// <summary>
        /// Location on map to consider as 0,0 for this map
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        /// <summary>
        /// ID of this Region and the map its found in (MapID.RegionID)
        /// </summary>
        public string RegionID
        {
            get
            {
                return _regionID;
            }
        }

        /// <summary>
        /// The MapID and RegionID of the original region being modified(MapID.RegionID).
        /// Leave blank if this is a new region.
        /// </summary>
        public string OriginalRegion
        {
            get
            {
                return _originalRegion;
            }
        }

        /// <summary>
        /// Any Changes Made to objects in the original Region
        /// Leave empty if this is a new region
        /// </summary>
        public List<ObjectChange> Changes
        {
            get
            {
                return _ChangedObjects;
            }
        }

        /// <summary>
        /// Any Objects added to this region with id as key
        /// </summary>
        public Dictionary<string, RegionObject> RegionObjects
        {
            get
            {
                return _regionObjects;
            }
            set
            {
                _regionObjects = value;
            }
        }

        /// <summary>
        /// Sprite Bases used in this region include doorway bases as well as region object bases
        /// </summary>
        public List<ISprite2> SpritesUsed
        {
            get
            {
                return _usedSprites;
            }
            set
            {
                _usedSprites = value;
            }
        }

        /// <summary>
        /// Any Added Doors, Portals, etc.(Devices that when walked over teleport the player) added to the region with id as key
        /// </summary>
        public Dictionary<string, Doorway> Doors
        {
            get
            {
                return _doors;
            }
            set
            {
                _doors = value;
            }
        }

        /// <summary>
        /// List of all Places of Power(AKA PoPs)
        /// </summary>
        public List<PlaceOfPower> PoPs
        {
            get
            {
                return _PoPs;
            }
            set
            {
                _PoPs = value;
            }
        }

        /// <summary>
        /// the size of the Region/its Background;
        /// </summary>
        public Vector2 Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
            }
        }
    }

    public struct ObjectChange
    {
        private string _iD;
        private ObjectChangeState _state;
        private ISpriteInstance _changes;

        /// <summary>
        /// Original Object ID (MapID.RegionID.ObjectID)
        /// </summary>
        public string ID
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
        /// How was the Object Changed
        /// </summary>
        public ObjectChangeState State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }

        /// <summary>
        /// For any changes that isn't just deletion
        /// </summary>
        public ISpriteInstance Changes
        {
            get
            {
                return _changes;
            }
            set
            {
                _changes = value;
            }
        }
    }

    /// <summary>
    /// How the the object Changed
    /// </summary>
    public enum ObjectChangeState
    {
        /// <summary>
        /// Object Deleted
        /// </summary>
        Deleted,
        /// <summary>
        /// Object Scaled, or otherwise goes through a transformation
        /// </summary>
        Transformed,
        /// <summary>
        /// Object was Moved
        /// </summary>
        Translated,
        /// <summary>
        /// Object was both Transformed and Translated
        /// </summary>
        TransformedandTranslated,
        /// <summary>
        /// Object's Base Class was changed
        /// </summary>
        BaseClassChange,
        /// <summary>
        /// Object's Base Class was changed and it was Transformed
        /// </summary>
        BaseClassTransform,
        /// <summary>
        /// Object's Base Class was changed and it was Translated
        /// </summary>
        BaseClassTranslate,
        /// <summary>
        /// Object's Base Class was changed and it was Transformed and Translated
        /// </summary>
        BaseClassTransformTranslate,
        /// <summary>
        /// Another Attribute of the Object was changed(starting frame, current animation, etc.)
        /// </summary>
        AttributeChange,
        /// <summary>
        /// Object Attributes were changed and it was transformed
        /// </summary>
        AttributeChangeTransform,
        /// <summary>
        /// Object Attributes were changed and its Base Class was changed
        /// </summary>
        AttributeChangeBaseClass,
        /// <summary>
        /// Object Attributes were changed and it was translated
        /// </summary>
        AttributeChangeTranslate,
        /// <summary>
        /// Object Attributes were changed and it was translated and its Base Class was Changed
        /// </summary>
        AttributeChangeBaseClassTransform,
        /// <summary>
        /// Object Attributes were changed and it was transformed and its Base Class was Changed
        /// </summary>
        AttributeChangeBaseClassTranslate,
        /// <summary>
        /// Object Attributes were changed and it was transformed and translated
        /// </summary>
        AttributeChangeTransformTranslate,
        /// <summary>
        /// Everything was Changed in someway(translation,transformation,Base Class, Attributes) 
        /// </summary>
        FullEdit
    }

    public struct RegionObject
    {
        private string _iD;
        private ISpriteInstance _objectInstance;

        /// <summary>
        /// The ID of this Region Object with its RegionID and MapID (MapID.RegionID.ObjectID)
        /// </summary>
        public string ID
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
        /// The Sprite instance of this object
        /// </summary>
        public ISpriteInstance ObjectInstance
        {
            get
            {
                return _objectInstance;
            }
            set
            {
                _objectInstance = value;
            }
        }
    }

    public struct Doorway
    {
        private string _iD;
        private string _mapConnectedTo;
        private Vector2 _locationConnectedTo;
        private ISpriteInstance _DoorSprite;

        /// <summary>
        /// The ID of this Doorway and its RegionID and its MapID (MapID.RegionID.DoorwayID)
        /// </summary>
        public string ID
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
        /// The MapID that the exit of the Doorway is on
        /// </summary>
        public string MapConnectedTo
        {
            get
            {
                return _mapConnectedTo;
            }
            set
            {
                _mapConnectedTo = value;
            }
        }

        /// <summary>
        /// The Position on the Map the exit of the Doorway comes out
        /// </summary>
        public Vector2 LocationConnectedTo
        {
            get
            {
                return _locationConnectedTo;
            }
            set
            {
                _locationConnectedTo = value;
            }
        }

        /// <summary>
        /// The Sprite instance used for the Doorway
        /// </summary>
        public ISpriteInstance DoorSprite
        {
            get
            {
                return _DoorSprite;
            }
            set
            {
                _DoorSprite = value;
            }
        }
    }

    /// <summary>
    /// Location that strengthens a specific type of magic or are neutralize other places of power
    /// </summary>
    public struct PlaceOfPower
    {
        private string _iD;
        private byte _strength;
        private Point _location;
        private PoPType _placeType;

        /// <summary>
        /// Identifying String
        /// </summary>
        public string ID
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
        /// the Strength Value of the Place of Power(0 - 255)
        /// </summary>
        public byte Strength
        {
            get
            {
                return _strength;
            }
            set
            {
                _strength = value;
            }
        }

        /// <summary>
        /// The Location of the Place of Power
        /// </summary>
        public Point Location
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

        /// <summary>
        /// The type of Magic empowered at this PoP
        /// </summary>
        public PoPType PlaceType
        {
            get
            {
                return _placeType;
            }
            set
            {
                _placeType = value;
            }
        }
    }

    /// <summary>
    /// The type of Magic Empowered in a Place of Power
    /// </summary>
    public enum PoPType
    {
        Fire,
        Water,
        Earth,
        Air,
        Arcane,
        Holy,
        Protection,
        Darkness,
        Void,
        Dimension,
        Summoning,
        Neutral
    }
}
