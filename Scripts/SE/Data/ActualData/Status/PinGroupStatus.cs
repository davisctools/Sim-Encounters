namespace ClinicalTools.SimEncounters
{
    public class PinGroupStatus
    {
        public bool Read { get; set; }

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