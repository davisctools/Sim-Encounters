using ClinicalTools.SimEncounters;
using SimpleJSON;
using System.Collections.Generic;

namespace ClinicalTools.Lift
{
    public class EncounterImagesJsonDeserializer : IStringDeserializer<List<EncounterImage>>, IJsonDeserializer<List<EncounterImage>>
    {
        protected IJsonDeserializer<EncounterImage> ImageDeserializer { get; }
        public EncounterImagesJsonDeserializer(IJsonDeserializer<EncounterImage> imageDeserializer) 
            => ImageDeserializer = imageDeserializer;

        public virtual List<EncounterImage> Deserialize(string text) => Deserialize(JSON.Parse(text));
        

        public virtual List<EncounterImage> Deserialize(JSONNode node)
        {
            if (node == null)
                return new List<EncounterImage>();

            var imagesNode = node["images"];
            if (imagesNode == null)
                imagesNode = node;
            var images = new List<EncounterImage>();
            foreach (var imageNode in imagesNode) {
                var image = ImageDeserializer.Deserialize(imageNode);
                if (image != null)
                    images.Add(image);
            }

            return images;
        }
    }
}