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

namespace com.dbtgaming.Library.UI
{
    public class InterfaceManager
    {
        private List<ISprite> _interfaceSprites = new List<ISprite>();
        private List<int> _depth = new List<int>();
        int highest = 0;
        public ISprite this[int i]
        {
            get
            {
                return _interfaceSprites[i];
            }
        }

        public int IndexOf(ISprite sprite)
        {
            return _interfaceSprites.IndexOf(sprite);
        }

        public int DepthOf(ISprite sprite)
        {
            return _depth[_interfaceSprites.IndexOf(sprite)];
        }

        public ISprite AtDepth(int depth)
        {
            return _interfaceSprites[_depth.IndexOf(depth)];
        }

        public void Add(int depth, ISprite sprite)
        {
            if (depth > highest)
                highest = depth;
            _depth.Add(depth);
            _interfaceSprites.Add(sprite);
            for (int i = _depth.Count - 2; i > -1; i--)
            {
                if (_depth[i] >= depth)
                    _depth[i]++;
            }
        }

        public void ChangeDepth(int depth, ISprite sprite)
        {
            int index = _interfaceSprites.IndexOf(sprite);
            if (_depth[index] == highest)
                highest--;
            if (depth > highest)
                highest = depth;
            if (_depth[index] > depth)
            {
                for (int i = 0; i < _depth.Count; i++)
                {
                    if (i != index)
                        if (_depth[i] >= depth && _depth[i] < _depth[index])
                            _depth[i]++;
                }
                _depth[index] = depth;
            }
            else if (_depth[index] < depth)
            {
                for (int i = 0; i < _depth.Count; i++)
                {
                    if (i != index)
                        if (_depth[i] <= depth && _depth[i] > _depth[index])
                            _depth[i]--;
                }
                _depth[index] = depth;
            }
        }

        public void Remove(ISprite sprite)
        {
            int index = _interfaceSprites.IndexOf(sprite);
            if (_depth[index] == highest)
                highest--;
            _depth.RemoveAt(index);
            _interfaceSprites.RemoveAt(index);
        }

        public void Update()
        {
            foreach (IAdvancedSprite sprite in _interfaceSprites)
            {
                sprite.Update();
            }
        }

        public void Draw(ICamera Camera)
        {
            int depth = 0;
            while (depth <= highest)
            {
                _interfaceSprites[_depth.IndexOf(depth)].Draw(Camera);
                depth++;
            }
        }
    }
}
