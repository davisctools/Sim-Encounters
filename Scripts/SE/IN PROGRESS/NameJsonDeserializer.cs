using ClinicalTools.SimEncounters;
using SimpleJSON;

namespace ClinicalTools.Lift
{
    public class NameJsonDeserializer : IJsonDeserializer<Name>
    {
        public Name Deserialize(JSONNode node)
        {
            var name = new Name();

            var honorific = node["honorific"];
            if (honorific != null)
                name.Honorific = honorific;
            var firstName = node["first"];
            if (firstName != null)
                name.FirstName = firstName;
            var lastName = node["last"];
            if (lastName != null)
                name.LastName = lastName;

            return name;
        }
    }
}