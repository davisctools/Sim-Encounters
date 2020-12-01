using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class SpriteSerializer : IStringSerializer<Sprite>
    {
        public string Serialize(Sprite sprite)
        {
            Texture2D texture = GetTexture(sprite);

            byte[] bytes = (sprite.texture.format == TextureFormat.RGB24) ? texture.EncodeToJPG() : texture.EncodeToPNG();
            return Convert.ToBase64String(bytes);
        }

        public Texture2D GetTexture(Sprite sprite)
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