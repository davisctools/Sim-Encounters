using ClinicalTools.Collections;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{

    public class EncounterImageContent
    {
        public virtual KeyedCollection<Sprite> Sprites { get; } = new KeyedCollection<Sprite>();
        public virtual KeyedCollection<Sprite> Icons { get; } = new KeyedCollection<Sprite>();

        protected virtual string DefaultIconsFolder { get; } = "Section Icons";

        public EncounterImageContent()
        {
            var iconSprites = Resources.LoadAll<Sprite>(DefaultIconsFolder);
            foreach (var iconSprite in iconSprites) 
                Icons.Add(iconSprite.name, iconSprite);
        }
    }
}