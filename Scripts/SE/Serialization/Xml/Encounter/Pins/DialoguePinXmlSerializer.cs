using System.Collections.Generic;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class DialoguePinXmlSerializer : IXmlSerializer<DialoguePin>
    {
        // pins are created by panels, so a lazy injection needs to be used to prevent an infinite loop
        protected virtual LazyInject<IXmlSerializer<Panel>> PanelFactory { get; }
        public DialoguePinXmlSerializer(LazyInject<IXmlSerializer<Panel>> panelFactory)
        {
            PanelFactory = panelFactory;
        }

        protected virtual XmlCollectionInfo ConversationInfo { get; } = new XmlCollectionInfo("conversation", "panel");

        public virtual bool ShouldSerialize(DialoguePin value) => value != null;

        public void Serialize(XmlSerializer serializer, DialoguePin value)
        {
            serializer.AddKeyValuePairs(ConversationInfo, value.Conversation, PanelFactory.Value);
        }

        public DialoguePin Deserialize(XmlDeserializer deserializer)
        {
            var dialoguePin = CreateDialoguePin(deserializer);

            AddConversation(deserializer, dialoguePin);

            return dialoguePin;
        }

        protected virtual DialoguePin CreateDialoguePin(XmlDeserializer deserializer) => new DialoguePin();

        protected virtual List<KeyValuePair<string, Panel>> GetConversation(XmlDeserializer deserializer)
            => deserializer.GetKeyValuePairs(ConversationInfo, PanelFactory.Value);
        protected virtual void AddConversation(XmlDeserializer deserializer, DialoguePin dialoguePin)
        {
            var conversationPairs = GetConversation(deserializer);
            if (conversationPairs == null)
                return;

            foreach (var panelPair in conversationPairs)
                dialoguePin.Conversation.Add(panelPair);
        }
    }
}