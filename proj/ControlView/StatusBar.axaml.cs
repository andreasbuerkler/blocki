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

            NotificationCenter.Instance.AddObserver(StatusChangedNotification, Notification.Id.StatusChanged);

            _xPos = this.FindControl<TextBlock>("xPos");
            _yPos = this.FindControl<TextBlock>("yPos");
            _zoom = this.FindControl<TextBlock>("zoom");

            _xPos.Text = "x 0";
            _yPos.Text = "y 0";
            _zoom.Text = "100%";
        }

        private void StatusChangedNotification(Notification notification)
        {
            StatusChanged message = (StatusChanged)notification.Message;
            if (message.xPos != null)
            {
                _xPos.Text = "x " + message.xPos.ToString();
            }
            if (message.yPos != null)
            {
                _yPos.Text = "y " + message.yPos.ToString();
            }
            if (message.zoomPercent != null)
            {
                _zoom.Text = message.zoomPercent.ToString() + "%";
            }
        }

        private TextBlock _xPos;
        private TextBlock _yPos;
        private TextBlock _zoom;
    }
}
