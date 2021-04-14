using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class IconSpriteRetriever : IIconSpriteRetriever
    {
        protected virtual string IconsResourcePath => "Icons";
        protected virtual Dictionary<string, Sprite> ResourceSpriteDictionary { get; } = new Dictionary<string, Sprite>();

        public Sprite GetIconSprite(ContentEncounter encounter, Icon icon)
        {
            if (icon == null)
                return null;

            switch (icon.Type) {
                case Icon.IconType.EncounterImage:
                    return encounter.Metadata.Image?.Sprite;
                case Icon.IconType.Upload:
                    var sprites = encounter.Content.Images;
                    return sprites.ContainsKey(icon.Reference) ? sprites[icon.Reference].Sprite : null;
                case Icon.IconType.Resource:
                    if (icon.Reference == null)
                        return null;

                    var reference = icon.Reference;
                    if (icon.Reference.Equals("instructor", StringComparison.InvariantCultureIgnoreCase)) {
                        reference = "Characters/whitecoat";
                        icon.Color = Color.white;
                    } else  if (icon.Reference.Equals("provider", StringComparison.InvariantCultureIgnoreCase)) {
                        reference = "Characters/provider-white";
                        icon.Color = Color.white;
                    }
                    if (ResourceSpriteDictionary.ContainsKey(reference))
                        return ResourceSpriteDictionary[reference];

                    var sprite = Resources.Load<Sprite>(Path.Combine(IconsResourcePath, reference));
                    ResourceSpriteDictionary.Add(reference, sprite);
                    return sprite;
                default:
                    return null;
            }
        }

    }
}