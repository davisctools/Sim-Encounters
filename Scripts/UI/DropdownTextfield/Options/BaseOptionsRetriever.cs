using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.UI
{
    public abstract class BaseOptionsRetriever : MonoBehaviour
    {
        public abstract IEnumerable<string> GetOptions();
    }
}