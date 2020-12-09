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

        protected virtual XmlNodeInfo ColorInfo { get; } = new XmlNodeInfo("color");
        protected virtual XmlNodeInfo IconInfo { get; } = new XmlNodeInfo("icon");
        protected virtual XmlNodeInfo RoleInfo { get; set; } = new XmlNodeInfo("role");
        protected virtual XmlNodeInfo NameInfo { get; set; } = new XmlNodeInfo("name");


        public virtual bool ShouldSerialize(Character value) => value != null;

        public virtual void Serialize(XmlSerializer serializer, Character value)
        {
            serializer.AddColor(ColorInfo, value.Color);
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
            character.Color = GetColor(deserializer);
            character.Role = GetRole(deserializer);
            character.Name = GetName(deserializer);

            return character;
        }

        protected virtual Icon GetIcon(XmlDeserializer deserializer)
            => deserializer.GetValue(IconInfo, IconFactory);
        protected virtual Color GetColor(XmlDeserializer deserializer)
            => deserializer.GetColor(ColorInfo);
        protected virtual string GetRole(XmlDeserializer deserializer)
            => deserializer.GetString(NameInfo);
        protected virtual string GetName(XmlDeserializer deserializer)
            => deserializer.GetString(NameInfo);
    }
}
