using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class SpriteXmlSerializer : IXmlSerializer<Sprite>
    {
        protected virtual XmlNodeInfo WidthName { get; } = new XmlNodeInfo("width");
        protected virtual XmlNodeInfo HeightName { get; } = new XmlNodeInfo("height");
        protected virtual XmlNodeInfo DataName { get; } = new XmlNodeInfo("data");

        public virtual bool ShouldSerialize(Sprite value) => value != null;

        public void Serialize(XmlSerializer serializer, Sprite sprite)
        {
            serializer.AddInt(WidthName, sprite.texture.width);
            serializer.AddInt(HeightName, sprite.texture.height);
            serializer.AddString(DataName, GetTextureString(sprite));
        }

        protected virtual int GetWidth(XmlDeserializer deserializer)
            => deserializer.GetInt(WidthName);
        protected virtual int GetHeight(XmlDeserializer deserializer)
            => deserializer.GetInt(HeightName);
        protected virtual string GetImageData(XmlDeserializer deserializer)
            => deserializer.GetString(DataName);
        public Sprite Deserialize(XmlDeserializer deserializer)
        {
            int width = GetWidth(deserializer);
            int height = GetHeight(deserializer);
            string imageData = GetImageData(deserializer);//.Replace(' ', '+');
            if (imageData == null)
                throw new Exception("No image data included.");

            var imageRect = new Rect(0, 0, width, height);
            return GetSprite(imageRect, imageData);
        }

        protected virtual Sprite GetSprite(Rect imageRect, string imageData)
        {
            var imageBytes = Convert.FromBase64String(imageData);
            Texture2D temp = new Texture2D(2, 2);
            temp.LoadImage(imageBytes);
            return Sprite.Create(temp, imageRect, Vector2.zero, 100);
        }

        protected virtual string GetTextureString(Sprite sprite)
        {
            Texture2D texture = GetTexture(sprite);

            byte[] bytes;
            if (sprite.texture.format == TextureFormat.RGB24)
                bytes = texture.EncodeToJPG();
            else
                bytes = texture.EncodeToPNG();

            return Convert.ToBase64String(bytes);
        }

        protected virtual Texture2D GetTexture(Sprite sprite)
        {
            if (sprite.texture.format == TextureFormat.DXT1 || sprite.texture.format == TextureFormat.DXT5)
                return DecompressDXT(sprite.texture);

            TextureFormat format;
            if (sprite.texture.format == TextureFormat.RGB24)
                format = TextureFormat.RGB24;
            else
                format = TextureFormat.RGBA32;
            Texture2D texture = new Texture2D(sprite.texture.width, sprite.texture.height, format, false);

            texture.SetPixels(0, 0, sprite.texture.width, sprite.texture.height, sprite.texture.GetPixels());
            texture.Apply();

            return texture;
        }


        /// <summary>
        /// https://stackoverflow.com/questions/51315918/how-to-encodetopng-compressed-textures-in-unity
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        protected virtual Texture2D DecompressDXT(Texture2D source)
        {
            RenderTexture renderTex = RenderTexture.GetTemporary(
                        source.width,
                        source.height,
                        0,
                        RenderTextureFormat.Default,
                        RenderTextureReadWrite.Linear);

            Graphics.Blit(source, renderTex);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableText = new Texture2D(source.width, source.height);
            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);
            return readableText;
        }
    }
}