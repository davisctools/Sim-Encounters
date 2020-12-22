namespace ClinicalTools.SimEncounters
{
    public class PinGroupStatus
    {
        public bool Read { get; set; }

        private ReadMoreStatus readMoreStatus;
        public ReadMoreStatus ReadMoreStatus {
            get {
                if (readMoreStatus == null)
                    readMoreStatus = new ReadMoreStatus { Read = Read };
                return readMoreStatus;
            }
            set => readMoreStatus = value;
        }
        private QuizStatus quizStatus;
        public QuizStatus QuizStatus {
            get {
                if (quizStatus == null)
                    quizStatus = new QuizStatus { Read = Read };
                return quizStatus;
            }
            set => quizStatus = value;
        }

        private DialogueStatus dialogueStatus;
        public DialogueStatus DialogueStatus {
            get {
                if (dialogueStatus == null)
                    dialogueStatus = new DialogueStatus { Read = Read };
                return dialogueStatus;
            }
            set => dialogueStatus = value;
        }
    }
}