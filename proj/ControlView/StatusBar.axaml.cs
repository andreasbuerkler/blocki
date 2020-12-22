using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Blocki.Notifications;

namespace Blocki.ControlView
{
    public class StatusBar : UserControl
    {
        public StatusBar()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            NotificationCenter.Instance.AddObserver(OnCursorChangedNotification, Notification.Id.CursorChanged);

            _xPos = this.FindControl<TextBlock>("xPos");
            _yPos = this.FindControl<TextBlock>("yPos");
        }

        private void OnCursorChangedNotification(Notification notification)
        {
            CursorChanged message = (CursorChanged)notification.Message;
            _xPos.Text = message.xPos.ToString();
            _yPos.Text = message.yPos.ToString();
        }

        private TextBlock _xPos;
        private TextBlock _yPos;
    }
}
