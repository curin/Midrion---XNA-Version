using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace com.dbtgaming.Library
{
    public class InputManager
    {
        public InputManager(string breaking)
        { }

        private static Hashtable _keybindings = new Hashtable();

        //initialize Variables and Properties
        static KeyboardState _keb;
        /// <summary>
        /// Keyboard state used in input manager
        /// </summary>
        public static KeyboardState Keb
        {
            get
            {
                return _keb;
            }
            set
            {
                _keb = value;
            }
        }

        static MouseState _mou;
        /// <summary>
        /// Mouse state used in input Manager
        /// </summary>
        public static MouseState Mou
        {
            get
            {
                return _mou;
            }
            set
            {
                _mou = value;
            }
        }

        static bool _caps = false;
        /// <summary>
        /// is capslock on?
        /// </summary>
        public static bool Caps
        {
            get
            {
                return _caps;
            }
        }

        static bool _held = false;

        /// <summary>
        /// used for scroll wheel
        /// </summary>
        static float _deltaScrollWheel = 0f;
        public static float DeltaScrollWheel
        {
            get
            {
                return _deltaScrollWheel;
            }
        }

        static float _lastScrollWheel = 0f;

        static Vector2 _lastMousePosition = new Vector2(0f, 0f);

        static Vector2 _deltaMousePosition;
        /// <summary>
        /// how far has the mouse moved?
        /// </summary>
        public static Vector2 DeltaMousePosition
        {
            get
            {
                return _deltaMousePosition;
            }
        }

        public static bool Shift
        {
            get
            {
                return _keb.IsKeyDown(Keys.LeftShift) || _keb.IsKeyDown(Keys.RightShift);
            }
        }

        public static bool Control
        {
            get
            {
                return _keb.IsKeyDown(Keys.LeftControl) || _keb.IsKeyDown(Keys.RightControl);
            }
        }

        public static bool Alt
        {
            get
            {
                return _keb.IsKeyDown(Keys.LeftAlt) || _keb.IsKeyDown(Keys.RightAlt);
            }
        }

        static Vector2 _temp;

        static List<Keybinding> _tempbindlist = new List<Keybinding>();
        /// <summary>
        /// Update the Keyboard and Mouse States and find change in mouse variables
        /// </summary>
        public static void Update()
        {
            _keb = Keyboard.GetState();
            _mou = Mouse.GetState();
            _deltaScrollWheel = _mou.ScrollWheelValue - _lastScrollWheel;
            _lastScrollWheel = _mou.ScrollWheelValue;
            _temp = new Vector2(_mou.X, _mou.Y);
            _deltaMousePosition = _temp - _lastMousePosition;
            _lastMousePosition = _temp;

            _tempbindlist.RemoveRange(0, _tempbindlist.Count);
            foreach (Keybinding bind in _keybindings.Values)
                if (!CheckButtonDown(bind.ID))
                {
                    _tempbinding = bind;
                    _tempbinding.Pulse = false;
                    _tempbinding.Held = false;
                    _tempbinding.TogglePulse = false;
                    _tempbindlist.Add(_tempbinding);
                }

            foreach (Keybinding bind in _tempbindlist)
                _keybindings[bind.ID] = bind;
        }

        //create lists mapping keys to characters
        public static Dictionary<Keys, char> lowercase = new Dictionary<Keys, char>();
        public static Dictionary<Keys, char> uppercase = new Dictionary<Keys, char>();

        /// <summary>
        /// is the right mouse button clicked?
        /// </summary>
        public static bool RightMouseButtonDown
        {
            get
            {
                return _mou.RightButton == ButtonState.Pressed;
            }
        }

        /// <summary>
        /// is the left mouse button clicked?
        /// </summary>
        public static bool LeftMouseButtonDown
        {
            get
            {
                return _mou.LeftButton == ButtonState.Pressed;
            }
        }

        /// <summary>
        /// is the middle mouse button clicked?
        /// </summary>
        public static bool MiddleMouseButtonDown
        {
            get
            {
                return _mou.MiddleButton == ButtonState.Pressed;
            }
        }

        /// <summary>
        /// Populate the keymapping lists
        /// </summary>
        public static void PopulateLists()
        {
            lowercase.Add(Keys.A, 'a');
            lowercase.Add(Keys.B, 'b');
            lowercase.Add(Keys.C, 'c');
            lowercase.Add(Keys.D, 'd');
            lowercase.Add(Keys.E, 'e');
            lowercase.Add(Keys.F, 'f');
            lowercase.Add(Keys.G, 'g');
            lowercase.Add(Keys.H, 'h');
            lowercase.Add(Keys.I, 'i');
            lowercase.Add(Keys.J, 'j');
            lowercase.Add(Keys.K, 'k');
            lowercase.Add(Keys.L, 'l');
            lowercase.Add(Keys.M, 'm');
            lowercase.Add(Keys.N, 'n');
            lowercase.Add(Keys.O, 'o');
            lowercase.Add(Keys.P, 'p');
            lowercase.Add(Keys.Q, 'q');
            lowercase.Add(Keys.R, 'r');
            lowercase.Add(Keys.S, 's');
            lowercase.Add(Keys.T, 't');
            lowercase.Add(Keys.U, 'u');
            lowercase.Add(Keys.V, 'v');
            lowercase.Add(Keys.W, 'w');
            lowercase.Add(Keys.X, 'x');
            lowercase.Add(Keys.Y, 'y');
            lowercase.Add(Keys.Z, 'z');
            lowercase.Add(Keys.OemTilde, '`');
            lowercase.Add(Keys.NumPad1, '1');
            lowercase.Add(Keys.NumPad2, '2');
            lowercase.Add(Keys.NumPad3, '3');
            lowercase.Add(Keys.NumPad4, '4');
            lowercase.Add(Keys.NumPad5, '5');
            lowercase.Add(Keys.NumPad6, '6');
            lowercase.Add(Keys.NumPad7, '7');
            lowercase.Add(Keys.NumPad8, '8');
            lowercase.Add(Keys.NumPad9, '9');
            lowercase.Add(Keys.NumPad0, '0');
            lowercase.Add(Keys.OemMinus, '-');
            lowercase.Add(Keys.OemPlus, '=');
            lowercase.Add(Keys.OemOpenBrackets, '[');
            lowercase.Add(Keys.OemCloseBrackets, ']');
            lowercase.Add(Keys.OemBackslash, '\\');
            lowercase.Add(Keys.OemSemicolon, ';');
            lowercase.Add(Keys.OemQuotes, '\'');
            lowercase.Add(Keys.OemComma, ',');
            lowercase.Add(Keys.OemPeriod, '.');
            lowercase.Add(Keys.OemQuestion, '/');
            uppercase.Add(Keys.A, 'A');
            uppercase.Add(Keys.B, 'B');
            uppercase.Add(Keys.C, 'C');
            uppercase.Add(Keys.D, 'D');
            uppercase.Add(Keys.E, 'E');
            uppercase.Add(Keys.F, 'F');
            uppercase.Add(Keys.G, 'G');
            uppercase.Add(Keys.H, 'H');
            uppercase.Add(Keys.I, 'I');
            uppercase.Add(Keys.J, 'J');
            uppercase.Add(Keys.K, 'K');
            uppercase.Add(Keys.L, 'L');
            uppercase.Add(Keys.M, 'M');
            uppercase.Add(Keys.N, 'N');
            uppercase.Add(Keys.O, 'O');
            uppercase.Add(Keys.P, 'P');
            uppercase.Add(Keys.Q, 'Q');
            uppercase.Add(Keys.R, 'R');
            uppercase.Add(Keys.S, 'S');
            uppercase.Add(Keys.T, 'T');
            uppercase.Add(Keys.U, 'U');
            uppercase.Add(Keys.V, 'V');
            uppercase.Add(Keys.W, 'W');
            uppercase.Add(Keys.X, 'X');
            uppercase.Add(Keys.Y, 'Y');
            uppercase.Add(Keys.Z, 'Z');
            uppercase.Add(Keys.OemTilde, '~');
            uppercase.Add(Keys.NumPad1, '!');
            uppercase.Add(Keys.NumPad2, '@');
            uppercase.Add(Keys.NumPad3, '#');
            uppercase.Add(Keys.NumPad4, '$');
            uppercase.Add(Keys.NumPad5, '%');
            uppercase.Add(Keys.NumPad6, '^');
            uppercase.Add(Keys.NumPad7, '&');
            uppercase.Add(Keys.NumPad8, '*');
            uppercase.Add(Keys.NumPad9, '(');
            uppercase.Add(Keys.NumPad0, ')');
            uppercase.Add(Keys.OemMinus, '_');
            uppercase.Add(Keys.OemPlus, '+');
            uppercase.Add(Keys.OemOpenBrackets, '{');
            uppercase.Add(Keys.OemCloseBrackets, '}');
            uppercase.Add(Keys.OemBackslash, '|');
            uppercase.Add(Keys.OemSemicolon, ':');
            uppercase.Add(Keys.OemQuotes, '\"');
            uppercase.Add(Keys.OemComma, '<');
            uppercase.Add(Keys.OemPeriod, '>');
            uppercase.Add(Keys.OemQuestion, '?');
        }

        static Keybinding _tempbinding;

        public static bool isBeingHeld(string ID)
        {
            _tempbinding = (Keybinding)_keybindings[ID];
            return _tempbinding.Held;
        }

        public static bool getToggle(string ID)
        {
            _tempbinding = (Keybinding)_keybindings[ID];
            return _tempbinding.Toggle;
        }

        public static bool CheckButtonDown(string ID, bool pulse = false, bool hold = false, bool toggle = false)
        {
            if (pulse && hold)
                throw new Exception("You can't pulse ot amd tell it to be held");

            if (!_keybindings.ContainsKey(ID))
                throw new Exception("Invalid Key Binding ID");

            bool ret = true;
            _tempbinding = (Keybinding)_keybindings[ID];

            if (pulse && !_tempbinding.Pulse || !pulse)
            {
                foreach (string st in _tempbinding.Keys)
                {
                    if (st == "shift")
                        ret = Shift;
                    if (st == "ctrl")
                        ret = Control;
                    if (st.Length == 1)
                        if (lowercase.ContainsValue(st[0]))
                            ret = Keb.IsKeyDown(lowercase.FirstOrDefault(x => x.Value == st[0]).Key);
                    if (st == "caps")
                        ret = Keb.IsKeyDown(Keys.CapsLock);
                    if (st == "tab")
                        ret = Keb.IsKeyDown(Keys.Tab);
                    if (st == "alt")
                        ret = Alt;
                    if (st == "del")
                        ret = Keb.IsKeyDown(Keys.Delete);
                    if (st == "enter")
                        ret = Keb.IsKeyDown(Keys.Enter);
                    if (st == "back")
                        ret = Keb.IsKeyDown(Keys.Back);
                    if (st == "up")
                        ret = Keb.IsKeyDown(Keys.Up);
                    if (st == "left")
                        ret = Keb.IsKeyDown(Keys.Left);
                    if (st == "right")
                        ret = Keb.IsKeyDown(Keys.Right);
                    if (st == "down")
                        ret = Keb.IsKeyDown(Keys.Down);
                    if (st == "pgUp")
                        ret = Keb.IsKeyDown(Keys.PageUp);
                    if (st == "pgDn")
                        ret = Keb.IsKeyDown(Keys.PageDown);
                    if (st == "ins")
                        ret = Keb.IsKeyDown(Keys.Insert);
                    if (st == "home")
                        ret = Keb.IsKeyDown(Keys.Home);
                    if (st == "end")
                        ret = Keb.IsKeyDown(Keys.End);
                    if (st == "esc")
                        ret = Keb.IsKeyDown(Keys.Escape);
                    if (st == "f1")
                        ret = Keb.IsKeyDown(Keys.F1);
                    if (st == "f2")
                        ret = Keb.IsKeyDown(Keys.F2);
                    if (st == "f3")
                        ret = Keb.IsKeyDown(Keys.F3);
                    if (st == "f4")
                        ret = Keb.IsKeyDown(Keys.F4);
                    if (st == "f5")
                        ret = Keb.IsKeyDown(Keys.F5);
                    if (st == "f6")
                        ret = Keb.IsKeyDown(Keys.F6);
                    if (st == "f7")
                        ret = Keb.IsKeyDown(Keys.F7);
                    if (st == "f8")
                        ret = Keb.IsKeyDown(Keys.F8);
                    if (st == "f9")
                        ret = Keb.IsKeyDown(Keys.F9);
                    if (st == "f10")
                        ret = Keb.IsKeyDown(Keys.F10);
                    if (st == "f11")
                        ret = Keb.IsKeyDown(Keys.F11);
                    if (st == "f12")
                        ret = Keb.IsKeyDown(Keys.F12);
                    if (st == "prntScr")
                        ret = Keb.IsKeyDown(Keys.PrintScreen);
                    if (st == "mou1")
                        ret = Mou.LeftButton == ButtonState.Pressed;
                    if (st == "mou2")
                        ret = Mou.RightButton == ButtonState.Pressed;
                    if (st == "mou3")
                        ret = Mou.MiddleButton == ButtonState.Pressed;
                    if (!ret)
                        return ret;
                }
                if (ret)
                {
                    if (pulse)
                        _tempbinding.Pulse = true;
                    if (hold)
                        _tempbinding.Held = true;
                    if (toggle && !_tempbinding.TogglePulse)
                        _tempbinding.Toggle = !_tempbinding.Toggle;
                }
            }
            else
            {
                ret = false;
            }
            if (pulse || hold || toggle && !_tempbinding.TogglePulse)
            {
                if (toggle && !_tempbinding.TogglePulse)
                    _tempbinding.TogglePulse = true;
                _keybindings[ID] = _tempbinding;
            }
            return ret;
        }

        public static Keybinding RegisterKeyBinding(string ID, string Category, Keys[] keys, string[] mouseinput, bool Override)
        {
            if (_keybindings.ContainsKey(ID) && !Override)
                return new Keybinding();
            else if (_keybindings.ContainsKey(ID) && Override)
                _keybindings.Remove(ID);
            string[] bindingkeys = new string[keys.Length + mouseinput.Length];
            Keys key;

            for (int i = 0; i < keys.Length; i++)
            {
                key = keys[i];
                if (lowercase.ContainsKey(key))
                    bindingkeys[i] = lowercase[key].ToString();
                if (key == Keys.LeftShift || key == Keys.RightShift)
                    bindingkeys[i] = "shift";
                if (key == Keys.RightControl || key == Keys.LeftControl)
                    bindingkeys[i] = "ctrl";
                if (key == Keys.LeftAlt || key == Keys.RightAlt)
                    bindingkeys[i] = "alt";
                if (key == Keys.Tab)
                    bindingkeys[i] = "tab";
                if (key == Keys.CapsLock)
                    bindingkeys[i] = "caps";
                if (key == Keys.Delete)
                    bindingkeys[i] = "del";
                if (key == Keys.Enter)
                    bindingkeys[i] = "enter";
                if (key == Keys.Back)
                    bindingkeys[i] = "back";
                if (key == Keys.Up)
                    bindingkeys[i] = "up";
                if (key == Keys.Down)
                    bindingkeys[i] = "down";
                if (key == Keys.Left)
                    bindingkeys[i] = "left";
                if (key == Keys.Right)
                    bindingkeys[i] = "right";
                if (key == Keys.PageUp)
                    bindingkeys[i] = "pgUp";
                if (key == Keys.PageDown)
                    bindingkeys[i] = "pgDn";
                if (key == Keys.Insert)
                    bindingkeys[i] = "ins";
                if (key == Keys.Home)
                    bindingkeys[i] = "home";
                if (key == Keys.End)
                    bindingkeys[i] = "end";
                if (key == Keys.Escape)
                    bindingkeys[i] = "esc";
                if (key == Keys.PrintScreen)
                    bindingkeys[i] = "prntScr";
                if (key == Keys.F1)
                    bindingkeys[i] = "f1";
                if (key == Keys.F2)
                    bindingkeys[i] = "f2";
                if (key == Keys.F3)
                    bindingkeys[i] = "f3";
                if (key == Keys.F4)
                    bindingkeys[i] = "f4";
                if (key == Keys.F5)
                    bindingkeys[i] = "f5";
                if (key == Keys.F6)
                    bindingkeys[i] = "f6";
                if (key == Keys.F7)
                    bindingkeys[i] = "f7";
                if (key == Keys.F8)
                    bindingkeys[i] = "f8";
                if (key == Keys.F9)
                    bindingkeys[i] = "f9";
                if (key == Keys.F10)
                    bindingkeys[i] = "f10";
                if (key == Keys.F11)
                    bindingkeys[i] = "f11";
                if (key == Keys.F12)
                    bindingkeys[i] = "f12";
            }
            mouseinput.CopyTo(bindingkeys, keys.Length);
            Keybinding binding = new Keybinding();
            binding.ID = ID;
            binding.Keys = bindingkeys;
            binding.Pulse = false;
            binding.Toggle = false;
            binding.Held = false;
            binding.Repeat = false;
            binding.TogglePulse = false;
            binding.Category = Category;

            Keybinding bind;

            bool found = true;
            foreach (Keybinding kebind in _keybindings.Values)
            {
                if (kebind.Category == Category)
                {
                    found = true;
                    if (kebind.Keys.Length == bindingkeys.Length)
                        foreach (string k in kebind.Keys)
                        {
                            if (!bindingkeys.Contains(k))
                                found = false;
                        }
                    if (found)
                    {
                        bind = (Keybinding)_keybindings[kebind.ID];
                        bind.Repeat = true;
                        _keybindings[kebind.ID] = bind;
                        binding.Repeat = true;
                    }
                }
            }

            _keybindings.Add(ID, binding);

            return binding;
        }

        /// <summary>
        /// Get what keys are pressed in string form
        /// </summary>
        /// <param name="ignoreCaps">Does caplock being on matter</param>
        /// <returns>pressed Keys in string form</returns>
        public static string PressedKeys(bool ignoreCaps)
        {
            string ret = "";
            //Deal with whether capslock is pressed
            if (_held && _keb.IsKeyUp(Keys.CapsLock))
                _held = false;
            if (!_held && _keb.IsKeyDown(Keys.CapsLock))
                switch (_caps)
                {
                    case true:
                        _caps = false;
                        break;
                    case false:
                        _caps = true;
                        break;
                }

            foreach (Keys key in _keb.GetPressedKeys())
            {
                //Check if caps makes a difference
                if (lowercase.ContainsKey(key))
                {
                    switch (ignoreCaps)
                    {
                        case false:
                            if (_keb.IsKeyDown(Keys.LeftShift) || _keb.IsKeyDown(Keys.LeftShift))
                                ret += uppercase[key];
                            else
                                ret += lowercase[key];
                            break;
                        case true:
                            //Check if caps is on or off and act accordingly
                            switch (_caps)
                            {
                                case true:
                                    if (_keb.IsKeyDown(Keys.LeftShift) || _keb.IsKeyDown(Keys.LeftShift))
                                        ret += lowercase[key];
                                    else
                                        ret += uppercase[key];
                                    break;
                                case false:
                                    if (_keb.IsKeyDown(Keys.LeftShift) || _keb.IsKeyDown(Keys.LeftShift))
                                        ret += uppercase[key];
                                    else
                                        ret += lowercase[key];
                                    break;
                            }
                            break;
                    }
                }
            }
            if (_keb.IsKeyDown(Keys.Tab))
                ret += "     ";
            if (_keb.IsKeyDown(Keys.Space))
                ret += " ";
            return ret;
        }

        public static Keybinding EditKeybinding(string ID, Keybinding bind)
        {
            if (_keybindings.ContainsKey(ID))
            {
                bind.ID = ID;
                _keybindings[ID] = ID;
            }
            else
            {
                throw new Exception("Keybinding ID Doesn't Exist. Make Sure a keybinding with that ID has been registered");
            }
            return bind;
        }
    }
}
