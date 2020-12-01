using UnityEngine;
using Zenject;

namespace ClinicalTools.UI
{
    public class DropdownOptionInstaller : MonoInstaller
    {
        public virtual BaseDropdownOption DropdownOptionPrefab { get => dropdownOptionPrefab; set => dropdownOptionPrefab = value; }
        [SerializeField] private BaseDropdownOption dropdownOptionPrefab;

        public override void InstallBindings()
            => Container.BindFactory<BaseDropdownOption, BaseDropdownOption.Factory>()
                    .FromComponentInNewPrefab(DropdownOptionPrefab);
    }
}