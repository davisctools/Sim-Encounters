using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ImageContentXmlSerializer : IObjectSerializer<LegacyEncounterImageContent>
    {
        protected virtual IObjectSerializer<Sprite> SpriteFactory { get; } 
        public ImageContentXmlSerializer(IObjectSerializer<Sprite> spriteFactory)
        {
            SpriteFactory = spriteFactory;
        }

        protected virtual XmlCollectionInfo IconsInfo { get; } = new XmlCollectionInfo("icons", "icon");
        protected virtual XmlCollectionInfo SpritesInfo { get; } = new XmlCollectionInfo("sprites", "sprite");

        public virtual bool ShouldSerialize(LegacyEncounterImageContent value) => value != null;

        public void Serialize(IDataSerializer serializer, LegacyEncounterImageContent value)
        {
            //serializer.AddKeyValuePairs(IconsInfo, value.Icons, SpriteFactory);
            serializer.AddKeyValuePairs(SpritesInfo, value.Sprites, SpriteFactory);
        }

        public virtual LegacyEncounterImageContent Deserialize(IDataDeserializer deserializer)
        {
            var imageData = CreateImageData(deserializer);

            AddSprites(deserializer, imageData);

            return imageData;
        }
        
        protected virtual LegacyEncounterImageContent CreateImageData(IDataDeserializer deserializer) => new LegacyEncounterImageContent();

        protected virtual List<KeyValuePair<string, Sprite>> GetSprites(IDataDeserializer deserializer)
            => deserializer.GetKeyValuePairs(SpritesInfo, SpriteFactory);
        protected virtual void AddSprites(IDataDeserializer deserializer, LegacyEncounterImageContent imageData)
        {
            var spritePairs = GetSprites(deserializer);
            if (spritePairs == null)
                return;

            foreach (var spritePair in spritePairs)
                imageData.Sprites.Add(spritePair);
        }
    }
}
