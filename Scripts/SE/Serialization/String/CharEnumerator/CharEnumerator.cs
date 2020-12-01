using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class CharEnumerator
    {
        public bool IsDone { get; protected set; }
        public char Current => Enumerator.Current;

        protected IEnumerator<char> Enumerator { get; }
        public CharEnumerator(IEnumerator<char> enumerator) => Enumerator = enumerator;
        public CharEnumerator(IEnumerable<char> enumerable) => Enumerator = enumerable?.GetEnumerator();

        public bool MoveNext()
        {
            IsDone = !Enumerator.MoveNext();
            return !IsDone;
        }
    }
}