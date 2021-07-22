using System.Collections;
using System.Collections.Generic;

namespace ClinicalTools.Collections
{
    /// <summary>
    /// An ordered bidirectional dictionary with automatically generated string keys.
    /// </summary>
    /// <typeparam name="T">Type of values stored in the collection</typeparam>
    public class OrderedCollection<T> : KeyedCollection<T>, IEnumerable<KeyValuePair<string, T>>
    {
        // An ordered list of values
        protected virtual List<T> ValueList { get; set; } = new List<T>();
        // An ordered list of the key value pairs
        // This could be made to use less memory, but both parts may be used frequently, so this is simpler
        protected virtual List<KeyValuePair<string, T>> PairList { get; set; } = new List<KeyValuePair<string, T>>();

        /// <summary>
        /// Ordered array of values in the collection.
        /// </summary>
        public virtual T[] ValueArr => ValueList.ToArray();
        public override IEnumerable<T> Values => ValueList;

        public virtual int IndexOf(T value) => ValueList.IndexOf(value);


        public virtual KeyValuePair<string, T> this[int index] => Get(index);

        public OrderedCollection() : base() { }
        public OrderedCollection(IKeyGenerator keyGenerator) : base(keyGenerator) { }

        public virtual bool Insert(int index, string key, T value)
        {
            if (!base.AddKeyedValue(key, value))
                return false;

            PairList.Insert(index, new KeyValuePair<string, T>(key, value));
            ValueList.Insert(index, value);
            return true;
        }

        public override bool AddKeyedValue(string key, T value)
        {
            if (!base.AddKeyedValue(key, value))
                return false;

            PairList.Add(new KeyValuePair<string, T>(key, value));
            ValueList.Add(value);
            return true;
        }

        public override void Remove(string key)
        {
            var index = IndexOf(Collection[key]);
            ValueList.RemoveAt(index);
            PairList.RemoveAt(index);

            base.Remove(key);
        }
        public override void Remove(T value)
        {
            var index = IndexOf(value);
            ValueList.RemoveAt(index);
            PairList.RemoveAt(index);

            base.Remove(value);
        }

        protected override void Set(string key, T value)
        {
            for (int i = 0; i < PairList.Count; i++) {
                if (PairList[i].Key != key)
                    continue;

                PairList[i] = new KeyValuePair<string, T>(key, value);
                break;
            }

            base.Set(key, value);
        }

        protected virtual KeyValuePair<string, T> Get(int val) => PairList[val];

        public virtual void MoveValue(int newIndex, int currentIndex)
        {
            var value = ValueList[currentIndex];
            var pair = PairList[currentIndex];
            ValueList.RemoveAt(currentIndex);
            PairList.RemoveAt(currentIndex);
            ValueList.Insert(newIndex, value);
            PairList.Insert(newIndex, pair);
        }
        IEnumerator IEnumerable.GetEnumerator() => PairList.GetEnumerator();
        public override IEnumerator<KeyValuePair<string, T>> GetEnumerator() => PairList.GetEnumerator();

    }
}