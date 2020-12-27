namespace Blocki.Notifications
{
    class DisplayedSizeChanged
    {
        public DisplayedSizeChanged(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public int width
        {
            get { return _width; }
        }

        public int height
        {
            get { return _height; }
        }

        private readonly int _width;
        private readonly int _height;
    }
}
