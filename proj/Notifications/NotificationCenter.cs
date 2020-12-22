using System.Collections.Generic;

namespace Blocki.Notifications
{
    public class NotificationCenter
    {
        public static NotificationCenter Instance
        {
            get { return _instance; }
        }

        public void AddObserver(NotificationDelegate p_notificationDelegate, Notification.Id notificationId)
        {
            List<NotificationDelegate> delegatesCollection;
            if (!_notificationCollection.TryGetValue(notificationId, out delegatesCollection))
            {
                delegatesCollection = new List<NotificationDelegate>();
                _notificationCollection.Add(notificationId, delegatesCollection);
            }
           delegatesCollection.Add(p_notificationDelegate);
        }

        public void PostNotification(Notification.Id notificationId, Notification p_notification)
        {
            List<NotificationDelegate> delegatesCollection;
            if (_notificationCollection.TryGetValue(notificationId, out delegatesCollection))
            {
                foreach (NotificationDelegate notificationDelegate in delegatesCollection)
                {
                    notificationDelegate(p_notification);
                }
            }
        }

        public delegate void NotificationDelegate(Notification p_notification);
        private static NotificationCenter _instance = new NotificationCenter();
        private readonly Dictionary<Notification.Id, List<NotificationDelegate>> _notificationCollection = new Dictionary<Notification.Id, List<NotificationDelegate>>();
    }
}
