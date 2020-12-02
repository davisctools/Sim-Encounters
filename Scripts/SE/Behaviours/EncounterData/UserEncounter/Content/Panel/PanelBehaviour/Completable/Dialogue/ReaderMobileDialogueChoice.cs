using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMobileDialogueChoice : CompletableReaderPanelBehaviour
    {
        protected override BaseChildUserPanelsDrawer ChildPanelsDrawer { get => childPanelsDrawer; }
        [SerializeField] private BaseChildUserPanelsDrawer childPanelsDrawer = null;
        protected override BaseUserPinGroupDrawer PinsDrawer => null;

        public virtual GameObject CompletedObject { get => completedObject; set => completedObject = value; }
        [SerializeField] private GameObject completedObject;
        public virtual GameObject IncompletedObject { get => incompletedObject; set => incompletedObject = value; }
        [SerializeField] private GameObject incompletedObject;
        protected override bool SetReadOnSelect => true;

        public override void Select(object sender, UserPanelSelectedEventArgs eventArgs)
        {
            base.Select(sender, eventArgs);

            CompletedObject.gameObject.SetActive(false);
            IncompletedObject.gameObject.SetActive(true);
        }

        public override void SetCompleted()
        {
            base.SetCompleted();
            CompletedObject.gameObject.SetActive(true);
            IncompletedObject.gameObject.SetActive(false);
        }
    }
}