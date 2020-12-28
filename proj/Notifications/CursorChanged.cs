namespace Blocki.Notifications
{
    public class CursorChanged
    {
        public CursorChanged(int x, int y, bool leftButtonIsPressed, bool leftButtonIsReleased, bool leftButtonHold, bool rightButtonHold)
        {
            _xPos = x;
            _yPos = y;
            _leftButtonIsPressed = leftButtonIsPressed;
            _leftButtonIsReleased = leftButtonIsReleased;
            _leftButtonIsHold = leftButtonHold;
            _rightButtonIsHold = rightButtonHold;
        }

        public int xPos
        {
            get { return _xPos; }
        }

        public int yPos
        {
            get { return _yPos; }
        }

        public bool leftButtonIsPressed
        {
            get { return _leftButtonIsPressed; }
        }

        public bool leftButtonIsReleased
        {
            get { return _leftButtonIsReleased; }
        }

        public bool leftButtonIsHold
        {
            get { return _leftButtonIsHold; }
        }

        public bool rightButtonIsHold
        {
            get { return _rightButtonIsHold; }
        }

        private readonly int _xPos;
        private readonly int _yPos;
        private readonly bool _leftButtonIsPressed;
        private readonly bool _leftButtonIsReleased;
        private readonly bool _leftButtonIsHold;
        private readonly bool _rightButtonIsHold;
    }
}
