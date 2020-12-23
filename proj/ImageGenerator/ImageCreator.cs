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
        }

        private void OnCursorChangedNotification(Notification notification)
        {
            bool updateImage = false;

            CursorChanged message = (CursorChanged)notification.Message;
            if ((message.buttonIsPressed) && (_activeButton == ButtonPressed.Id.AddBlock))
            {
                _container.AddBlock(message.xPos, message.yPos);
                updateImage = true;
            }
            if ((message.buttonIsPressed) && (_activeButton == ButtonPressed.Id.Delete) && (_activeBlockId >= 0))
            {
                _container.DeleteBlock(_activeBlockId);
                updateImage = true;
            }
            if (!message.buttonIsHold)
            {
                _activeBlockId = _container.SelectBlock(message.xPos, message.yPos);
            }
            else if ((_activeButton == ButtonPressed.Id.Move) && (_activeBlockId >= 0))
            {
                _container.MoveBlock(_activeBlockId, message.xPos, message.yPos);
                updateImage = true;
            }

            if (updateImage)
            {
                Notification newNotification = new Notification(new ImageChanged(CreateImage()));
                NotificationCenter.Instance.PostNotification(Notification.Id.ImageChanged, newNotification);
            }
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

        private ButtonPressed.Id _activeButton = ButtonPressed.Id.None;
        private int _activeBlockId;
        private ImageContainer _container = new ImageContainer();
        private XmlSerializer _serializer = new XmlSerializer(typeof(DrawElements.Svg));
    }
}
