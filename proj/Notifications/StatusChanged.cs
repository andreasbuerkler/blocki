namespace Blocki.Notifications
{
    class StatusChanged
    {
        public StatusChanged(int? zoomPercent, int? xPos, int? yPos)
        {
            _zoomPercent = zoomPercent;
            _xPos = xPos;
            _yPos = yPos;
        }

        public int? zoomPercent
        {
            get { return _zoomPercent; }
        }

        public int? xPos
        {
            get { return _xPos; }
        }

        public int? yPos
        {
            get { return _yPos; }
        }

        private readonly int? _zoomPercent;
        private readonly int? _xPos;
        private readonly int? _yPos;
    }
}
