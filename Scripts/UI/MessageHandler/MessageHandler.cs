using UnityEngine;

namespace ClinicalTools.UI
{
    public class MessageHandler : BaseMessageHandler
    {
        public BaseBasicMessageHandler NotificationHandler { get => notificationHandler; set => notificationHandler = value; }
        [SerializeField] private BaseBasicMessageHandler notificationHandler;
        public BaseBasicMessageHandler ErrorHandler { get => errorHandler; set => errorHandler = value; }
        [SerializeField] private BaseBasicMessageHandler errorHandler;


        public override void ShowMessage(string message, MessageType type = MessageType.Notification)
        {
            gameObject.SetActive(true);

            BaseBasicMessageHandler messageHandler;
            if (type == MessageType.Notification)
                messageHandler = NotificationHandler;
            else
                messageHandler = ErrorHandler;
            messageHandler.ShowMessage(message);
        }
    }
}