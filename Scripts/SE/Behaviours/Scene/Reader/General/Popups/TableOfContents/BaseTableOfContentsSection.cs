using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseTableOfContentsSection : BaseSelectableUserSectionBehaviour
    {
        public new class Factory : PlaceholderFactory<BaseTableOfContentsSection> { }
    }
}