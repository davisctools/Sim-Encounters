using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public interface IIconSpriteRetriever
    {
        Sprite GetIconSprite(ContentEncounter encounter, Icon icon);
    }
}