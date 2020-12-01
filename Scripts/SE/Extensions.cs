using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Extensions
{
    public static class Extensions
    {
        private static readonly System.Random rng = new System.Random();
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1) {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static void AddField(this WWWForm form, string fieldName, bool value)
            => form.AddField(fieldName, (value) ? 1 : 0);
    }
}