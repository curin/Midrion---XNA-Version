using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.dbtgaming.Game.Library.MapObjects
{
    public struct MapSave
    {
        
    }

    public struct MagicTile
    {
        private Dictionary<PoPType, byte> _magicLevels;
        private PoPType _mainMagic;

        public PoPType MainMagic
        {
            get
            {
                return _mainMagic;
            }
            set
            {
                _mainMagic = value;
            }
        }

        public Dictionary<PoPType, byte> MagicLevels
        {
            get
            {
                return _magicLevels;
            }
            set
            {
                _magicLevels = value;
            }
        }
    }
}
