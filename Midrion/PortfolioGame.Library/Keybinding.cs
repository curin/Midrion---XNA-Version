using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.dbtgaming.Library
{
    public struct Keybinding
    {
        string _id;
        public string ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        string _category;
        public string Category
        {
            get
            {
                return _category;
            }
            set
            {
                _category = value;
            }
        }

        string[] _keys;
        public string[] Keys
        {
            get
            {
                return _keys;
            }
            set
            {
                _keys = value;
            }
        }

        bool _pulse;
        public bool Pulse
        {
            get
            {
                return _pulse;
            }
            set
            {
                _pulse = value;
            }
        }

        bool _toggle;
        public bool Toggle
        {
            get
            {
                return _toggle;
            }
            set
            {
                _toggle = value;
            }
        }

        bool _togglePulse;
        public bool TogglePulse
        {
            get
            {
                return _togglePulse;
            }
            set
            {
                _togglePulse = value;
            }
        }

        bool _held;
        public bool Held
        {
            get
            {
                return _held;
            }
            set
            {
                _held = value;
            }
        }

        bool _repeat;
        public bool Repeat
        {
            get
            {
                return _repeat;
            }
            set
            {
                _repeat = value;
            }
        }
    }
}
