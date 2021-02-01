using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseTableOfContentsSection : MonoBehaviour
    {
        public abstract void Display(UserSection section);

        public abstract void SetToggleGroup(ToggleGroup toggleGroup);

        public class Factory : PlaceholderFactory<BaseTableOfContentsSection> { }
    }
}