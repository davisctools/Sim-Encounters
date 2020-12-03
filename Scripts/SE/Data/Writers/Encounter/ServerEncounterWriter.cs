using ClinicalTools.SimEncounters.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class ServerEncounterWriter : IEncounterWriter
    {
        protected IServerReader ServerReader { get; }
        protected IUrlBuilder UrlBuilder { get; }
        protected IStringSerializer<Sprite> SpriteSerializer { get; }
        protected IXmlSerializer<EncounterImageContent> ImageDataSerializer { get; }
        protected IXmlSerializer<EncounterNonImageContent> EncounterContentSerializer { get; }
        public ServerEncounterWriter(
            IServerReader serverReader,
            IUrlBuilder urlBuilder,
            IStringSerializer<Sprite> spriteSerializer,
            IXmlSerializer<EncounterImageContent> imageDataSerializer,
            IXmlSerializer<EncounterNonImageContent> encounterContentSerializer)
        {
            ServerReader = serverReader;
            UrlBuilder = urlBuilder;
            SpriteSerializer = spriteSerializer;
            ImageDataSerializer = imageDataSerializer;
            EncounterContentSerializer = encounterContentSerializer;
        }

        protected virtual string PhpFile { get; } = "UploadEncounter.php";
        public WaitableTask Save(User user, Encounter encounter)
        {
            if (user.IsGuest)
                return WaitableTask.CompletedTask;

            var url = UrlBuilder.BuildUrl(PhpFile);
            var form = CreateForm(user, encounter);

            var webRequest = UnityWebRequest.Post(url, form);
            var serverResults = ServerReader.Begin(webRequest);

            var result = new WaitableTask();
            serverResults.AddOnCompletedListener((serverResult) => ProcessResults(result, serverResult, encounter.Metadata));
            return result;
        }


        protected virtual string AccountIdVariable { get; } = "accountId";
        protected virtual WWWForm CreateForm(User user, Encounter encounter)
        {
            var form = new WWWForm();

            form.AddField(AccountIdVariable, user.AccountId);
            AddFormModeFields(form, encounter.Metadata);
            AddFormContentFields(form, encounter.Content.NonImageContent);
            AddFormImageDataFields(form, encounter.Content.ImageContent);
            AddMetadataFields(form, encounter.Metadata);

            return form;
        }

        protected virtual string ModeVariable { get; } = "mode";
        protected virtual string UploadModeValue { get; } = "upload";
        protected virtual string UpdateModeValue { get; } = "update";
        protected virtual string RecordNumberVariable { get; } = "recordNumber";
        protected virtual void AddFormModeFields(WWWForm form, EncounterMetadata metadata)
        {
            string mode;
            if (metadata.RecordNumber >= 0 && metadata.RecordNumber < 1000) {
                form.AddField(RecordNumberVariable, metadata.RecordNumber);
                mode = UpdateModeValue;
            } else {
                mode = UploadModeValue;
            }

            form.AddField(ModeVariable, mode);
        }

        protected virtual string XmlMimeType { get; } = "text/xml";
        protected virtual string NonImageContentVariable { get; } = "xmlData";
        protected virtual string NonImageContentFilename { get; } = "xmlData";
        protected virtual void AddFormContentFields(WWWForm form, EncounterNonImageContent content)
        {
            var contentDoc = new XmlDocument();
            var contentSerializer = new XmlSerializer(contentDoc);
            EncounterContentSerializer.Serialize(contentSerializer, content);
            var fileBytes = GetFileAsByteArray(contentDoc.OuterXml);
            form.AddBinaryData(NonImageContentVariable, fileBytes, NonImageContentFilename, XmlMimeType);
        }
        protected virtual string ImageContentVariable { get; } = "imgData";
        protected virtual string ImageContentFilename { get; } = "imgData";
        protected virtual int MaxAllowedPacketSize { get; } = 10000000;
        protected virtual void AddFormImageDataFields(WWWForm form, EncounterImageContent imageData)
        {
            var imagesDoc = new XmlDocument();
            var imagesSerializer = new XmlSerializer(imagesDoc);
            ImageDataSerializer.Serialize(imagesSerializer, imageData);
            byte[] fileBytesImg = GetFileAsByteArray(imagesDoc.OuterXml);
            Debug.Log("Image file size (in bytes): " + fileBytesImg.Length);
            // Eventually an approach to get around the size limit may be needed
            if (fileBytesImg.Length <= MaxAllowedPacketSize)
                form.AddBinaryData(ImageContentVariable, fileBytesImg, ImageContentFilename, XmlMimeType);
            else
                Debug.LogError("Error: Images exceed upload size limit");
        }

        protected virtual string FirstNameVariable { get; } = "firstName";
        protected virtual string LastNameVariable { get; } = "lastName";
        protected virtual string TitleVariable { get; } = "title";
        protected virtual string DifficultyVariable { get; } = "difficulty";
        protected virtual string SubtitleVariable { get; } = "subtitle";
        protected virtual string DescriptionVariable { get; } = "description";
        protected virtual string DateModifiedVariable { get; } = "modified";
        protected virtual string AudienceVariable { get; } = "audience";
        protected virtual string VersionVariable { get; } = "version";
        protected virtual string VersionValue { get; } = "0.1";
        protected virtual string UrlVariable { get; } = "url";
        protected virtual string CompletionCodeVariable { get; } = "urlkey";
        protected virtual string PublicVariable { get; } = "public";
        protected virtual string TemplateVariable { get; } = "template";
        protected virtual string SpriteDataVariable { get; } = "imageData";
        protected virtual string SpriteWidthVariable { get; } = "imageWidth";
        protected virtual string SpriteHeightVariable { get; } = "imageHeight";
        protected virtual void AddMetadataFields(WWWForm form, EncounterMetadata metadata)
        {
            if (metadata is INamed named) {
                AddEscapedField(form, FirstNameVariable, named.Name.FirstName);
                AddEscapedField(form, LastNameVariable, named.Name.LastName);
            } else {
                AddEscapedField(form, TitleVariable, metadata.Title);
            }

            AddEscapedField(form, DifficultyVariable, metadata.Difficulty.ToString());
            AddEscapedField(form, SubtitleVariable, metadata.Subtitle);
            AddEscapedField(form, DescriptionVariable, metadata.Description);
            AddCategoryField(form, metadata.Categories);
            metadata.ResetDateModified();
            form.AddField(DateModifiedVariable, metadata.DateModified.ToString());
            AddEscapedField(form, AudienceVariable, metadata.Audience);
            AddEscapedField(form, VersionVariable, VersionValue);
            
            if (metadata is IWebCompletion webCompletion) {
                AddEscapedField(form, UrlVariable, webCompletion.Url);
                AddEscapedField(form, CompletionCodeVariable, webCompletion.CompletionCode);
            }
            
            form.AddField(PublicVariable, metadata.IsPublic);
            form.AddField(TemplateVariable, metadata.IsTemplate);
            if (metadata.Sprite == null)
                return;

            AddEscapedField(form, SpriteDataVariable, SpriteSerializer.Serialize(metadata.Sprite));
            form.AddField(SpriteWidthVariable, metadata.Sprite.texture.width);
            form.AddField(SpriteHeightVariable, metadata.Sprite.texture.height);
        }

        protected virtual void AddEscapedField(WWWForm form, string variable, string value)
        {
            if (value == null)
                value = "";
            form.AddField(variable, UnityWebRequest.EscapeURL(value));
        }

        protected virtual string TagsVariable { get; } = "tags";
        protected virtual void AddCategoryField(WWWForm form, IEnumerable<string> categories)
        {
            var categoryString = "";
            foreach (var category in categories)
                categoryString += UnityWebRequest.EscapeURL(category) + ";";
            form.AddField(TagsVariable, categoryString);
        }

        /**
         * Returns the passed in string as a byte array. Makes code easier to read
         */
        protected virtual byte[] GetFileAsByteArray(string data) => Encoding.UTF8.GetBytes(data);

        protected virtual void ProcessResults(WaitableTask actionResult, TaskResult<string> serverResult, EncounterMetadata metadata)
        {
            if (serverResult.IsError()) {
                actionResult.SetError(serverResult.Exception);
                return;
            }

            Debug.Log("Returned text from PHP: \n" + serverResult.Value);
            if (string.IsNullOrWhiteSpace(serverResult.Value)) {
                actionResult.SetError(new Exception("No text returned from the server."));
                return;
            }

            var splitStr = serverResult.Value.Split('|');
            if (int.TryParse(splitStr[0], out var recordNumber))
                metadata.RecordNumber = recordNumber;

            actionResult.SetCompleted();
        }
    }
}