using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Input;
using Avalonia.Svg.Skia;
using Blocki.Notifications;

namespace Blocki.DrawView
{
    public class ImageDisplay : UserControl
    {
        public ImageDisplay()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            _svgResourceImage = this.FindControl<Image>("svgResourceImage");
            _imageCanvas = this.FindControl<Canvas>("imageCanvas");
            _imageCanvas.AddHandler(Canvas.PointerPressedEvent, OnImageClick);
            _imageCanvas.AddHandler(Canvas.PointerReleasedEvent, OnImageReleased);
            _imageCanvas.AddHandler(Canvas.PointerMovedEvent, OnImageMoved);
            _imageCanvas.AddHandler(Canvas.PointerWheelChangedEvent , OnImageWheelChanged);
            NotificationCenter.Instance.AddObserver(OnImageChangedNotification, Notification.Id.ImageChanged);
        }

        private void OnImageClick(object sender, PointerPressedEventArgs e)
        {
            PointerPoint cursorPosition = e.GetCurrentPoint(_imageCanvas);
            bool leftButtonWasPressed = false;
            if (cursorPosition.Properties.IsLeftButtonPressed)
            {
                _leftButtonIsPressed = true;
                leftButtonWasPressed = true;
            }
            if (cursorPosition.Properties.IsRightButtonPressed)
            {
                _RightButtonIsPressed = true;
            }
            if ((cursorPosition.Properties.IsLeftButtonPressed) || (cursorPosition.Properties.IsRightButtonPressed))
            {
                int xPos = Convert.ToInt32(cursorPosition.Position.X);
                int yPos = Convert.ToInt32(cursorPosition.Position.Y);
                Notification notification = new Notification(new CursorChanged(xPos, yPos, leftButtonWasPressed, false, _leftButtonIsPressed, _RightButtonIsPressed));
                NotificationCenter.Instance.PostNotification(Notification.Id.CursorChanged, notification);
            }
        }

        private void OnImageReleased(object sender, PointerReleasedEventArgs e)
        {
            PointerPoint cursorPosition = e.GetCurrentPoint(_imageCanvas);
            bool leftButtonWasReleased = false;
            if (!cursorPosition.Properties.IsLeftButtonPressed)
            {
                _leftButtonIsPressed = false;
                leftButtonWasReleased = true;
            }
            if (!cursorPosition.Properties.IsRightButtonPressed)
            {
                _RightButtonIsPressed = false;
            }
            if ((!cursorPosition.Properties.IsLeftButtonPressed) || (!cursorPosition.Properties.IsRightButtonPressed))
            {
                int xPos = Convert.ToInt32(cursorPosition.Position.X);
                int yPos = Convert.ToInt32(cursorPosition.Position.Y);
                Notification notification = new Notification(new CursorChanged(xPos, yPos, false, leftButtonWasReleased, _leftButtonIsPressed, _RightButtonIsPressed));
                NotificationCenter.Instance.PostNotification(Notification.Id.CursorChanged, notification);
            }
        }

        private void OnImageMoved(object sender, PointerEventArgs e)
        {
            PointerPoint cursorPosition = e.GetCurrentPoint(_imageCanvas);
            int xPos = Convert.ToInt32(cursorPosition.Position.X);
            int yPos = Convert.ToInt32(cursorPosition.Position.Y);
            Notification notification = new Notification(new CursorChanged(xPos, yPos, false, false, _leftButtonIsPressed, _RightButtonIsPressed));
            NotificationCenter.Instance.PostNotification(Notification.Id.CursorChanged, notification);
        }

        private void OnImageWheelChanged(object sender, PointerWheelEventArgs e)
        {
            double zoomFactor = (e.Delta.Y < 0) ? 1.1 : 1.0 / 1.1;
            Notification notification = new Notification(new ZoomChanged(zoomFactor));
            NotificationCenter.Instance.PostNotification(Notification.Id.ZoomChanged, notification);
        }

        private void OnImageChangedNotification(Notification notification)
        {
            ImageChanged message = (ImageChanged)notification.Message;
            SvgSource svg = new SvgSource();
            svg.Picture = svg.FromSvg(message.image);
            SvgImage testImage = new SvgImage();
            testImage.Source = svg;
            _svgResourceImage.Source = testImage;
        }

        private Image _svgResourceImage;
        private Canvas _imageCanvas;
        private bool _leftButtonIsPressed = false;
        private bool _RightButtonIsPressed = false;
    }
}
