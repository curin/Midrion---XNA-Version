using com.dbtgaming.Library.UI;
using Microsoft.Xna.Framework;

namespace com.dbtgaming.Library.GSM
{
    public class BasicCam : ICamera
    {
        protected Vector2 _location;
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

        protected Rectangle _viewport;
        public Rectangle Viewport
        {
            get
            {
                return _viewport;
            }
            set
            {
                _viewport = value;
            }
        }

        public void Update()
        {

        }
    }
}
