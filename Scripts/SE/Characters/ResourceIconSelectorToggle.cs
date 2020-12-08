using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ResourceIconSelectorToggle : IconSelectorToggle
    {
        [SerializeField] private string reference;

        protected override void Start() => Icon = new Icon(Icon.IconType.Resource, reference);
    }
}