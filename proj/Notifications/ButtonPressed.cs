namespace Blocki.Notifications
{
    public class ButtonPressed
    {
        public ButtonPressed(Id buttonId)
        {
            _buttonId = buttonId;
        }

        public enum Id
        {
            AddBlock,
            Connect,
            Delete,
            Move,
            Save,
            None
        }

        public Id buttonId
        {
            get { return _buttonId; }
        }

        private readonly Id _buttonId;
    }
}
