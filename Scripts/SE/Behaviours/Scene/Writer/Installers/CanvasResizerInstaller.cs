using ClinicalTools.UI;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class CanvasResizerInstaller : MonoInstaller
    {
        [SerializeField] private CanvasResizer canvasResizer;

        public override void InstallBindings() => Container.BindInstance(canvasResizer);
    }
}