using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Blocki.Notifications;
using Blocki.Helper;

namespace Blocki.ImageGenerator
{
    public class ImageCreator
    {
        public ImageCreator()
        {
            NotificationCenter.Instance.AddObserver(OnCursorChangedNotification, Definitions.NotificationId.CursorChanged);
            NotificationCenter.Instance.AddObserver(OnButtonPressedNotification, Definitions.NotificationId.ButtonPressed);
            NotificationCenter.Instance.AddObserver(OnDisplayedSizeChangedNotification, Definitions.NotificationId.DisplayedSizeChanged);
            NotificationCenter.Instance.AddObserver(OnZoomChangedNotification, Definitions.NotificationId.ZoomChanged);
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

            _svg.SetViewBox(Convert.ToInt32(_xStart), Convert.ToInt32(_yStart), Convert.ToInt32(_widthZoom), Convert.ToInt32(_heightZoom));
            _grid.Delete(_svg);
            _grid.Add(_svg, xPos, yPos);
        }

        private void UpdatePanViewBox(int xPos, int yPos)
        {
            int xDiff = _lastXpos - xPos;
            int yDiff = _lastYpos - yPos;
            _xStart += Convert.ToDouble(xDiff) * _zoomFactor;
            _yStart += Convert.ToDouble(yDiff) * _zoomFactor;
            _svg.SetViewBox(Convert.ToInt32(_xStart), Convert.ToInt32(_yStart), Convert.ToInt32(_widthZoom), Convert.ToInt32(_heightZoom));
            _grid.Delete(_svg);
            _grid.Add(_svg, xPos, yPos);
        }

        private void OnCursorChangedNotification(Notification notification)
        {
            CursorChanged message = (CursorChanged)notification.Message;
            int xPosViewBox = Convert.ToInt32(_xStart + (message.xPos * _zoomFactor));
            int yPosViewBox = Convert.ToInt32(_yStart + (message.yPos * _zoomFactor));
            bool imageNeedsUpdate = false;

            if ((message.leftButtonIsPressed) && (_activeButton == Definitions.ButtonId.AddBlock))
            {
                _block.Add(_svg, xPosViewBox, yPosViewBox);
                imageNeedsUpdate = true;
            }
            if ((message.leftButtonIsPressed) && (_activeButton == Definitions.ButtonId.Delete))
            {
                _block.Delete(_svg);
                imageNeedsUpdate = true;
            }
            if ((message.leftButtonIsPressed) && (_activeButton == Definitions.ButtonId.Connect))
            {
                _connection.Add(_svg, xPosViewBox, yPosViewBox);
                imageNeedsUpdate = true;
            }

            if (!message.leftButtonIsHold)
            {
                if(_block.Visitor(_svg, xPosViewBox, yPosViewBox))
                {
                    imageNeedsUpdate = true;
                }
            }
            else if (_activeButton == Definitions.ButtonId.Move)
            {
                _block.Move(_svg, xPosViewBox, yPosViewBox);
                imageNeedsUpdate = true;
            }

            if (message.rightButtonIsHold)
            {
                UpdatePanViewBox(message.xPos, message.yPos);
                imageNeedsUpdate = true;
            }

            if (imageNeedsUpdate)
            {
                _connection.Update(_svg);
                UpdateImage();
            }
            Notification newNotification = new Notification(new StatusChanged(null, xPosViewBox, yPosViewBox));
            NotificationCenter.Instance.PostNotification(Definitions.NotificationId.StatusChanged, newNotification);

            _lastXpos = message.xPos;
            _lastYpos = message.yPos;
        }

        private string CreateImage()
        {
            MemoryStream _stream = new MemoryStream();
            _serializer.Serialize(_stream, _svg);
            return Encoding.UTF8.GetString(_stream.ToArray());
        }

        private void OnButtonPressedNotification(Notification notification)
        {
            ButtonPressed message = (ButtonPressed)notification.Message;
            if ((_activeButton == Definitions.ButtonId.Connect) && (message.buttonId != Definitions.ButtonId.Connect))
            {
                _connection.RemoveSelection(_svg);
            }
            _activeButton = message.buttonId;
        }

        private void OnDisplayedSizeChangedNotification(Notification notification)
        {
            DisplayedSizeChanged message = (DisplayedSizeChanged)notification.Message;
            _svg.SetImageSize(message.width, message.height);
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
            NotificationCenter.Instance.PostNotification(Definitions.NotificationId.StatusChanged, newNotification);
        }

        private void UpdateImage()
        {
            Notification newNotification = new Notification(new ImageChanged(CreateImage()));
            NotificationCenter.Instance.PostNotification(Definitions.NotificationId.ImageChanged, newNotification);
        }

        private readonly DrawElements.Svg _svg = new DrawElements.Svg();
        private readonly Grid _grid = new Grid();
        private readonly Block _block = new Block();
        private readonly Connection _connection = new Connection();
        private Definitions.ButtonId _activeButton = Definitions.ButtonId.None;
        private int _lastXpos = 0;
        private int _lastYpos = 0;
        private readonly XmlSerializer _serializer = new XmlSerializer(typeof(DrawElements.Svg));
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
