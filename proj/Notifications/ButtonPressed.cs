using Blocki.Helper;

namespace Blocki.Notifications
{
    public class ButtonPressed
    {
        public ButtonPressed(Definitions.ButtonId buttonId)
        {
            _buttonId = buttonId;
        }

        public Definitions.ButtonId buttonId
        {
            get { return _buttonId; }
        }

        private readonly Definitions.ButtonId _buttonId;
    }
}
