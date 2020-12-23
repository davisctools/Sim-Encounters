using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseTableOfContentsSection : MonoBehaviour
    {
        public abstract void Display(UserSection section);

        public class Factory : PlaceholderFactory<BaseTableOfContentsSection> { }
    }
}