using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public interface ISpriteDeserializer
    {
        Sprite Deserialize(int width, int height, string imageData);
    }
}