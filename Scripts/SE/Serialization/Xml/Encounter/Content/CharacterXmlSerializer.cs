using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class CharacterXmlSerializer : IXmlSerializer<Character>
    {
        protected virtual IXmlSerializer<Icon> IconFactory { get; }
        public CharacterXmlSerializer(IXmlSerializer<Icon> iconFactory)
        {
            IconFactory = iconFactory;
        }

        protected virtual XmlNodeInfo PrimaryColorInfo { get; } = new XmlNodeInfo("color1");
        protected virtual XmlNodeInfo SecondaryColorInfo { get; } = new XmlNodeInfo("color2");
        protected virtual XmlNodeInfo IconInfo { get; } = new XmlNodeInfo("icon");
        protected virtual XmlNodeInfo RoleInfo { get; set; } = new XmlNodeInfo("role");
        protected virtual XmlNodeInfo NameInfo { get; set; } = new XmlNodeInfo("name");


        public virtual bool ShouldSerialize(Character value) => value != null;

        public virtual void Serialize(XmlSerializer serializer, Character value)
        {
            serializer.AddColor(PrimaryColorInfo, value.PrimaryColor);
            serializer.AddColor(SecondaryColorInfo, value.SecondaryColor);
            if (value.Icon != null)
                serializer.AddValue(IconInfo, value.Icon, IconFactory);
            if (!string.IsNullOrWhiteSpace(value.Role))
                serializer.AddString(RoleInfo, value.Role);
            if (!string.IsNullOrWhiteSpace(value.Name))
                serializer.AddString(NameInfo, value.Name);
        }

        public virtual Character Deserialize(XmlDeserializer deserializer)
        {
            var character = new Character();
            character.Icon = GetIcon(deserializer);

            var primaryColor = GetPrimaryColor(deserializer);
            if (primaryColor != Color.white)
                character.PrimaryColor = primaryColor;

            var secondaryColor = GetSecondaryColor(deserializer);
            if (secondaryColor != Color.white)
                character.SecondaryColor = secondaryColor;

            character.Role = GetRole(deserializer);
            character.Name = GetName(deserializer);

            return character;
        }

        protected virtual Icon GetIcon(XmlDeserializer deserializer)
            => deserializer.GetValue(IconInfo, IconFactory);
        protected virtual Color GetPrimaryColor(XmlDeserializer deserializer)
            => deserializer.GetColor(PrimaryColorInfo);
        protected virtual Color GetSecondaryColor(XmlDeserializer deserializer)
            => deserializer.GetColor(SecondaryColorInfo);
        protected virtual string GetRole(XmlDeserializer deserializer)
            => deserializer.GetString(RoleInfo);
        protected virtual string GetName(XmlDeserializer deserializer)
            => deserializer.GetString(NameInfo);
    }
}
