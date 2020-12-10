using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class IconSpriteRetriever : IIconSpriteRetriever
    {
        protected virtual string IconsResourcePath => "Icons";
        protected virtual Dictionary<string, Sprite> ResourceSpriteDictionary { get; } = new Dictionary<string, Sprite>();

        public Sprite GetIconSprite(Encounter encounter, Icon icon)
        {
            if (icon == null)
                return null;

            switch (icon.Type) {
                case Icon.IconType.EncounterImage:
                    return encounter.Metadata.Sprite;
                case Icon.IconType.Upload:
                    var sprites = encounter.Content.ImageContent.Sprites;
                    return sprites.ContainsKey(icon.Reference) ? sprites[icon.Reference] : null;
                case Icon.IconType.Resource:
                    if (icon.Reference == null)
                        return null;

                    if (ResourceSpriteDictionary.ContainsKey(icon.Reference))
                        return ResourceSpriteDictionary[icon.Reference];

                    var sprite = Resources.Load<Sprite>(Path.Combine(IconsResourcePath, icon.Reference));
                    ResourceSpriteDictionary.Add(icon.Reference, sprite);
                    return sprite;
                default:
                    return null;
            }
        }

    }
}