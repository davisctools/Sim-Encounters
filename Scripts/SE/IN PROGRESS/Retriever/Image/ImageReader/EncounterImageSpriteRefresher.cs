using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class EncounterImageSpriteRefresher : IEncounterImageSpriteRefresher
    {
        protected IEncounterImageTextureRetriever TextureRetriever { get; }
        public EncounterImageSpriteRefresher(IEncounterImageTextureRetriever spriteRetriever)
            => TextureRetriever = spriteRetriever;

        public WaitableTask RefreshTexture(User user, EncounterMetadata metadata, EncounterImage image)
        {
            if (metadata.Image?.Sprite != null && metadata.Image.Id == image.Id) {
                image.Sprite = metadata.Image.Sprite;
                return WaitableTask.CompletedTask;
            }

            var spriteTask = TextureRetriever.GetTexture(user, metadata, image);
            var task = new WaitableTask();
            spriteTask.AddOnCompletedListener((result) => ProcessResults(task, image, result));

            return task;
        }

        protected virtual void ProcessResults(WaitableTask task, EncounterImage image, TaskResult<Texture2D> result)
        {
            if (result == null || result.IsError()) {
                task.SetError(result.Exception);
                return;
            }

            var texture = result.Value;
            if (texture == null) {
                task.SetError(new Exception("Could not get texture."));
                return;
            }

            image.Sprite = CreateSprite(texture);
            task.SetCompleted();
        }

        protected virtual Sprite CreateSprite(Texture2D texture) 
            => Sprite.Create(
                texture, 
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(texture.width / 2, texture.height / 2));
    }
}
