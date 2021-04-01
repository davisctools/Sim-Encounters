using ClinicalTools.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class EncounterSpriteUploader : BaseSpriteUploader, IEncounterSpriteSelector
    {
        [SerializeField] private Toggle useEncounterImageToggle;

        protected WaitableTask<string> CurrentWaitableSpriteKey { get; set; }
        protected Encounter Encounter { get; set; }
        protected KeyedCollection<EncounterImage> ImageCollection => Encounter?.Content.Images;
        protected string CurrentKey { get; set; }

        protected virtual string LegacyEncounterImageKey => "patientImage";
        protected virtual string EncounterImageKey => "encounterImage";

        protected virtual void Start()
            => useEncounterImageToggle.onValueChanged.AddListener(OnUseEncounterImageToggled);

        protected virtual void OnUseEncounterImageToggled(bool isOn) => SetImage();

        protected override string GetImagePath()
        {
            var imagePath = base.GetImagePath();
            if (!string.IsNullOrWhiteSpace(imagePath))
                useEncounterImageToggle.isOn = false;
            return imagePath;
        }

        public virtual WaitableTask<string> SelectSprite(Encounter encounter, string spriteKey)
        {
            if (CurrentWaitableSpriteKey?.IsCompleted() == false)
                CurrentWaitableSpriteKey.SetError(new Exception("New popup opened"));

            gameObject.SetActive(true);
            Encounter = encounter;
            var isEncounterImage = KeyIsEncounterImage(spriteKey);
            CurrentKey = !isEncounterImage ? spriteKey : null;
            CurrentWaitableSpriteKey = new WaitableTask<string>();
            useEncounterImageToggle.SetIsOnWithoutNotify(isEncounterImage);

            SetImage();

            return CurrentWaitableSpriteKey;
        }

        protected virtual void SetImage()
        {
            if (useEncounterImageToggle.isOn)
                SetImage(Encounter.Metadata.Image.Sprite);
            else if (CurrentKey != null && ImageCollection.ContainsKey(CurrentKey))
                SetImage(ImageCollection[CurrentKey].Sprite);
            else
                SetImage(null);

        }

        protected virtual bool KeyIsEncounterImage(string key)
            => key != null && (key.Equals(LegacyEncounterImageKey, StringComparison.InvariantCultureIgnoreCase) || key.Equals(EncounterImageKey, StringComparison.InvariantCultureIgnoreCase));

        protected override void ApplyClicked()
        {
            if (useEncounterImageToggle.isOn)
                ApplyEncounterImage();
            else
                ApplyContentImage();
            base.ApplyClicked();
        }

        protected virtual void ApplyEncounterImage()
        {
            if (CurrentKey != null && ImageCollection.ContainsKey(CurrentKey))
                ImageCollection.Remove(CurrentKey);

            CurrentWaitableSpriteKey.SetResult(EncounterImageKey);
        }

        protected virtual void ApplyContentImage()
        {
            if (CurrentImage != null) {
                if (CurrentKey != null && ImageCollection.ContainsKey(CurrentKey))
                    ImageCollection[CurrentKey].Sprite = CurrentImage;
                else if (CurrentKey != null)
                    ImageCollection.Add(CurrentKey, new EncounterImage());// CurrentImage); !!!!TODO
                else
                    CurrentKey = ImageCollection.Add(new EncounterImage());// CurrentImage); !!!!TODO
            }

            CurrentWaitableSpriteKey.SetResult(CurrentKey);

        }

        protected override void Remove()
        {
            if (CurrentKey != null && ImageCollection.ContainsKey(CurrentKey))
                ImageCollection.Remove(CurrentKey);

            CurrentWaitableSpriteKey.SetResult(null);

            base.Remove();
        }

        protected override void Close()
        {
            if (CurrentWaitableSpriteKey?.IsCompleted() == false)
                CurrentWaitableSpriteKey.SetError(new Exception("Canceled"));

            base.Close();
        }
    }
}