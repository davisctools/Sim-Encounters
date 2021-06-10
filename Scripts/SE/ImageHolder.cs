using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class ImageHolder : MonoBehaviour
    {
        public static ImageHolder Instance { get; set; }
        protected List<Sprite> Sprites { get; set; } = new List<Sprite>();


        // Start is called before the first frame update
        void Awake()
        {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //SceneManager.sceneLoaded += OnSceneLoaded;
        }

        protected int Index { get; set; }
        // Update is called once per frame
        public static void HoldImage(Sprite sprite)
        {
            if (AcceptsData && Instance != null)
                Instance.Sprites.Add(sprite);
            //Instance.Images[Instance.Index++].sprite = sprite;
            //Instance.Sprites[Instance.Index++] = sprite;
            //if (Instance.Index >= Instance.Sprites.Length)
            //Instance.Index = 0;
        }

        protected static bool AcceptsData { get; set; }
        public static void BeginHoldingData()
        {
            AcceptsData = true;
        }
        public static void StopHoldingData()
        {
            AcceptsData = false;
            Instance.Sprites.Clear();
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            ClearInFrames();
        }

        public IEnumerable ClearInFrames()
        {
            yield return null;
            yield return null;
            yield return null;
        }
    }
}