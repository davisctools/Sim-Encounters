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
        private const char CaseInfoDivider = '|';
        private const char CategoryDivider = ';';
        private const int EncounterParts = 14;
        private const int RecordNumberIndex = 0;
        private const int AuthorAccountIdIndex = 1;
        private const int AuthorNameIndex = 2;
        private const int TitleIndex = 3;
        private const int DifficultyIndex = 4;
        private const int SubtitleIndex = 5;
        private const int DescriptionIndex = 6;
        private const int TagsIndex = 7;
        private const int ModifiedIndex = 8;
        private const int AudienceIndex = 9;
        private const int VersionIndex = 10;
        private const int IsPublicIndex = 11;
        private const int IsTemplateIndex = 12;
        private const int RatingIndex = 13;
        private const int ImageWidthIndex = 14;
        private const int ImageHeightIndex = 15;
        private const int ImageDataIndex = 16;

        public EncounterMetadata Deserialize(string text)
        {
            try {
                var parsedItem = text.Split(CaseInfoDivider);
                if (parsedItem == null || parsedItem.Length < EncounterParts)
                    return null;

                var metadata = new EncounterMetadata() {
                    RecordNumber = int.Parse(parsedItem[RecordNumberIndex]),
                    Author = new Author(int.Parse(parsedItem[AuthorAccountIdIndex])) {
                        Name = GetName(parsedItem[AuthorNameIndex])
                    },
                    Title = parsedItem[TitleIndex],
                    Difficulty = GetDifficulty(parsedItem[DifficultyIndex]),
                    Subtitle = UnityWebRequest.UnEscapeURL(parsedItem[SubtitleIndex]),
                    Description = UnityWebRequest.UnEscapeURL(parsedItem[DescriptionIndex]),
                    DateModified = long.Parse(parsedItem[ModifiedIndex]),
                    Audience = UnityWebRequest.UnEscapeURL(parsedItem[AudienceIndex]),
                    EditorVersion = UnityWebRequest.UnEscapeURL(parsedItem[VersionIndex]),
                    IsPublic = GetBoolValue(parsedItem[IsPublicIndex]),
                    IsTemplate = GetBoolValue(parsedItem[IsTemplateIndex]),
                    Rating = float.Parse(parsedItem[RatingIndex])
                };
                if (float.TryParse(parsedItem[RatingIndex], out var rating))
                    metadata.Rating = rating;
                var categories = parsedItem[TagsIndex].Split(CategoryDivider);
                if (categories.Length == 1)
                    categories = parsedItem[TagsIndex].Split(',');
                foreach (var category in categories)
                    metadata.Categories.Add(UnityWebRequest.UnEscapeURL(category).Trim());

                metadata.Filename = metadata.GetDesiredFilename();

                if (parsedItem.Length + 1 >= ImageDataIndex) {
                    metadata.Image = new EncounterImage() {
                        Sprite = GetSprite(parsedItem[ImageWidthIndex], parsedItem[ImageHeightIndex],
                            UnityWebRequest.UnEscapeURL(parsedItem[ImageDataIndex]))
                    };
                }

                return metadata;
            } catch (Exception) {
                return null;
            }
        }

        protected Name GetName(string name)
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

        protected Difficulty GetDifficulty(string difficulty)
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

        protected bool GetBoolValue(string value) => value == "1";

        protected Sprite GetSprite(string widthText, string heightText, string spriteText)
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