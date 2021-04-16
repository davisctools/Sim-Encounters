namespace ClinicalTools.SimEncounters
{
    public class PinGroupXmlSerializer : IObjectSerializer<PinGroup>
    {
        protected virtual IObjectSerializer<DialoguePin> DialoguePinFactory { get; }
        protected virtual IObjectSerializer<QuizPin> QuizPinFactory { get; }

        protected virtual XmlNodeInfo DialogueInfo { get; } = new XmlNodeInfo("dialogue");
        protected virtual XmlNodeInfo QuizInfo { get; } = new XmlNodeInfo("quiz");

        public PinGroupXmlSerializer(IObjectSerializer<DialoguePin> dialoguePinFactory, IObjectSerializer<QuizPin> quizPinFactory)
        {
            DialoguePinFactory = dialoguePinFactory;
            QuizPinFactory = quizPinFactory;
        }

        public virtual bool ShouldSerialize(PinGroup value) 
            => value != null && (value.Dialogue != null || value.Quiz != null);

        public virtual void Serialize(IDataSerializer serializer, PinGroup value)
        {
            serializer.AddValue(DialogueInfo, value.Dialogue, DialoguePinFactory);
            serializer.AddValue(QuizInfo, value.Quiz, QuizPinFactory);
        }

        public virtual PinGroup Deserialize(IDataDeserializer deserializer)
        {
            var pinData = CreatePinData(deserializer);

            AddDialogue(deserializer, pinData);
            AddQuiz(deserializer, pinData);

            return pinData;
        }

        protected virtual PinGroup CreatePinData(IDataDeserializer deserializer) => new PinGroup();

        protected virtual DialoguePin GetDialogue(IDataDeserializer deserializer)
            => deserializer.GetValue(DialogueInfo, DialoguePinFactory);
        protected virtual void AddDialogue(IDataDeserializer deserializer, PinGroup quizPin)
            => quizPin.Dialogue = GetDialogue(deserializer);

        protected virtual QuizPin GetQuiz(IDataDeserializer deserializer)
            => deserializer.GetValue(QuizInfo, QuizPinFactory);
        protected virtual void AddQuiz(IDataDeserializer deserializer, PinGroup quizPin)
            => quizPin.Quiz = GetQuiz(deserializer);
    }
}
