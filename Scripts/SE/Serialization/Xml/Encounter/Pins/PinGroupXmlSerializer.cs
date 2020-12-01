namespace ClinicalTools.SimEncounters
{
    public class PinGroupXmlSerializer : IXmlSerializer<PinGroup>
    {
        protected virtual IXmlSerializer<DialoguePin> DialoguePinFactory { get; }
        protected virtual IXmlSerializer<QuizPin> QuizPinFactory { get; }

        protected virtual XmlNodeInfo DialogueInfo { get; } = new XmlNodeInfo("dialogue");
        protected virtual XmlNodeInfo QuizInfo { get; } = new XmlNodeInfo("quiz");

        public PinGroupXmlSerializer(IXmlSerializer<DialoguePin> dialoguePinFactory, IXmlSerializer<QuizPin> quizPinFactory)
        {
            DialoguePinFactory = dialoguePinFactory;
            QuizPinFactory = quizPinFactory;
        }

        public virtual bool ShouldSerialize(PinGroup value) 
            => value != null && (value.Dialogue != null || value.Quiz != null);

        public virtual void Serialize(XmlSerializer serializer, PinGroup value)
        {
            serializer.AddValue(DialogueInfo, value.Dialogue, DialoguePinFactory);
            serializer.AddValue(QuizInfo, value.Quiz, QuizPinFactory);
        }

        public virtual PinGroup Deserialize(XmlDeserializer deserializer)
        {
            var pinData = CreatePinData(deserializer);

            AddDialogue(deserializer, pinData);
            AddQuiz(deserializer, pinData);

            return pinData;
        }

        protected virtual PinGroup CreatePinData(XmlDeserializer deserializer) => new PinGroup();

        protected virtual DialoguePin GetDialogue(XmlDeserializer deserializer)
            => deserializer.GetValue(DialogueInfo, DialoguePinFactory);
        protected virtual void AddDialogue(XmlDeserializer deserializer, PinGroup quizPin)
            => quizPin.Dialogue = GetDialogue(deserializer);

        protected virtual QuizPin GetQuiz(XmlDeserializer deserializer)
            => deserializer.GetValue(QuizInfo, QuizPinFactory);
        protected virtual void AddQuiz(XmlDeserializer deserializer, PinGroup quizPin)
            => quizPin.Quiz = GetQuiz(deserializer);
    }
}
