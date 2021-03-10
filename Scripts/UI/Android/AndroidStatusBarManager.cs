using UnityEngine;

namespace ClinicalTools.UI
{
    public class AndroidStatusBarManager : MonoBehaviour
    {
        protected static int OriginalScreenHeight { get; set; }
        public static int NavigationBarHeight { get; set; }

#if UNITY_ANDROID && !UNITY_EDITOR
        protected virtual void Start()
        {
            if (OriginalScreenHeight != 0)
                return;

            if (Screen.fullScreen)
                Debug.LogWarning("Game must not be started in fullscreen mode to properly calculate " +
                    "the navigation bar height.");

            OriginalScreenHeight = Screen.height;
        }

        void Update()
        {
            Screen.fullScreen = false;
            ApplicationChrome.dimmed = false;
            ApplicationChrome.statusBarState = ApplicationChrome.States.TranslucentOverContent;
            ApplicationChrome.navigationBarState = ApplicationChrome.States.TranslucentOverContent;
            NavigationBarHeight = Screen.height - OriginalScreenHeight;
        }
#endif
    }
}