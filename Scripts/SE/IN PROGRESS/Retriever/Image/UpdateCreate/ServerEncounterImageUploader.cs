using ClinicalTools.Collections;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class ServerEncounterImageUploader : IEncounterImageUploader, IEncounterImageUpdater
    {
        protected IUrlBuilder UrlBuilder { get; }
        protected IServerStringReader ServerReader { get; }
        protected ISpriteSelector2 SpriteSelector { get; }
        protected IStringDeserializer<EncounterImage> ImageDeserializer { get; }
        public ServerEncounterImageUploader(
            IUrlBuilder urlBuilder,
            IServerStringReader serverReader,
            ISpriteSelector2 spriteSelector,
            IStringDeserializer<EncounterImage> imageDeserializer)
        {
            UrlBuilder = urlBuilder;
            ServerReader = serverReader;
            SpriteSelector = spriteSelector;
            ImageDeserializer = imageDeserializer;
        }

        public virtual WaitableTask<EncounterImage> UploadImage(User user, ContentEncounter encounter)
            => GetImageTask(user, encounter, new EncounterImage(), false);
        public WaitableTask<EncounterImage> UpdateImage(User user, ContentEncounter encounter, EncounterImage image)
            => GetImageTask(user, encounter, new EncounterImage() { Key = image.Key }, true);

        protected virtual WaitableTask<EncounterImage> GetImageTask(
            User user, 
            ContentEncounter encounter,
            EncounterImage image,
            bool updating)
        {
            var spriteData = SpriteSelector.SelectSprite();
            if (spriteData == null)
                return new WaitableTask<EncounterImage>(new Exception("No image selected."));

            image.Sprite = spriteData.Sprite;

            var images = encounter.Content.Images;
            if (image.Key == null)
                image.Key = images.Add(image);

            var webRequest = GetWebRequest(user, encounter.Metadata, image, spriteData);
            var serverOutput = ServerReader.Begin(webRequest);

            var task = new WaitableTask<EncounterImage>();
            OnServerResult action = updating ? (OnServerResult)OnUpdateServerResult : (OnServerResult)OnUploadServerResult;
            serverOutput.AddOnCompletedListener((result) => action(task, images, image, result));
            return task;
        }


        protected virtual string Php { get; } = "Main.php";
        protected virtual UnityWebRequest GetWebRequest(User user, OldEncounterMetadata metadata, EncounterImage image, SpriteData spriteData)
        {
            var url = UrlBuilder.BuildUrl(Php);
            var form = new WWWForm();
            AddFormModeFields(form, user, metadata, image);
            AddFormContentFields(form, spriteData);
            return UnityWebRequest.Post(url, form);
        }

        protected virtual string AccountVariable { get; } = "account";
        protected virtual string ModeVariable { get; } = "mode";
        protected virtual string ModeValue { get; } = "encounter";
        protected virtual string ActionVariable { get; } = "action";
        protected virtual string ActionValue { get; } = "save";
        protected virtual string SubactionVariable { get; } = "subaction";
        protected virtual string SubactionValue { get; } = "image";
        protected virtual string EncounterVariable { get; } = "encounter";
        protected virtual string KeyVariable { get; } = "key";
        protected virtual void AddFormModeFields(WWWForm form, User user, OldEncounterMetadata metadata, EncounterImage image)
        {
            form.AddField(AccountVariable, user.AccountId.ToString());
            form.AddField(ModeVariable, ModeValue);
            form.AddField(ActionVariable, ActionValue);
            form.AddField(SubactionVariable, SubactionValue);
            form.AddField(EncounterVariable, metadata.RecordNumber.ToString());
            form.AddField(KeyVariable, image.Key);
        }

        protected virtual string JpgMimeType { get; } = "image/jpeg";
        protected virtual string PngMimeType { get; } = "image/png";
        protected virtual string FileVariable { get; } = "file";
        protected virtual void AddFormContentFields(WWWForm form, SpriteData spriteData)
        {
            var filename = Path.GetFileName(spriteData.Path);
            var extension = Path.GetExtension(filename);
            var mimeType = extension.EndsWith("png", StringComparison.InvariantCultureIgnoreCase) ? PngMimeType : JpgMimeType;
            form.AddBinaryData(FileVariable, spriteData.Bytes, filename, mimeType);
        }

        public delegate void OnServerResult(
            WaitableTask<EncounterImage> task,
            KeyedCollection<EncounterImage> images,
            EncounterImage image,
            TaskResult<string> serverOutput);
        protected virtual void OnUploadServerResult(
            WaitableTask<EncounterImage> task,
            KeyedCollection<EncounterImage> images,
            EncounterImage image,
            TaskResult<string> serverOutput)
        {
            try {
                ProcessResults(images, image, serverOutput);
                task.SetResult(image);
            } catch (Exception ex) {
                images.Remove(image);
                task.SetError(ex);
            }
        }
        protected virtual void OnUpdateServerResult(
            WaitableTask<EncounterImage> task,
            KeyedCollection<EncounterImage> images,
            EncounterImage image,
            TaskResult<string> serverOutput)
        {
            try {
                ProcessResults(images, image, serverOutput);
                var oldImage = images[image.Key];
                images.Remove(image.Key);
                images.AddKeyedValue(image.Key, image);
                task.SetResult(image);
                oldImage.SetUpdated(image);
            } catch (Exception ex) {
                task.SetError(ex);
            }
        }

        protected virtual void ProcessResults(
            KeyedCollection<EncounterImage> images,
            EncounterImage image,
            TaskResult<string> serverOutput)
        {
            if (serverOutput == null || serverOutput.IsError())
                throw serverOutput.Exception;

            var serverImage = ImageDeserializer.Deserialize(serverOutput.Value);
            if (serverImage == null)
                throw new Exception("Could not parse server result.");

            image.Id = serverImage.Id;
            image.DateModified = serverImage.DateModified;
            if (image.Key != serverImage.Key)
                throw new Exception("Server assigned key doesn't match.");
        }
    }
}