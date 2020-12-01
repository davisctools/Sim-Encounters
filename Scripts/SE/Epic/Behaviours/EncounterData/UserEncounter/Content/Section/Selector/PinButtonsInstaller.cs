using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class PinButtonsInstaller : MonoInstaller
    {
        public virtual RectTransform PoolParent { get => poolParent; set => poolParent = value; }
        [SerializeField] private RectTransform poolParent;

        public virtual BaseUserDialoguePinDrawer DialoguePinButtonPrefab { get => dialoguePinButtonPrefab; set => dialoguePinButtonPrefab = value; }
        [SerializeField] private BaseUserDialoguePinDrawer dialoguePinButtonPrefab;
        public virtual BaseUserQuizPinDrawer QuizPinButtonPrefab { get => quizPinButtonPrefab; set => quizPinButtonPrefab = value; }
        [SerializeField] private BaseUserQuizPinDrawer quizPinButtonPrefab;

        public override void InstallBindings()
        {
            Container.BindMemoryPool<BaseUserDialoguePinDrawer, BaseUserDialoguePinDrawer.Pool>()
                     .FromComponentInNewPrefab(DialoguePinButtonPrefab)
                     .UnderTransform(PoolParent);
            Container.BindMemoryPool<BaseUserQuizPinDrawer, BaseUserQuizPinDrawer.Pool>()
                     .FromComponentInNewPrefab(QuizPinButtonPrefab)
                     .UnderTransform(PoolParent);
        }
    }
}