using ClinicalTools.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Button))]
    public class WriterReturnToMainMenuButton : MonoBehaviour
    {
        protected ISelectedListener<LoadingWriterSceneInfoSelectedEventArgs> WriterSceneInfoSelectedListener { get; set; }
        protected IMenuSceneStarter MenuSceneStarter { get; set; }
        protected IMenuEncountersInfoReader MenuInfoReader { get; set; }
        protected BaseConfirmationPopup ConfirmationPopup { get; set; }
        [Inject]
        public virtual void Inject(
            ISelectedListener<LoadingWriterSceneInfoSelectedEventArgs> writerSceneInfoSelectedListener,
            IMenuSceneStarter menuSceneStarter, 
            IMenuEncountersInfoReader menuInfoReader, 
            BaseConfirmationPopup confirmationPopup)
        {
            WriterSceneInfoSelectedListener = writerSceneInfoSelectedListener;
            MenuSceneStarter = menuSceneStarter;
            MenuInfoReader = menuInfoReader;
            ConfirmationPopup = confirmationPopup;
        }

        protected virtual void Awake()
        {
            GetComponent<Button>().onClick.AddListener(ConfirmReturnToMainMenu);
        }

        protected virtual void ConfirmReturnToMainMenu()
            => ConfirmationPopup.ShowConfirmation(ReturnToMainMenu, "CONFIRMATION",
                "Are you sure you want to exit?\nAny unsaved changes will be lost");

        protected virtual void ReturnToMainMenu()
        {
            var sceneInfo = WriterSceneInfoSelectedListener.CurrentValue.SceneInfo;
            var categories = MenuInfoReader.GetMenuEncountersInfo(sceneInfo.User);
            var menuSceneInfo = new LoadingMenuSceneInfo(sceneInfo.User, sceneInfo.LoadingScreen, categories);
            MenuSceneStarter.StartScene(menuSceneInfo);
        }
    }
}