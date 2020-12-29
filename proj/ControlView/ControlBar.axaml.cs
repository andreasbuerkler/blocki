using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Avalonia.Media;
using Blocki.Notifications;

namespace Blocki.ControlView
{
    public class ControlBar : UserControl
    {
        public ControlBar()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            _addButton = this.Find<Button>("addButton");
            _connectButton = this.Find<Button>("connectButton");
            _deleteButton = this.Find<Button>("deleteButton");
            _moveButton = this.Find<Button>("moveButton");
            _saveButton = this.Find<Button>("saveButton");

            SetButtonColor(null);
            _saveButton.Background = _inactiveButtonColor;
        }

        private void OnAddButtonClick(object sender, RoutedEventArgs e)
        {
            SetButtonColor((Button)sender);
            Notification notification = new Notification(new ButtonPressed(ButtonPressed.Id.AddBlock));
            NotificationCenter.Instance.PostNotification(Notification.Id.ButtonPressed, notification);
        }

        private void OnConnectButtonClick(object sender, RoutedEventArgs e)
        {
            SetButtonColor((Button)sender);
            Notification notification = new Notification(new ButtonPressed(ButtonPressed.Id.Connect));
            NotificationCenter.Instance.PostNotification(Notification.Id.ButtonPressed, notification);
        }

        private void OnDeleteButtonClick(object sender, RoutedEventArgs e)
        {
            SetButtonColor((Button)sender);
            Notification notification = new Notification(new ButtonPressed(ButtonPressed.Id.Delete));
            NotificationCenter.Instance.PostNotification(Notification.Id.ButtonPressed, notification);
        }

        private void OnMoveButtonClick(object sender, RoutedEventArgs e)
        {
            SetButtonColor((Button)sender);
            Notification notification = new Notification(new ButtonPressed(ButtonPressed.Id.Move));
            NotificationCenter.Instance.PostNotification(Notification.Id.ButtonPressed, notification);
        }

        private void OnSaveButtonClick(object sender, RoutedEventArgs e)
        {
            Notification notification = new Notification(new ButtonPressed(ButtonPressed.Id.Save));
            NotificationCenter.Instance.PostNotification(Notification.Id.ButtonPressed, notification);
        }

        private void SetButtonColor(Button activeButton)
        {
            _addButton.Background = _inactiveButtonColor;
            _connectButton.Background = _inactiveButtonColor;
            _deleteButton.Background = _inactiveButtonColor;
            _moveButton.Background = _inactiveButtonColor;
            if (activeButton != null)
            {
                activeButton.Background = _activeButtonColor;
            }
        }

        private readonly IBrush _inactiveButtonColor = new SolidColorBrush(Color.FromUInt32(0xFF404040), 1.0);
        private readonly IBrush _activeButtonColor = new SolidColorBrush(Colors.CornflowerBlue, 0.4);
        private Button _addButton;
        private Button _connectButton;
        private Button _deleteButton;
        private Button _moveButton;
        private Button _saveButton;
    }
}
