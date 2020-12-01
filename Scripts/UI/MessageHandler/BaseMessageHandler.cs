using UnityEngine;

namespace ClinicalTools.UI
{
    public enum MessageType { Notification, Error }
    public abstract class BaseMessageHandler : MonoBehaviour
    {
        public abstract void ShowMessage(string message, MessageType type = MessageType.Notification);
    }
}