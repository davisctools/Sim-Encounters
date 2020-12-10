using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class IconXmlSerializer : IXmlSerializer<Icon>
    {
        protected virtual XmlNodeInfo ReferenceInfo { get; } = new XmlNodeInfo("ref");
        protected virtual XmlNodeInfo TypeInfo { get; set; } = new XmlNodeInfo("type");
        protected virtual XmlNodeInfo ColorInfo { get; } = new XmlNodeInfo("color");

        public virtual bool ShouldSerialize(Icon value) => value != null;

        public virtual void Serialize(XmlSerializer serializer, Icon value)
        {
            serializer.AddString(TypeInfo, GetTypeString(value.Type));
            if (!string.IsNullOrWhiteSpace(value.Reference))
                serializer.AddString(ReferenceInfo, value.Reference);
            if (value.Color != Color.white)
                serializer.AddColor(ColorInfo, value.Color);
        }

        protected virtual string ResourceTypeString => "resource";
        protected virtual string UploadTypeString => "upload";
        protected virtual string EncounterImageTypeString => "encounter";
        protected virtual string GetTypeString(Icon.IconType type)
        {
            switch (type) {
                case Icon.IconType.Resource:
                    return ResourceTypeString;
                case Icon.IconType.Upload:
                    return UploadTypeString;
                case Icon.IconType.EncounterImage:
                    return EncounterImageTypeString;
                default:
                    return null;
            }
        }

        public virtual Icon Deserialize(XmlDeserializer deserializer)
        {
            var type = GetType(deserializer);
            var reference = GetReference(deserializer);
            var color = GetColor(deserializer);

            return new Icon(type, reference, color);
        }


        protected virtual Icon.IconType GetType(XmlDeserializer deserializer)
        {
            var typeStr = deserializer.GetString(TypeInfo);
            if (typeStr.Equals(UploadTypeString, StringComparison.InvariantCultureIgnoreCase))
                return Icon.IconType.Upload;
            else if (typeStr.Equals(EncounterImageTypeString, StringComparison.InvariantCultureIgnoreCase))
                return Icon.IconType.EncounterImage;
            else
                return Icon.IconType.Resource;
        }
        
        protected virtual string GetReference(XmlDeserializer deserializer)
            => deserializer.GetString(ReferenceInfo);

        protected virtual Color GetColor(XmlDeserializer deserializer)
        {
            var color = deserializer.GetColor(ColorInfo);
            return color != Color.clear ? color : Color.white;
        }
    }
}
