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

        private const string PhpFile = "UploadEncounter.php";
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


        private const string AccountIdVariable = "accountId";
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

        private const string ModeVariable = "mode";
        private const string UploadModeValue = "upload";
        private const string UpdateModeValue = "update";
        private const string RecordNumberVariable = "recordNumber";
        private void AddFormModeFields(WWWForm form, EncounterMetadata metadata)
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

        private const string XmlMimeType = "text/xml";
        private const string NonImageContentVariable = "xmlData";
        private const string NonImageContentFilename = "xmlData";
        private void AddFormContentFields(WWWForm form, EncounterNonImageContent content)
        {
            var contentDoc = new XmlDocument();
            var contentSerializer = new XmlSerializer(contentDoc);
            EncounterContentSerializer.Serialize(contentSerializer, content);
            var fileBytes = GetFileAsByteArray(contentDoc.OuterXml);
            form.AddBinaryData(NonImageContentVariable, fileBytes, NonImageContentFilename, XmlMimeType);
        }
        private const string ImageContentVariable = "imgData";
        private const string ImageContentFilename = "imgData";
        private const int MaxAllowedPacketSize = 10000000;
        private void AddFormImageDataFields(WWWForm form, EncounterImageContent imageData)
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

        private const string FirstNameVariable = "firstName";
        private const string LastNameVariable = "lastName";
        private const string TitleVariable = "title";
        private const string DifficultyVariable = "difficulty";
        private const string SubtitleVariable = "subtitle";
        private const string DescriptionVariable = "description";
        private const string DateModifiedVariable = "modified";
        private const string AudienceVariable = "audience";
        private const string VersionVariable = "version";
        private const string VersionValue = "0.1";
        private const string UrlVariable = "url";
        private const string CompletionCodeVariable = "urlkey";
        private const string PublicVariable = "public";
        private const string TemplateVariable = "template";
        private const string SpriteDataVariable = "imageData";
        private const string SpriteWidthVariable = "imageWidth";
        private const string SpriteHeightVariable = "imageHeight";
        private void AddMetadataFields(WWWForm form, EncounterMetadata metadata)
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

        private void AddEscapedField(WWWForm form, string variable, string value)
            => form.AddField(variable, UnityWebRequest.EscapeURL(value));

        private const string TagsVariable = "tags";
        private void AddCategoryField(WWWForm form, IEnumerable<string> categories)
        {
            var categoryString = "";
            foreach (var category in categories)
                categoryString += UnityWebRequest.EscapeURL(category) + ";";
            form.AddField(TagsVariable, categoryString);
        }

        /**
         * Returns the passed in string as a byte array. Makes code easier to read
         */
        private byte[] GetFileAsByteArray(string data) => Encoding.UTF8.GetBytes(data);

        private void ProcessResults(WaitableTask actionResult, TaskResult<string> serverResult, EncounterMetadata metadata)
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