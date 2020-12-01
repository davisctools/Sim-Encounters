using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ImageContentXmlSerializer : IXmlSerializer<EncounterImageContent>
    {
        protected virtual IXmlSerializer<Sprite> SpriteFactory { get; } 
        public ImageContentXmlSerializer(IXmlSerializer<Sprite> spriteFactory)
        {
            SpriteFactory = spriteFactory;
        }

        protected virtual XmlCollectionInfo IconsInfo { get; } = new XmlCollectionInfo("icons", "icon");
        protected virtual XmlCollectionInfo SpritesInfo { get; } = new XmlCollectionInfo("sprites", "sprite");

        public virtual bool ShouldSerialize(EncounterImageContent value) => value != null;

        public void Serialize(XmlSerializer serializer, EncounterImageContent value)
        {
            //serializer.AddKeyValuePairs(IconsInfo, value.Icons, SpriteFactory);
            serializer.AddKeyValuePairs(SpritesInfo, value.Sprites, SpriteFactory);
        }

        public virtual EncounterImageContent Deserialize(XmlDeserializer deserializer)
        {
            var imageData = CreateImageData(deserializer);

            AddSprites(deserializer, imageData);

            return imageData;
        }
        
        protected virtual EncounterImageContent CreateImageData(XmlDeserializer deserializer) => new EncounterImageContent();

        protected virtual List<KeyValuePair<string, Sprite>> GetSprites(XmlDeserializer deserializer)
            => deserializer.GetKeyValuePairs(SpritesInfo, SpriteFactory);
        protected virtual void AddSprites(XmlDeserializer deserializer, EncounterImageContent imageData)
        {
            var spritePairs = GetSprites(deserializer);
            if (spritePairs == null)
                return;

            foreach (var spritePair in spritePairs)
                imageData.Sprites.Add(spritePair);
        }
    }
}
