using System;
using System.Collections.Generic;

namespace ClinicalTools.Collections
{
    public class KeyGenerator : IKeyGenerator
    {
        protected int KeyByteLength { get; } = 3;
        protected int KeyCharLength => 2;

        protected virtual HashSet<string> Keys { get; } = new HashSet<string>();
        protected virtual Random KeyRandomizer { get; set; }

        public KeyGenerator(int seed) => SetSeed(seed);

        public virtual void SetSeed(int seed) => KeyRandomizer = new Random(seed);


        public virtual bool Contains(string key) => Keys.Contains(key);
        public virtual void AddKey(string key)
        {
            if (!Keys.Contains(key))
                Keys.Add(key);
        }

        public virtual string Generate() => Generate(KeyRandomizer);

        public virtual string Generate(string seed)
        {
            var keyRandomizer = new Random(seed.GetHashCode());
            return Generate(keyRandomizer);
        }

        protected virtual string Generate(Random keyRandomizer)
        {
            var bytes = new byte[KeyByteLength];
            keyRandomizer.NextBytes(bytes);
            var key = Convert.ToBase64String(bytes);
            key = key.Substring(0, KeyCharLength);

            if (Keys.Contains(key))
                return Generate(keyRandomizer);

            Keys.Add(key);
            return key;
        }

        public bool IsValidKey(string key) => key.Length == KeyCharLength;
    }
}