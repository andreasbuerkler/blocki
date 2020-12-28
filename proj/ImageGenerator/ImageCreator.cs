using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Blocki.Notifications;

namespace Blocki.ImageGenerator
{
    public class ImageCreator
    {
        public ImageCreator()
        {
            NotificationCenter.Instance.AddObserver(OnCursorChangedNotification, Notification.Id.CursorChanged);
            NotificationCenter.Instance.AddObserver(OnButtonPressedNotification, Notification.Id.ButtonPressed);
            NotificationCenter.Instance.AddObserver(OnDisplayedSizeChangedNotification, Notification.Id.DisplayedSizeChanged);
            NotificationCenter.Instance.AddObserver(OnZoomChangedNotification, Notification.Id.ZoomChanged);
        }

        private void UpdateZoomViewBox(int xPos, int yPos, double zoomDiff)
        {
            double widthRatio = Convert.ToDouble(xPos) / Convert.ToDouble(_width);
            double widthOffset = _xStart + (_widthZoom * widthRatio);
            double heightRatio = Convert.ToDouble(yPos) / Convert.ToDouble(_height);
            double heightOffset = _yStart + (_heightZoom * heightRatio);

            _widthZoom = _width * _zoomFactor;
            _heightZoom = _height * _zoomFactor;

            _xStart -= widthOffset;
            _yStart -= heightOffset;

            _xStart = _xStart * zoomDiff;
            _yStart = _yStart * zoomDiff;

            _xStart += widthOffset;
            _yStart += heightOffset;

            _container.SetViewBox(Convert.ToInt32(_xStart), Convert.ToInt32(_yStart), Convert.ToInt32(_widthZoom), Convert.ToInt32(_heightZoom));
        }

        private void UpdatePanViewBox(int xPos, int yPos)
        {
            int xDiff = _lastXpos - xPos;
            int yDiff = _lastYpos - yPos;
            _xStart += Convert.ToDouble(xDiff) * _zoomFactor;
            _yStart += Convert.ToDouble(yDiff) * _zoomFactor;
            _container.SetViewBox(Convert.ToInt32(_xStart), Convert.ToInt32(_yStart), Convert.ToInt32(_widthZoom), Convert.ToInt32(_heightZoom));
        }

        private void OnCursorChangedNotification(Notification notification)
        {
            CursorChanged message = (CursorChanged)notification.Message;
            int xPosViewBox = Convert.ToInt32(_xStart + (message.xPos * _zoomFactor));
            int yPosViewBox = Convert.ToInt32(_yStart + (message.yPos * _zoomFactor));
            bool imageNeedsUpdate = false;

            if ((message.leftButtonIsPressed) && (_activeButton == ButtonPressed.Id.AddBlock))
            {
                _container.AddBlock(xPosViewBox, yPosViewBox);
                imageNeedsUpdate = true;
            }
            if ((message.leftButtonIsPressed) && (_activeButton == ButtonPressed.Id.Delete) && (_activeBlockId >= 0))
            {
                _container.DeleteBlock(_activeBlockId);
                imageNeedsUpdate = true;
            }

            if (!message.leftButtonIsHold)
            {
                if (_container.SelectBlock(xPosViewBox, yPosViewBox, out _activeBlockId))
                {
                    imageNeedsUpdate = true;
                }
            }
            else if ((_activeButton == ButtonPressed.Id.Move) && (_activeBlockId >= 0))
            {
                _container.MoveBlock(_activeBlockId, xPosViewBox, yPosViewBox);
                imageNeedsUpdate = true;
            }

            if (message.rightButtonIsHold)
            {
                UpdatePanViewBox(message.xPos, message.yPos);
                imageNeedsUpdate = true;
            }

            if (imageNeedsUpdate)
            {
                UpdateImage();
            }
            Notification newNotification = new Notification(new StatusChanged(null, xPosViewBox, yPosViewBox));
            NotificationCenter.Instance.PostNotification(Notification.Id.StatusChanged, newNotification);

            _lastXpos = message.xPos;
            _lastYpos = message.yPos;
        }

        private string CreateImage()
        {
            MemoryStream _stream = new MemoryStream();
            _serializer.Serialize(_stream, _container.container);
            return Encoding.UTF8.GetString(_stream.ToArray());
        }

        private void OnButtonPressedNotification(Notification notification)
        {
            ButtonPressed message = (ButtonPressed)notification.Message;
            _activeButton = message.buttonId;
        }

        private void OnDisplayedSizeChangedNotification(Notification notification)
        {
            DisplayedSizeChanged message = (DisplayedSizeChanged)notification.Message;
            _container.SetImageSize(message.width, message.height);
            _width = message.width;
            _height = message.height;

            if (!_viewboxIsInitialized) {
                _widthZoom = message.width;
                _heightZoom = message.height;
                _viewboxIsInitialized = true;
            }

            UpdateZoomViewBox(0, 0, 1.0);
            UpdateImage();
        }

        private void OnZoomChangedNotification(Notification notification)
        {
            ZoomChanged message = (ZoomChanged)notification.Message;
            if (((_zoomFactor > 0.2) && (message.zoom < 1.0)) || ((_zoomFactor < 5.0) && (message.zoom > 1.0)))
            {
                _zoomFactor *= message.zoom;
                UpdateZoomViewBox(_lastXpos, _lastYpos, message.zoom);
                UpdateImage();
            }

            Notification newNotification = new Notification(new StatusChanged(Convert.ToInt32(_zoomFactor*100), null, null));
            NotificationCenter.Instance.PostNotification(Notification.Id.StatusChanged, newNotification);
        }

        private void UpdateImage()
        {
            Notification newNotification = new Notification(new ImageChanged(CreateImage()));
            NotificationCenter.Instance.PostNotification(Notification.Id.ImageChanged, newNotification);
        }

        private ButtonPressed.Id _activeButton = ButtonPressed.Id.None;
        private int _activeBlockId = -1;
        private int _lastXpos = 0;
        private int _lastYpos = 0;
        private ImageContainer _container = new ImageContainer();
        private XmlSerializer _serializer = new XmlSerializer(typeof(DrawElements.Svg));
        private int _width = 500;
        private int _height = 500;
        private double _xStart = 0;
        private double _widthZoom = 500;
        private double _yStart = 0;
        private double _heightZoom = 500;
        private double _zoomFactor = 1.0;
        private bool _viewboxIsInitialized = false;

    }
}
