using ClinicalTools.Collections;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseCharactersEditor : MonoBehaviour
    {
        public abstract void EditCharacters(OrderedCollection<Character> characters);
    }
}