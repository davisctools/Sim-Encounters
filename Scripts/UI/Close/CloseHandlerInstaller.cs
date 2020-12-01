using System;
using UnityEngine;
using Zenject;

namespace ClinicalTools.UI
{
    public class CloseHandlerInstaller : MonoInstaller
    {
        [SerializeField] private MonoBehaviour closeBehaviour;
        public MonoBehaviour CloseBehaviour { get => closeBehaviour; set => closeBehaviour = value; }
        public override void InstallBindings()
        {
            if (CloseBehaviour is ICloseHandler closeHandler)
                Container.BindInstance(closeHandler);
            else
                throw new Exception("Close behaviour does not implement ICloseHandler");
        }
    }
}