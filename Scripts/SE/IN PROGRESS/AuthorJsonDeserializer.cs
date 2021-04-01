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
    public class EncounterImageJsonDeserializer : IJsonDeserializer<EncounterImage>
    {
        public EncounterImage Deserialize(JSONNode node)
            => new EncounterImage() {
                DateModified = node["date"],
                Id = node["id"],
                Key = node["key"],
                FileName = node["file"]
            };
    }
    public class AuthorJsonDeserializer : IJsonDeserializer<Author>
    {
        protected IJsonDeserializer<Name> NameDeserializer { get; }
        public AuthorJsonDeserializer(IJsonDeserializer<Name> nameDeserializer) => NameDeserializer = nameDeserializer;

        public Author Deserialize(JSONNode node) 
            => new Author(node["id"]) {
                Name = NameDeserializer.Deserialize(node["name"])
            };
    }
}