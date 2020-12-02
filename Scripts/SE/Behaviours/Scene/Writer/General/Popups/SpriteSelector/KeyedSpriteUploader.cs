using ClinicalTools.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Button))]
    public class EncounterImageSelectorButton
    {

    }

    public class KeyedSpriteUploader : BaseSpriteUploader, IKeyedSpriteSelector
    {
        protected WaitableTask<string> CurrentWaitableSpriteKey { get; set; }
        protected KeyedCollection<Sprite> SpriteCollection { get; set; }
        protected string CurrentKey { get; set; }
        public virtual WaitableTask<string> SelectSprite(KeyedCollection<Sprite> sprites, string spriteKey)
        {
            if (CurrentWaitableSpriteKey?.IsCompleted() == false)
                CurrentWaitableSpriteKey.SetError(new Exception("New popup opened"));

            gameObject.SetActive(true);
            SpriteCollection = sprites;
            CurrentKey = spriteKey;
            CurrentWaitableSpriteKey = new WaitableTask<string>();

            if (spriteKey != null && SpriteCollection.ContainsKey(spriteKey))
                SetImage(SpriteCollection[spriteKey]);
            else
                SetImage(null);

            return CurrentWaitableSpriteKey;
        }

        protected override void ApplyClicked()
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

            base.ApplyClicked();
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