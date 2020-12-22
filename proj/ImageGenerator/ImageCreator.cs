using System.IO;
using System.Text;
using System.Xml.Serialization;
using Blocki.DrawElements;
using Blocki.Notifications;

namespace Blocki.ImageGenerator
{
    public class ImageCreator
    {
        public ImageCreator()
        {
            NotificationCenter.Instance.AddObserver(OnCursorChangedNotification, Notification.Id.CursorChanged);
            _drawContainer.SetImageSize(924, 748);
            Block test = new Block();
            test.AddRect(0, 0, 100, 100);
            _firstBlockId = _drawContainer.AddBlock(test);
        }

        private void OnCursorChangedNotification(Notification notification)
        {
            CursorChanged message = (CursorChanged)notification.Message;
            Block test = _drawContainer.GetBlock(_firstBlockId);
            test.ModifyRect(0, message.xPos, message.yPos, 100, 100);

            Notification newNotification = new Notification(new ImageChanged(CreateImage(_drawContainer)));
            NotificationCenter.Instance.PostNotification(Notification.Id.ImageChanged, newNotification);
        }

        private string CreateImage(DrawElements.Svg image)
        {
            MemoryStream _stream = new MemoryStream();
            _serializer.Serialize(_stream, image);
            return Encoding.UTF8.GetString(_stream.ToArray());
        }

        private DrawElements.Svg _drawContainer = new DrawElements.Svg();
        private XmlSerializer _serializer = new XmlSerializer(typeof(DrawElements.Svg));
        private int _firstBlockId;
    }
}
