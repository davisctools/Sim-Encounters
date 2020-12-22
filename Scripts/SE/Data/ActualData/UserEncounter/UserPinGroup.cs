using System;

namespace ClinicalTools.SimEncounters
{
    public class UserPinGroup
    {
        public UserEncounter Encounter { get; }
        public PinGroup Data { get; }

        public UserReadMorePin ReadMorePin { get; }
        public UserDialoguePin DialoguePin { get; }
        public UserQuizPin QuizPin { get; }
        public PinGroupStatus Status { get; }

        public event Action StatusChanged;

        public UserPinGroup(UserEncounter encounter, PinGroup pinGroup, PinGroupStatus status)
        {
            Encounter = encounter;
            Data = pinGroup;
            Status = status;

            if (pinGroup.ReadMore != null) {
                ReadMorePin = new UserReadMorePin(Encounter, Data.ReadMore, Status.ReadMoreStatus);
                ReadMorePin.StatusChanged += UpdateIsRead;
            }
            if (pinGroup.Dialogue != null) {
                DialoguePin = new UserDialoguePin(Encounter, Data.Dialogue, Status.DialogueStatus);
                DialoguePin.StatusChanged += UpdateIsRead;
            }
            if (pinGroup.Quiz != null) {
                QuizPin = new UserQuizPin(Encounter, Data.Quiz, Status.QuizStatus);
                QuizPin.StatusChanged += UpdateIsRead;
            }
        }

        protected virtual void UpdateIsRead()
        {
            if (!Status.Read && DialoguePin?.IsRead() != false && QuizPin?.IsRead() != false)
                SetRead(true);
        }

        public bool IsRead() => Status.Read;
        public void SetRead(bool read)
        {
            if (Status.Read == read)
                return;
            Status.Read = read;
            StatusChanged?.Invoke();
        }
    }
}