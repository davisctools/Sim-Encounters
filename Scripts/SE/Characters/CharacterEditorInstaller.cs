using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class CharacterEditorInstaller : MonoInstaller
    {
        public virtual CharacterEditor CharacterEditorPrefab { get => characterEditorPrefab; set => characterEditorPrefab = value; }
        [SerializeField] private CharacterEditor characterEditorPrefab;

        public override void InstallBindings()
            => Container.BindFactory<CharacterEditor, CharacterEditor.Factory>()
                    .FromComponentInNewPrefab(CharacterEditorPrefab);
    }
}