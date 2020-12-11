using ClinicalTools.Collections;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public interface IEncounterSpriteSelector
    {
        WaitableTask<string> SelectSprite(Encounter encounter, string spriteKey);
    }
    public interface IKeyedSpriteSelector
    {
        WaitableTask<string> SelectSprite(KeyedCollection<Sprite> sprites, string spriteKey);
    }
    public interface ISpriteSelector 
    {
        WaitableTask<Sprite> SelectSprite(Sprite sprite);
    }
}