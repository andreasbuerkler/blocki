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
            NotificationCenter.Instance.AddObserver(OnImageChangedNotification, Notification.Id.ImageChanged);
        }

        private void OnImageClick(object sender, PointerPressedEventArgs e)
        {
            var cursorPosition = e.GetCurrentPoint(_imageCanvas);
            if (cursorPosition.Properties.IsLeftButtonPressed)
            {
                _pressed = true;
                Notification notification = new Notification(new CursorChanged(Convert.ToInt32(cursorPosition.Position.X), Convert.ToInt32(cursorPosition.Position.Y)));
                NotificationCenter.Instance.PostNotification(Notification.Id.CursorChanged, notification);
            }
        }

        private void OnImageReleased(object sender, PointerReleasedEventArgs e)
        {
            var cursorPosition = e.GetCurrentPoint(_imageCanvas);
            if (!cursorPosition.Properties.IsLeftButtonPressed)
            {
                _pressed = false;
            }
        }

        private void OnImageMoved(object sender, PointerEventArgs e)
        {
            if (_pressed)
            {
                PointerPoint cursorPosition = e.GetCurrentPoint(_imageCanvas);
                Notification notification = new Notification(new CursorChanged(Convert.ToInt32(cursorPosition.Position.X), Convert.ToInt32(cursorPosition.Position.Y)));
                NotificationCenter.Instance.PostNotification(Notification.Id.CursorChanged, notification);
            }
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
        private bool _pressed = false;
    }
}
