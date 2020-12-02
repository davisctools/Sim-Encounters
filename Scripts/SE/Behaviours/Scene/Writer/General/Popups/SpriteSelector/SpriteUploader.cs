using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class SpriteUploader : BaseSpriteUploader, ISpriteSelector
    {
        protected WaitableTask<Sprite> CurrentWaitableSprite { get; set; }
        public virtual WaitableTask<Sprite> SelectSprite(Sprite sprite)
        {
            if (CurrentWaitableSprite?.IsCompleted() == false)
                CurrentWaitableSprite.SetError(new Exception("New popup opened"));

            gameObject.SetActive(true);
            SetImage(sprite);

            CurrentWaitableSprite = new WaitableTask<Sprite>();
            return CurrentWaitableSprite;
        }

        protected override void ApplyClicked()
        {
            CurrentWaitableSprite.SetResult(CurrentImage);
            base.ApplyClicked();
        }
        protected override void Remove()
        {
            CurrentWaitableSprite.SetResult(null);
            base.Remove();
        }
        protected override void Close()
        {
            if (CurrentWaitableSprite?.IsCompleted() == false)
                CurrentWaitableSprite.SetError(new Exception("Canceled"));
            base.Close();
        }

    }
}