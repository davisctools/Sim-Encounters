using ClinicalTools.SEColors;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class CharacterXmlSerializer : IXmlSerializer<Character>
    {
        protected virtual IXmlSerializer<Icon> IconFactory { get; }
        protected virtual ICharacterColorThemeManager ColorThemeManager { get; } = new CharacterColorThemeManager();
        public CharacterXmlSerializer(IXmlSerializer<Icon> iconFactory) => IconFactory = iconFactory;

        protected virtual XmlNodeInfo ColorThemeInfo { get; } = new XmlNodeInfo("colorTheme");
        protected virtual XmlNodeInfo SecondaryColorInfo { get; } = new XmlNodeInfo("color2");
        protected virtual XmlNodeInfo IconInfo { get; } = new XmlNodeInfo("icon");
        protected virtual XmlNodeInfo RoleInfo { get; set; } = new XmlNodeInfo("role");
        protected virtual XmlNodeInfo NameInfo { get; set; } = new XmlNodeInfo("name");


        public virtual bool ShouldSerialize(Character value) => value != null;

        public virtual void Serialize(XmlSerializer serializer, Character value)
        {
            serializer.AddInt(ColorThemeInfo, ColorThemeManager.GetThemeIndex(value.ColorTheme));
            if (value.Icon != null)
                serializer.AddValue(IconInfo, value.Icon, IconFactory);
            if (!string.IsNullOrWhiteSpace(value.Role))
                serializer.AddString(RoleInfo, value.Role);
            if (!string.IsNullOrWhiteSpace(value.Name))
                serializer.AddString(NameInfo, value.Name);
        }

        public virtual Character Deserialize(XmlDeserializer deserializer)
            => new Character {
                Icon = GetIcon(deserializer),
                ColorTheme = GetColorTheme(deserializer),
                Role = GetRole(deserializer),
                Name = GetName(deserializer)
            };

        protected virtual Icon GetIcon(XmlDeserializer deserializer)
            => deserializer.GetValue(IconInfo, IconFactory);
        protected virtual CharacterColorTheme GetColorTheme(XmlDeserializer deserializer)
        {
            var colorThemeIndex = deserializer.GetInt(ColorThemeInfo);
            if (colorThemeIndex >= 0)
                return ColorThemeManager.GetColorTheme(colorThemeIndex);
            else
                return ColorThemeManager.GetColorTheme(GetSecondaryColor(deserializer));
        }

        protected virtual Color GetSecondaryColor(XmlDeserializer deserializer)
            => deserializer.GetColor(SecondaryColorInfo);
        protected virtual string GetRole(XmlDeserializer deserializer)
            => deserializer.GetString(RoleInfo);
        protected virtual string GetName(XmlDeserializer deserializer)
            => deserializer.GetString(NameInfo);
    }
}
