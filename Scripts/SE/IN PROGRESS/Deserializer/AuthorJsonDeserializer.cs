using ClinicalTools.SimEncounters;
using SimpleJSON;

namespace ClinicalTools.Lift
{
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