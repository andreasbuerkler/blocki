namespace Blocki.Notifications
{
    public class Notification
    {
        public enum Id
        {
            CursorChanged,
            ImageChanged,
            ButtonPressed,
            ZoomChanged,
            DisplayedSizeChanged,
            StatusChanged
        }

        public Notification(object message)
        {
            _message = message;
        }

        public object Message
        {
            get { return _message; }
        }

        private readonly object _message;
    }
}
