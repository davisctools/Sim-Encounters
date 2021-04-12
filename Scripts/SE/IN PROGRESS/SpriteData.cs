using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class SpriteData
    {
        public Sprite Sprite { get; }
        public byte[] Bytes { get; }
        public string Path { get; }

        public SpriteData(Sprite sprite, string path, byte[] bytes)
        {
            Sprite = sprite;
            Path = path;
            Bytes = bytes;
        }
    }
}