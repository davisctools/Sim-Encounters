using System;

namespace ClinicalTools.SimEncounters
{
    public class DoubleTildeStringSplitter : IStringSplitter
    {
        private const string divider = "~~";
        public string[] Split(string str) => str.Split(new string[] { divider }, StringSplitOptions.None);
    }
}