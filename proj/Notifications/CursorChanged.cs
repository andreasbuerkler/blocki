namespace Blocki.Notifications
{
    public class CursorChanged
    {
        public CursorChanged(int x, int y, bool pressed, bool released, bool hold)
        {
            _xPos = x;
            _yPos = y;
            _buttonIsPressed = pressed;
            _buttonIsReleased = released;
            _buttonIsHold = hold;
        }

        public int xPos
        {
            get { return _xPos; }
        }

        public int yPos
        {
            get { return _yPos; }
        }

        public bool buttonIsPressed
        {
            get { return _buttonIsPressed; }
        }

        public bool buttonIsReleased
        {
            get { return _buttonIsReleased; }
        }

        public bool buttonIsHold
        {
            get { return _buttonIsHold; }
        }

        private readonly int _xPos;
        private readonly int _yPos;
        private readonly bool _buttonIsPressed;
        private readonly bool _buttonIsReleased;
        private readonly bool _buttonIsHold;
    }
}
