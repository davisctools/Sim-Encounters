using System;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class EncounterMetadataDeserializer : IStringDeserializer<EncounterMetadata>
    {
        private readonly ISpriteDeserializer spriteDeserializer;
        public EncounterMetadataDeserializer(ISpriteDeserializer spriteDeserializer)
            => this.spriteDeserializer = spriteDeserializer;

        // Ideally I'd use JSON objects, and I've set the PHP to allow them.
        // Unfortunately, it would drastically increase the size of the menu information the server gives
        // It would also cause more work to support it, so for now, I'll stick with more simplistic storage methods
        protected virtual char CaseInfoDivider { get; } = '|';
        protected virtual char CategoryDivider { get; } = ';';
        protected virtual int EncounterParts { get; } = 14;

        public virtual EncounterMetadata Deserialize(string text)
        {
            try {
                var parsedItem = text.Split(CaseInfoDivider);
                if (parsedItem == null || parsedItem.Length < EncounterParts)
                    return null;

                int index = 0;
                var metadata = CreateMetadata();
                metadata.RecordNumber = int.Parse(parsedItem[index++]);
                metadata.AuthorAccountId = int.Parse(parsedItem[index++]);
                metadata.AuthorName = GetName(parsedItem[index++]);

                if (metadata is INamed named)
                    named.Name = GetName(parsedItem[index++]);
                else
                    metadata.Title = parsedItem[index++];

                metadata.Difficulty = GetDifficulty(parsedItem[index++]);
                metadata.Subtitle = UnityWebRequest.UnEscapeURL(parsedItem[index++]);
                metadata.Description = UnityWebRequest.UnEscapeURL(parsedItem[index++]);

                AddCategories(metadata, parsedItem[index++]);

                metadata.DateModified = long.Parse(parsedItem[index++]);
                metadata.Audience = UnityWebRequest.UnEscapeURL(parsedItem[index++]);
                metadata.EditorVersion = UnityWebRequest.UnEscapeURL(parsedItem[index++]);
                metadata.IsPublic = GetBoolValue(parsedItem[index++]);
                metadata.IsTemplate = GetBoolValue(parsedItem[index++]);

                if (float.TryParse(parsedItem[index++], out var rating))
                    metadata.Rating = rating;


                metadata.Filename = metadata.GetDesiredFilename();


                if (metadata is IWebCompletion webCompletion) {
                    webCompletion.Url = UnityWebRequest.UnEscapeURL(parsedItem[index++].Trim());
                    webCompletion.CompletionCode = UnityWebRequest.UnEscapeURL(parsedItem[index++].Trim());
                }

                // 3 is the number of image related fields
                if (index + 3 > parsedItem.Length)
                    return metadata;

                var spriteWidthStr = parsedItem[index++];
                var spriteHeightStr = parsedItem[index++];
                var spriteDataStr = parsedItem[index++];
                if (!string.IsNullOrWhiteSpace(spriteWidthStr)) {
                    metadata.Sprite = GetSprite(spriteWidthStr, spriteHeightStr,
                        UnityWebRequest.UnEscapeURL(spriteDataStr));
                    ImageHolder.HoldImage(metadata.Sprite);
                }

                if (index >= parsedItem.Length)
                    return metadata;

                metadata.GrantInfo = parsedItem[index++];

                return metadata;
            } catch (Exception) {
                return null;
            }
        }

        protected virtual void AddCategories(EncounterMetadata metadata, string categories)
        {
            var categoriesArr = categories.Split(CategoryDivider);
            if (categoriesArr.Length == 1)
                categoriesArr = categories.Split(',');
            foreach (var category in categoriesArr)
                metadata.Categories.Add(UnityWebRequest.UnEscapeURL(category).Trim());
        }

        protected virtual EncounterMetadata CreateMetadata() => new EncounterMetadata();

        protected virtual Name GetName(string name)
        {
            var nameParts = name.Split(CategoryDivider);
            switch (nameParts.Length) {
                case 0:
                    return new Name();
                case 1:
                    return new Name(UnityWebRequest.UnEscapeURL(nameParts[0]));
                case 2:
                    return new Name(UnityWebRequest.UnEscapeURL(nameParts[0]),
                                    UnityWebRequest.UnEscapeURL(nameParts[1]));
                default:
                    return new Name(UnityWebRequest.UnEscapeURL(nameParts[0]),
                                    UnityWebRequest.UnEscapeURL(nameParts[1]),
                                    UnityWebRequest.UnEscapeURL(nameParts[2]));
            }
        }

        protected virtual Difficulty GetDifficulty(string difficulty)
        {
            difficulty = UnityWebRequest.UnEscapeURL(difficulty);

            if (difficulty.Equals("intermediate", StringComparison.InvariantCultureIgnoreCase))
                return Difficulty.Intermediate;
            else if (difficulty.Equals("beginner", StringComparison.InvariantCultureIgnoreCase))
                return Difficulty.Beginner;
            else if (difficulty.Equals("advanced", StringComparison.InvariantCultureIgnoreCase))
                return Difficulty.Advanced;
            return Difficulty.Beginner;
        }

        protected virtual bool GetBoolValue(string value) => value == "1";

        protected virtual Sprite GetSprite(string widthText, string heightText, string spriteText)
        {
            if (!int.TryParse(widthText, out var width) || !int.TryParse(heightText, out var height))
                return null;

            try {
                return spriteDeserializer.Deserialize(width, height, spriteText);
            } catch (Exception) {
                return null;
            }
        }
    }
}