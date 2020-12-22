using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public interface IIconSpriteRetriever
    {
        Sprite GetIconSprite(Encounter encounter, Icon icon);
    }
}