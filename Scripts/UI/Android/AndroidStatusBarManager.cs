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
            ApplicationChrome.dimmed = false;
            ApplicationChrome.statusBarState = ApplicationChrome.States.TranslucentOverContent;
        }
    }
}