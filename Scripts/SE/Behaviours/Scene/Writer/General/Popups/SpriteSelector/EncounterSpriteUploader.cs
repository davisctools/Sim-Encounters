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
        protected KeyedCollection<Sprite> SpriteCollection => Encounter?.Content.ImageContent.Sprites;
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
                SetImage(Encounter.Metadata.Sprite);
            else if (CurrentKey != null && SpriteCollection.ContainsKey(CurrentKey))
                SetImage(SpriteCollection[CurrentKey]);
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
            if (CurrentKey != null && SpriteCollection.ContainsKey(CurrentKey))
                SpriteCollection.Remove(CurrentKey);

            CurrentWaitableSpriteKey.SetResult(EncounterImageKey);
        }

        protected virtual void ApplyContentImage()
        {
            if (CurrentImage != null) {
                if (CurrentKey != null && SpriteCollection.ContainsKey(CurrentKey))
                    SpriteCollection[CurrentKey] = CurrentImage;
                else if (CurrentKey != null)
                    SpriteCollection.Add(CurrentKey, CurrentImage);
                else
                    CurrentKey = SpriteCollection.Add(CurrentImage);
            }

            CurrentWaitableSpriteKey.SetResult(CurrentKey);

        }

        protected override void Remove()
        {
            if (CurrentKey != null && SpriteCollection.ContainsKey(CurrentKey))
                SpriteCollection.Remove(CurrentKey);

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