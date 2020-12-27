using System;
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
            NotificationCenter.Instance.AddObserver(OnZoomChangedNotification, Notification.Id.ZoomChanged);

            _xPos = this.FindControl<TextBlock>("xPos");
            _yPos = this.FindControl<TextBlock>("yPos");
            _zoom = this.FindControl<TextBlock>("zoom");
        }

        private void OnCursorChangedNotification(Notification notification)
        {
            CursorChanged message = (CursorChanged)notification.Message;
            _xPos.Text = "x:" + message.xPos.ToString();
            _yPos.Text = "y:" + message.yPos.ToString();
        }

        private void OnZoomChangedNotification(Notification notification)
        {
            ZoomChanged message = (ZoomChanged)notification.Message;
            int zoomPercent = Convert.ToInt32(message.zoom * 100.0);
            _zoom.Text = zoomPercent.ToString() + "%";
        }

        private TextBlock _xPos;
        private TextBlock _yPos;
        private TextBlock _zoom;
    }
}
