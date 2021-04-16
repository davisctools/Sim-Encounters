using System.Collections.Generic;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class DialoguePinXmlSerializer : IObjectSerializer<DialoguePin>
    {
        // pins are created by panels, so a lazy injection needs to be used to prevent an infinite loop
        protected virtual LazyInject<IObjectSerializer<Panel>> PanelFactory { get; }
        public DialoguePinXmlSerializer(LazyInject<IObjectSerializer<Panel>> panelFactory)
        {
            PanelFactory = panelFactory;
        }

        protected virtual XmlCollectionInfo ConversationInfo { get; } = new XmlCollectionInfo("conversation", "panel");

        public virtual bool ShouldSerialize(DialoguePin value) => value != null;

        public void Serialize(IDataSerializer serializer, DialoguePin value)
        {
            serializer.AddKeyValuePairs(ConversationInfo, value.Conversation, PanelFactory.Value);
        }

        public DialoguePin Deserialize(IDataDeserializer deserializer)
        {
            var dialoguePin = CreateDialoguePin(deserializer);

            AddConversation(deserializer, dialoguePin);

            return dialoguePin;
        }

        protected virtual DialoguePin CreateDialoguePin(IDataDeserializer deserializer) => new DialoguePin();

        protected virtual List<KeyValuePair<string, Panel>> GetConversation(IDataDeserializer deserializer)
            => deserializer.GetKeyValuePairs(ConversationInfo, PanelFactory.Value);
        protected virtual void AddConversation(IDataDeserializer deserializer, DialoguePin dialoguePin)
        {
            var conversationPairs = GetConversation(deserializer);
            if (conversationPairs == null)
                return;

            foreach (var panelPair in conversationPairs)
                dialoguePin.Conversation.Add(panelPair);
        }
    }
}