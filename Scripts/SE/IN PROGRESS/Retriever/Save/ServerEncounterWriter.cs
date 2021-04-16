using ClinicalTools.Collections;
using ClinicalTools.SimEncounters.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public enum SaveVersion { AutoSave, Public, Private }
    public class SaveEncounterParameters
    {
        public User User { get; set; }
        public ContentEncounter Encounter { get; set; }
        public SaveVersion SaveVersion { get; set; } = SaveVersion.Private;
        public string Description { get; set; }

        public string GetDescription()
        {
            if (Description != null)
                return Description;

            if (Encounter.Metadata.RecordNumber <= 0)
                return "Encounter created";

            switch (SaveVersion) {
                case SaveVersion.AutoSave:
                    return "Autosave";
                case SaveVersion.Public:
                    return "Update to public";
                case SaveVersion.Private:
                    return "Encounter update";
                default:
                    return "";
            }
        }
    }

    public class ServerEncounterWriter : IEncounterWriter
    {
        protected IServerStringReader ServerReader { get; }
        protected IUrlBuilder UrlBuilder { get; }
        protected IStringSerializer<Sprite> SpriteSerializer { get; }
        protected IObjectSerializer<LegacyEncounterImageContent> ImageDataSerializer { get; }
        protected IObjectSerializer<EncounterContentData> EncounterContentSerializer { get; }
        public ServerEncounterWriter(
            IServerStringReader serverReader,
            IUrlBuilder urlBuilder,
            IStringSerializer<Sprite> spriteSerializer,
            IObjectSerializer<LegacyEncounterImageContent> imageDataSerializer,
            IObjectSerializer<EncounterContentData> encounterContentSerializer)
        {
            ServerReader = serverReader;
            UrlBuilder = urlBuilder;
            SpriteSerializer = spriteSerializer;
            ImageDataSerializer = imageDataSerializer;
            EncounterContentSerializer = encounterContentSerializer;
        }

        protected virtual string PhpFile { get; } = "Main.php";
        public WaitableTask Save(SaveEncounterParameters parameters)
        {
            if (parameters.User.IsGuest)
                return WaitableTask.CompletedTask;

            var url = UrlBuilder.BuildUrl(PhpFile);
            var form = CreateForm(parameters);

            var webRequest = UnityWebRequest.Post(url, form);
            var serverResults = ServerReader.Begin(webRequest);

            var result = new WaitableTask();
            serverResults.AddOnCompletedListener((serverResult) => ProcessResults(result, serverResult, parameters.Encounter.Metadata));
            return result;
        }


        protected virtual string AccountVariable { get; } = "account";
        protected virtual WWWForm CreateForm(SaveEncounterParameters parameters)
        {
            var form = new WWWForm();

            form.AddField(AccountVariable, parameters.User.AccountId);
            AddFormModeFields(form, parameters);
            AddFormContentFields(form, parameters.Encounter.Content);
            AddMetadataFields(form, parameters.Encounter.Metadata);
            AddImagesField(form, parameters.Encounter.Content.Images);

            return form;
        }

        protected virtual string ModeVariable { get; } = "mode";
        protected virtual string ModeValue { get; } = "encounter";
        protected virtual string ActionVariable { get; } = "action";
        protected virtual string ActionValue { get; } = "save";
        protected virtual string SubactionVariable { get; } = "subaction";
        protected virtual string SubactionUploadValue { get; } = "new";
        protected virtual string SubactionUpdateValue { get; } = "full";
        protected virtual string EncounterVariable { get; } = "encounter";
        protected virtual string VersionVariable { get; } = "version";
        protected virtual string VersionPublicValue { get; } = "public";
        protected virtual string VersionAutosaveValue { get; } = "public";
        protected virtual string ClientVersionVariable { get; } = "client_version";
        protected virtual string ClientVersionValue { get; } = "0.1.0";
        protected virtual string SaveDescriptionVariable { get; } = "save_description";
        protected virtual void AddFormModeFields(WWWForm form, SaveEncounterParameters parameters)
        {
            string subaction;
            var recordNumber = parameters.Encounter.Metadata.RecordNumber;
            if (recordNumber > 0) {
                form.AddField(EncounterVariable, recordNumber);
                subaction = SubactionUpdateValue;
                switch (parameters.SaveVersion) {
                    case SaveVersion.AutoSave:
                        form.AddField(VersionVariable, VersionAutosaveValue);
                        break;
                    case SaveVersion.Public:
                        form.AddField(VersionVariable, VersionPublicValue);
                        break;
                }
            } else {
                subaction = SubactionUploadValue;
            }

            form.AddField(ModeVariable, ModeValue);
            form.AddField(ActionVariable, ActionValue);
            form.AddField(SubactionVariable, subaction);
            form.AddField(ClientVersionVariable, ClientVersionValue);
            form.AddField(SaveDescriptionVariable, parameters.GetDescription());
        }

        protected virtual string XmlMimeType { get; } = "text/xml";
        protected virtual string ContentVariable { get; } = "file";
        protected virtual string ContentFilename { get; } = "data.ced";
        protected virtual void AddFormContentFields(WWWForm form, EncounterContentData content)
        {
            var contentDoc = new XmlDocument();
            var contentSerializer = new XmlSerializer(contentDoc);
            EncounterContentSerializer.Serialize(contentSerializer, content);
            var fileBytes = GetFileAsByteArray(contentDoc.OuterXml);
            form.AddBinaryData(ContentVariable, fileBytes, ContentFilename, XmlMimeType);
        }


        protected virtual string FirstNameVariable { get; } = "first_name";
        protected virtual string LastNameVariable { get; } = "last_name";
        protected virtual string TitleVariable { get; } = "title";
        protected virtual string SubtitleVariable { get; } = "subtitle";
        protected virtual string DescriptionVariable { get; } = "description";
        protected virtual string AudienceVariable { get; } = "audience";
        protected virtual string DifficultyVariable { get; } = "difficulty";
        protected virtual string TemplateVariable { get; } = "template";
        protected virtual string ImageVariable { get; } = "image";
        protected virtual string UrlVariable { get; } = "url";
        protected virtual string CompletionCodeVariable { get; } = "url_key";
        protected virtual void AddMetadataFields(WWWForm form, OldEncounterMetadata metadata)
        {
            if (metadata is INamed named) {
                AddField(form, FirstNameVariable, named.Name.FirstName);
                AddField(form, LastNameVariable, named.Name.LastName);
            } else {
                AddField(form, TitleVariable, metadata.Title);
            }

            AddField(form, SubtitleVariable, metadata.Subtitle);
            AddField(form, DescriptionVariable, metadata.Description);

            AddCategoryField(form, metadata.Categories);

            AddField(form, AudienceVariable, metadata.Audience);
            AddField(form, DifficultyVariable, metadata.Difficulty.ToString());

            form.AddField(TemplateVariable, metadata.IsTemplate);

            if (metadata.Image != null)
                form.AddField(ImageVariable, metadata.Image.Id);

            if (metadata is IWebCompletion webCompletion) {
                AddField(form, UrlVariable, webCompletion.Url);
                AddField(form, CompletionCodeVariable, webCompletion.CompletionCode);
            }
        }

        protected virtual void AddField(WWWForm form, string variable, string value)
        {
            if (value != null) form.AddField(variable, value);
        }

        protected virtual string ImagesVariable { get; } = "images";
        protected virtual void AddImagesField(WWWForm form, KeyedCollection<EncounterImage> images)
        {
            if (images.Count == 0)
                return;

            var imagesStr = "";
            foreach (var image in images.Values) {
                if (imagesStr.Length > 0)
                    imagesStr += ',';
                imagesStr += image.Id;
            }
            form.AddField(ImagesVariable, imagesStr);
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

        protected virtual void ProcessResults(WaitableTask actionResult, TaskResult<string> serverResult, OldEncounterMetadata metadata)
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
            if (metadata.RecordNumber <= 0 && int.TryParse(splitStr[0], out var recordNumber))
                metadata.RecordNumber = recordNumber;

            actionResult.SetCompleted();
        }
    }
}