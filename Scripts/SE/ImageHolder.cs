using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ImageHolder : MonoBehaviour
    {
        public static ImageHolder Instance { get; set; }
        protected List<Sprite> Sprites { get; } = new List<Sprite>();

        void Awake()
        {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        protected int Index { get; set; }
        public static void HoldImage(Sprite sprite)
        {
            if (AcceptsData && Instance != null)
                Instance.Sprites.Add(sprite);
        }

        protected static bool AcceptsData { get; set; }
        public static void BeginHoldingData() => AcceptsData = true;
        public static void StopHoldingData()
        {
            AcceptsData = false;
            if (Instance != null)
                Instance.StartCoroutine(Instance.ClearInFrames());
        }

        protected virtual int ClearFrames { get; } = 5;
        public IEnumerator ClearInFrames()
        {
            for (int i = 0; i < ClearFrames; i++)
                yield return null;

            Instance.Sprites.Clear();
        }
    }
}