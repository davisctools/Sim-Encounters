using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.UI
{
    public class AndroidStatusBarManager : MonoBehaviour
    {
        void Update()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            Screen.fullScreen = true;
            ApplicationChrome.dimmed = false;
            ApplicationChrome.statusBarState = ApplicationChrome.States.TranslucentOverContent;
            ApplicationChrome.navigationBarState = ApplicationChrome.States.Hidden;
#endif
        }
    }
}