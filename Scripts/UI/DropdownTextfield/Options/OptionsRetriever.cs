using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.UI
{
    public class OptionsRetriever : BaseOptionsRetriever
    {
        public string[] Options { get => options; set => options = value; }
        [SerializeField] private string[] options;
        public override IEnumerable<string> GetOptions() => Options;
    }
}