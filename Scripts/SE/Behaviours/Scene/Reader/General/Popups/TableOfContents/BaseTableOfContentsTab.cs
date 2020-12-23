using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseTableOfContentsTab : MonoBehaviour
    {
        public abstract void Display(UserSection section, UserTab tab);

        public class Factory : PlaceholderFactory<BaseTableOfContentsTab> { }
    }
}