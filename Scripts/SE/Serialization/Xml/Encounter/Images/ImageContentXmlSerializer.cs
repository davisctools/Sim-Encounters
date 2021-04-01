using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ImageContentXmlSerializer : IXmlSerializer<LegacyEncounterImageContent>
    {
        protected virtual IXmlSerializer<Sprite> SpriteFactory { get; } 
        public ImageContentXmlSerializer(IXmlSerializer<Sprite> spriteFactory)
        {
            SpriteFactory = spriteFactory;
        }

        protected virtual XmlCollectionInfo IconsInfo { get; } = new XmlCollectionInfo("icons", "icon");
        protected virtual XmlCollectionInfo SpritesInfo { get; } = new XmlCollectionInfo("sprites", "sprite");

        public virtual bool ShouldSerialize(LegacyEncounterImageContent value) => value != null;

        public void Serialize(XmlSerializer serializer, LegacyEncounterImageContent value)
        {
            //serializer.AddKeyValuePairs(IconsInfo, value.Icons, SpriteFactory);
            serializer.AddKeyValuePairs(SpritesInfo, value.Sprites, SpriteFactory);
        }

        public virtual LegacyEncounterImageContent Deserialize(XmlDeserializer deserializer)
        {
            var imageData = CreateImageData(deserializer);

            AddSprites(deserializer, imageData);

            return imageData;
        }
        
        protected virtual LegacyEncounterImageContent CreateImageData(XmlDeserializer deserializer) => new LegacyEncounterImageContent();

        protected virtual List<KeyValuePair<string, Sprite>> GetSprites(XmlDeserializer deserializer)
            => deserializer.GetKeyValuePairs(SpritesInfo, SpriteFactory);
        protected virtual void AddSprites(XmlDeserializer deserializer, LegacyEncounterImageContent imageData)
        {
            var spritePairs = GetSprites(deserializer);
            if (spritePairs == null)
                return;

            foreach (var spritePair in spritePairs)
                imageData.Sprites.Add(spritePair);
        }
    }
}
