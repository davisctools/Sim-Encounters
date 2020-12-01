using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ClinicalTools.Collections
{
    /// <summary>
    /// A bidirectional dictionary with automatically generated string keys.
    /// </summary>
    /// <typeparam name="T">Type of values stored in the collection</typeparam>
    public class KeyedCollection<T> : IEnumerable<KeyValuePair<string, T>>
    {
        protected IKeyGenerator KeyGenerator { get; }

        /// <summary>
        /// Dictionary of values in the collection.
        /// This dictionary is bidirectional through the use of the Keys dictionary, 
        /// so all values must be unique.
        /// </summary>
        protected virtual IDictionary<string, T> Collection { get; } = new Dictionary<string, T>();

        /// <summary>
        /// Dictionary of keys in the collection.
        /// </summary>
        protected virtual IDictionary<T, string> KeyCollection { get; } = new Dictionary<T, string>();

        public virtual T this[string key] {
            get => Get(key);
            set => Set(key, value);
        }
        public virtual string this[T value] => GetKey(value);

        public virtual IEnumerable<string> Keys => Collection.Keys.AsEnumerable();
        public virtual IEnumerable<T> Values => Collection.Values.AsEnumerable();
        public int Count => Collection.Count;

        public KeyedCollection()
        {
            KeyGenerator = new KeyGenerator((int)DateTime.Now.ToFileTimeUtc());
        }
        public KeyedCollection(IKeyGenerator keyGenerator) {
            KeyGenerator = keyGenerator;
        }

        /// <summary>
        /// Creates a unique key for the collection item.
        /// </summary>
        /// <returns>The unique key</returns>
        protected virtual string GenerateKey() => KeyGenerator.Generate();

        /// <summary>
        /// Creates a unique key for the collection item.
        /// </summary>
        /// <param name="seed">Seed used to generate the key</param>
        /// <returns>The unique key</returns>
        protected virtual string GenerateKey(string oldKey)
        {
            if (string.IsNullOrEmpty(oldKey))
                return GenerateKey();
            if (KeyGenerator.Contains(oldKey))
                return KeyGenerator.Generate(oldKey);

            KeyGenerator.AddKey(oldKey);
            return oldKey;
        }

        /// <summary>
        /// Adds a value to the collection.
        /// The value must be unique.
        /// </summary>
        /// <param name="value">Value to add</param>
        /// <returns>The key of the added value, or null if unable to add the value</returns>
        public virtual string Add(T value)
        {
            var key = GenerateKey();
            if (AddKeyedValue(key, value))
                return key;

            return null;
        }

        public virtual string Add(KeyValuePair<string, T> pair)
        {
            var key = GenerateKey(pair.Key);
            if (AddKeyedValue(key, pair.Value))
                return key;

            return null;
        }

        public virtual string Add(string key, T value)
        {
            key = GenerateKey(key);
            if (AddKeyedValue(key, value))
                return key;

            return null;
        }

        public virtual bool AddKeyedValue(string key, T value)
        {
            if (KeyCollection.ContainsKey(value)) {
                Debug.LogError($"Value already exists in collection:\n" +
                    $"{value.ToString()}");
                return false;
            }

            Collection.Add(key, value);
            KeyCollection.Add(value, key);

            return true;
        }

        public virtual string GetKey(T value) => KeyCollection[value];

        /// <summary>
        /// Gets a value from the collection by its key.
        /// </summary>
        /// <param name="key">Key of the value to get</param>
        /// <returns>
        /// The value for the passed key, or null if the key isn't in the collection
        /// </returns>
        protected virtual T Get(string key)
        {
            if (Collection.ContainsKey(key))
                return Collection[key];
            else
                return default;
        }

        protected virtual void Set(string key, T value)
        {
            if (Collection.ContainsKey(key))
                Collection[key] = value;
            else
                Collection.Add(key, value);
        }

        /// <summary>
        /// Removes a value from the collection by its key.
        /// </summary>
        /// <param name="key">Key of the value to remove</param>
        public virtual void Remove(string key)
        {
            var item = Collection[key];
            KeyCollection.Remove(item);
            Collection.Remove(key);
        }
        /// <summary>
        /// Removes a value from the collection.
        /// </summary>
        /// <param name="value">Value to remove</param>
        public virtual void Remove(T value)
        {
            var key = KeyCollection[value];
            KeyCollection.Remove(value);
            Collection.Remove(key);
        }

        public virtual bool ContainsKey(string key) => Collection.ContainsKey(key);
        public virtual bool Contains(T value) => KeyCollection.ContainsKey(value);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public virtual IEnumerator<KeyValuePair<string, T>> GetEnumerator() => Collection.GetEnumerator();
    }
}