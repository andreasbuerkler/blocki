namespace Blocki.Helper
{
    public class Definitions
    {
        public enum Orientation
        {
            Left,
            Right,
            Top,
            Bottom,
            Unknown
        }

        public enum ContainerStatus
        {
            Highlighted,
            SelectedForConnection,
            Unknown
        }

        public enum ContainerType
        {
            Block,
            Line,
            Text,
            Grid,
            Unknown
        }

        public enum NotificationId
        {
            CursorChanged,
            ImageChanged,
            ButtonPressed,
            ZoomChanged,
            DisplayedSizeChanged,
            StatusChanged
        }

        public enum ButtonId
        {
            AddBlock,
            Connect,
            Delete,
            Move,
            Save,
            None
        }
    }
}
