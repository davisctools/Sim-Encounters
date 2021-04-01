using ClinicalTools.SimEncounters;
using SimpleJSON;

namespace ClinicalTools.Lift
{
    public class BasicEncounterMetadataDeserializer2 : IStringDeserializer<EncounterMetadata>, IJsonDeserializer<EncounterMetadata>
    {
        protected IJsonDeserializer<EncounterImage> ImageDeserializer { get; }
        protected IJsonDeserializer<Author> AuthorDeserializer { get; }
        protected IJsonDeserializer<Name> NameDeserializer { get; }
        public BasicEncounterMetadataDeserializer2(
            IJsonDeserializer<EncounterImage> imageDeserializer,
            IJsonDeserializer<Author> authorDeserializer,
            IJsonDeserializer<Name> nameDeserializer)
        {
            ImageDeserializer = imageDeserializer;
            AuthorDeserializer = authorDeserializer;
            NameDeserializer = nameDeserializer;
        }

        public virtual EncounterMetadata Deserialize(string text) => Deserialize(JSON.Parse(text));
        public virtual EncounterMetadata Deserialize(JSONNode node)
        {
            var metadata = new EncounterMetadata();
            AddValues(metadata, node);
            return metadata;
        }
        public virtual void AddValues(EncounterMetadata metadata, JSONNode node)
        {
            SetId(metadata, node);
            SetAuthor(metadata, node);
            SetDifficulty(metadata, node);
            SetSubtitle(metadata, node);
            SetDescription(metadata, node);
            SetCategories(metadata, node);
            SetModified(metadata, node);
            SetAudience(metadata, node);
            SetVersion(metadata, node);
            SetPublic(metadata, node);
            SetTemplate(metadata, node);
            SetImage(metadata, node);

            if (metadata is INamed named)
                SetName(named, node);
            else
                SetTitle(metadata, node);

            if (metadata is IWebCompletion webCompletion) {
                SetUrl(webCompletion, node);
                SetCompletionCode(webCompletion, node);
            }
        }

        public virtual void SetId(EncounterMetadata metadata, JSONNode node)
        {
            JSONNode id = node["id"];
            if (id != null) metadata.RecordNumber = id;
        }
        public virtual void SetAuthor(EncounterMetadata metadata, JSONNode node)
        {
            JSONNode author = node["author"];
            if (author != null) metadata.Author = AuthorDeserializer.Deserialize(author);
        }
        public virtual void SetTitle(EncounterMetadata metadata, JSONNode node)
        {
            JSONNode title = node["title"];
            if (title != null) metadata.Title = title;
        }
        public virtual void SetDifficulty(EncounterMetadata metadata, JSONNode node)
        {
            JSONNode difficulty = (string)node["difficulty"];
            if (difficulty == null)
                return;

            if (difficulty == "aaa")
                metadata.Difficulty = Difficulty.Beginner;
        }
        public virtual void SetSubtitle(EncounterMetadata metadata, JSONNode node)
        {
            JSONNode subtitle = node["subtitle"];
            if (subtitle != null) metadata.Subtitle = subtitle;
        }
        public virtual void SetDescription(EncounterMetadata metadata, JSONNode node)
        {
            JSONNode description = node["description"];
            if (description != null) metadata.Description = description;
        }
        public virtual void SetCategories(EncounterMetadata metadata, JSONNode node)
        {
            JSONNode tags = node["tags"];
            //if (tags != null) metadata.Categories = tags;
        }
        public virtual void SetModified(EncounterMetadata metadata, JSONNode node)
        {
            JSONNode modified = node["modified"];
            if (modified != null) metadata.DateModified = modified;
        }
        public virtual void SetAudience(EncounterMetadata metadata, JSONNode node)
        {
            JSONNode audience = node["audience"];
            if (audience != null) metadata.Audience = audience;
        }
        public virtual void SetVersion(EncounterMetadata metadata, JSONNode node)
        {
            JSONNode version = node["version"];
            if (version != null) metadata.EditorVersion = version;
        }
        public virtual void SetPublic(EncounterMetadata metadata, JSONNode node)
        {
            JSONNode isPublic = node["public"];
            if (isPublic != null) metadata.IsPublic = isPublic;
        }
        public virtual void SetTemplate(EncounterMetadata metadata, JSONNode node)
        {
            JSONNode isTemplate = node["template"];
            if (isTemplate != null) metadata.IsTemplate = isTemplate;
        }
        public virtual void SetImage(EncounterMetadata metadata, JSONNode node)
        {
            JSONNode image = node["image"];
            if (image != null) metadata.Image = ImageDeserializer.Deserialize(image);
        }
        public virtual void SetName(INamed named, JSONNode node)
        {
            JSONNode name = node["name"];
            if (name != null) named.Name = NameDeserializer.Deserialize(name);
        }
        public virtual void SetUrl(IWebCompletion webCompletion, JSONNode node)
        {
            JSONNode url = node["url"];
            if (url != null) webCompletion.Url = url;
        }
        public virtual void SetCompletionCode(IWebCompletion webCompletion, JSONNode node)
        {
            JSONNode completionCode = node["key"];
            if (completionCode != null) webCompletion.CompletionCode = completionCode;
        }
    }
}