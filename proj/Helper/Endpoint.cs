using System;

namespace Blocki.Helper
{
    public class Endpoint
    {
        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public Definitions.Orientation Orientation
        {
            get { return _orientation; }
            set { _orientation = value; }
        }

        public int Xpos
        {
            get { return _xPos; }
            set { _xPos = value; }
        }

        public int Ypos
        {
            get { return _yPos; }
            set { _yPos = value; }
        }

        public bool PosValid
        {
            get { return _posValid; }
            set { _posValid = value; }
        }

        private Guid _id;
        private Definitions.Orientation _orientation;
        private int _xPos;
        private int _yPos;
        private bool _posValid;
    }
}
