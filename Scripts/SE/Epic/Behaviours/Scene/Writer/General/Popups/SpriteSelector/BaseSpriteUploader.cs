using Crosstales.FB;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class BaseSpriteUploader : MonoBehaviour
    {
        public virtual Button CancelButton { get => cancelButton; set => cancelButton = value; }
        [SerializeField] private Button cancelButton;
        public virtual Button ApplyButton { get => applyButton; set => applyButton = value; }
        [SerializeField] private Button applyButton;
        public virtual Button UploadImageButton { get => uploadImageButton; set => uploadImageButton = value; }
        [SerializeField] private Button uploadImageButton;
        public virtual Button RemoveImageButton { get => removeImageButton; set => removeImageButton = value; }
        [SerializeField] private Button removeImageButton;
        public virtual GameObject NoImageGameObject { get => noImageGameObject; set => noImageGameObject = value; }
        [SerializeField] private GameObject noImageGameObject;
        public virtual GameObject HasImageGameObject { get => hasImageGameObject; set => hasImageGameObject = value; }
        [SerializeField] private GameObject hasImageGameObject;
        public virtual Image ImageObject { get => imageObject; set => imageObject = value; }
        [SerializeField] private Image imageObject;

        protected virtual void Awake()
        {
            ApplyButton.onClick.AddListener(ApplyClicked);
            CancelButton.onClick.AddListener(Close);

            RemoveImageButton.onClick.AddListener(Remove);
            UploadImageButton.onClick.AddListener(UploadImage);
        }


        protected virtual void ApplyClicked() => Close();
        protected virtual void Remove() => Close();
        protected virtual void Close() => gameObject.SetActive(false);
        

        protected Sprite CurrentImage { get; set; }
        protected virtual void SetImage(Sprite image)
        {
            CurrentImage = image;
            var hasImage = image != null;
            NoImageGameObject.SetActive(!hasImage);
            HasImageGameObject.SetActive(hasImage);
            RemoveImageButton.interactable = hasImage;
            ImageObject.sprite = image;
        }

        protected virtual void UploadImage()
        {
            var image = GetImage();
            if (image != null)
                SetImage(image);
        }

        public virtual TextureFormat TextureFormat { get; } = TextureFormat.RGBA32;
        public virtual int MaxWidth { get; } = 990;
        public virtual int MaxHeight { get; } = 550;
        protected virtual Sprite GetImage()
        {
            var filePath = GetImagePath();
            if (string.IsNullOrWhiteSpace(filePath))
                return null;

            byte[] bytes = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2, TextureFormat, false);
            texture.LoadImage(bytes);
            ScaleTexture(texture, MaxHeight, MaxWidth);

            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), 100);
        }

        protected virtual void ScaleTexture(Texture2D texture, int maxHeight, int maxWidth)
        {
            //Scale to max file size
            var maxRatio = maxHeight / maxWidth;
            var ratio = texture.height / texture.width;

            if (ratio >= maxRatio) {
                if (texture.height <= maxHeight)
                    return;

                var width = GetFittedSecondDimention(texture.height, maxHeight, texture.width);
                TextureScale.Bilinear(texture, width, maxHeight);
            } else if (texture.width > maxWidth) {
                var height = GetFittedSecondDimention(texture.width, maxWidth, texture.height);
                TextureScale.Bilinear(texture, maxWidth, height);
            }
        }

        protected virtual int GetFittedSecondDimention(int firstDimention, int firstDimentionMax, int secondDimention)
        {
            float ratio = (float)firstDimentionMax / firstDimention;
            return (int)(secondDimention * ratio);
        }

        protected virtual string[] ImageExtensions { get; } = new string[] { "png", "jpg", "jpeg" };
        protected virtual string GetImagePath() => FileBrowser.OpenSingleFile("Open Image", "", ImageExtensions);
    }
}