namespace Blocki.Notifications
{
    class ZoomChanged
    {
        public ZoomChanged(double zoom)
        {
            _zoom = zoom;
        }

        public double zoom
        {
            get { return _zoom; }
        }

        private readonly double _zoom;
    }
}
