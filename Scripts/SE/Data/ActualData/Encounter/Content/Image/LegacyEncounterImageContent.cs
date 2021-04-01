using ClinicalTools.Collections;
using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class LegacyEncounterImageContent
    {
        public virtual KeyedCollection<Sprite> Sprites { get; } = new KeyedCollection<Sprite>();
        public virtual KeyedCollection<Sprite> Icons { get; } = new KeyedCollection<Sprite>();

        protected virtual string DefaultIconsFolder { get; } = "Icons";

        public LegacyEncounterImageContent()
        {
            var iconSprites = Resources.LoadAll<Sprite>(DefaultIconsFolder);
            foreach (var iconSprite in iconSprites) 
                Icons.Add(iconSprite.name, iconSprite);
        }
    }
}