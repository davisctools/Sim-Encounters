using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class SpriteDeserializer : ISpriteDeserializer
    {
        public Sprite Deserialize(int width, int height, string imageData)
        {
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
    }
}