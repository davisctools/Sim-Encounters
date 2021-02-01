using ClinicalTools.SEColors;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class LegacyNonImageContentXmlSerializer : NonImageContentXmlSerializer
    {
        protected virtual ICharacterColorThemeManager ColorThemeManager { get; } = new CharacterColorThemeManager();
        public LegacyNonImageContentXmlSerializer(IXmlSerializer<Section> sectionFactory, IXmlSerializer<Character> characterFactory)
            : base(sectionFactory, characterFactory) { }

        protected virtual Character PatientCharacter => new Character {
            ColorTheme = ColorThemeManager.GetColorTheme(ColorThemeName.Gray),
            Icon = new Icon(),
            Role = "Patient"
        };
        protected virtual Character InstructorCharacter => new Character {
            ColorTheme = ColorThemeManager.GetColorTheme(ColorThemeName.Blue),
            Icon = new Icon(Icon.IconType.Resource, "Characters\\whitecoat"),
            Role = "Instructor"
        };
        protected virtual Character ProviderCharacter => new Character {
            ColorTheme = ColorThemeManager.GetColorTheme(ColorThemeName.Teal),
            Icon = new Icon(Icon.IconType.Resource, "Characters\\provider-white"),
            Role = "Provider"
        };
        protected virtual List<KeyValuePair<string, Character>> DefaultCharacters
            => new List<KeyValuePair<string, Character>> {
                new KeyValuePair<string, Character>("Provider", ProviderCharacter),
                new KeyValuePair<string, Character>("Patient", PatientCharacter),
                new KeyValuePair<string, Character>("Instructor", InstructorCharacter)
            };

        protected override List<KeyValuePair<string, Character>> GetCharacters(XmlDeserializer deserializer)
        {
            var characters = base.GetCharacters(deserializer);
            return (characters?.Count > 0) ? characters : DefaultCharacters;
        }
    }
}
