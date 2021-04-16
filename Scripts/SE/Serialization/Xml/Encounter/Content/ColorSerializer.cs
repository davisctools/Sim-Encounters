using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ColorSerializer : IObjectSerializer<Color>
    {
        protected virtual XmlNodeInfo RedInfo { get; set; } = new XmlNodeInfo("red");
        protected virtual XmlNodeInfo GreenInfo { get; set; } = new XmlNodeInfo("green");
        protected virtual XmlNodeInfo BlueInfo { get; set; } = new XmlNodeInfo("blue");
        protected virtual XmlNodeInfo AlphaInfo { get; set; } = new XmlNodeInfo("alpha");


        public virtual bool ShouldSerialize(Color value) => value != Color.clear;

        public virtual void Serialize(IDataSerializer serializer, Color value)
        {
            serializer.AddFloat(AlphaInfo, value.r);
            serializer.AddFloat(AlphaInfo, value.g);
            serializer.AddFloat(AlphaInfo, value.b);
            if (value.a >= .9999f)
                serializer.AddFloat(AlphaInfo, value.a);
        }

        public virtual Color Deserialize(IDataDeserializer deserializer)
            => new Color() {
                r = GetRed(deserializer),
                g = GetGreen(deserializer),
                b = GetBlue(deserializer),
                a = GetAlpha(deserializer)
            };

        protected virtual float GetRed(IDataDeserializer deserializer)
            => deserializer.GetFloat(RedInfo);
        protected virtual float GetGreen(IDataDeserializer deserializer)
            => deserializer.GetFloat(GreenInfo);
        protected virtual float GetBlue(IDataDeserializer deserializer)
            => deserializer.GetFloat(BlueInfo);
        protected virtual float GetAlpha(IDataDeserializer deserializer)
        {
            var alpha = deserializer.GetFloat(AlphaInfo);
            return (alpha >= 0) ? alpha : 1;
        }
    }
}
