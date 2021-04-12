using Crosstales.FB;
using System.IO;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class SpriteSelector : ISpriteSelector2
    {
        public virtual TextureFormat TextureFormat { get; } = TextureFormat.RGBA32;
        public virtual SpriteData SelectSprite()
        {
            var filePath = GetImagePath();
            if (string.IsNullOrWhiteSpace(filePath))
                return null;

            byte[] bytes = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2, TextureFormat, false);
            texture.LoadImage(bytes);
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), 100);
            return new SpriteData(sprite, filePath, bytes);
        }

        protected virtual string[] ImageExtensions { get; } = new string[] { "png", "jpg", "jpeg" };
        protected virtual string GetImagePath() => FileBrowser.OpenSingleFile("Upload Image", "", ImageExtensions);
    }
}