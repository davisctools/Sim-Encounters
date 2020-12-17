using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class LegacyNonImageContentXmlSerializer : NonImageContentXmlSerializer
    {
        public LegacyNonImageContentXmlSerializer(IXmlSerializer<Section> sectionFactory, IXmlSerializer<Character> characterFactory)
            : base(sectionFactory, characterFactory) { }

        private readonly Character _patientCharacter = new Character {
            PrimaryColor = new Color(0.937f, 0.953f, 0.965f),
            SecondaryColor = new Color(0.937f, 0.953f, 0.965f),
            Icon = new Icon(),
            Role = "Patient"
        };
        private readonly Character _instructorCharacter = new Character {
            PrimaryColor = new Color(0.937f, 0.953f, 0.965f),
            SecondaryColor = new Color(0.937f, 0.953f, 0.965f),
            Icon = new Icon(Icon.IconType.Resource, "whitecoat", new Color(0.1490196f, 0.1882353f, 0.2196078f)),
            Role = "Instructor"
        };
        private readonly Character _providerCharacter = new Character {
            PrimaryColor = new Color(0.372549f, 0.6862745f, 0.6745098f),
            SecondaryColor = new Color(0.827451f, 0.9058824f, 0.9098039f),
            Icon = new Icon(Icon.IconType.Resource, "provider-white", Color.white),
            Role = "Provider"
        };

        protected override List<KeyValuePair<string, Character>> GetCharacters(XmlDeserializer deserializer)
        {
            var characters = base.GetCharacters(deserializer);
            if (characters?.Count > 0)
                return characters;

            return new List<KeyValuePair<string, Character>> {
                new KeyValuePair<string, Character>("Provider", _providerCharacter),
                new KeyValuePair<string, Character>("Patient", _patientCharacter),
                new KeyValuePair<string, Character>("Instructor", _instructorCharacter)
            };
        }
    }
}
