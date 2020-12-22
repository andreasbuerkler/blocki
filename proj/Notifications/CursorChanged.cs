namespace Blocki.Notifications
{
    public class CursorChanged
    {
        public CursorChanged(int x, int y)
        {
            _xPos = x;
            _yPos = y;
        }

        public int xPos
        {
            get { return _xPos; }
        }

        public int yPos
        {
            get { return _yPos; }
        }

        private readonly int _xPos;
        
        private readonly int _yPos;
    }
}
