using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class AndroidBackButton : MonoBehaviour
    {
        protected Stack<Action> BackActions = new Stack<Action>();

#if MOBILE
        protected virtual void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && BackActions.Count > 0)
                BackActions.Pop().Invoke();
        }
#endif

        public void Register(Action action) => BackActions.Push(action);

        public void Deregister(Action action)
        {
            if (BackActions.Count > 0 && BackActions.Peek() == action)
                BackActions.Pop();
        }
    }
}
