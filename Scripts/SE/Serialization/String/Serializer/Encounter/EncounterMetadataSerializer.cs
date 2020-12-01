﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class EncounterMetadataSerializer : IStringSerializer<EncounterMetadata>
    {
        public IStringSerializer<Sprite> SpriteSerializer { get; set; } = new SpriteSerializer();

        public virtual string Serialize(EncounterMetadata metadata)
        {
            var str = metadata.RecordNumber.ToString();
            str += AppendValue(metadata.AuthorAccountId.ToString());
            str += AppendValue(metadata.AuthorName);

            if (metadata is INamed named)
                str += AppendValue(named.Name);
            else
                str += AppendValue(metadata.Title);

            str += AppendValue(metadata.Difficulty.ToString());
            str += AppendValue(metadata.Subtitle);
            str += AppendValue(metadata.Description);
            str += AppendValue(metadata.Categories);
            str += AppendValue(metadata.DateModified.ToString());
            str += AppendValue(metadata.Audience);
            str += AppendValue(metadata.EditorVersion);
            str += AppendValue(metadata.IsPublic);
            str += AppendValue(metadata.IsTemplate);
            str += AppendValue(metadata.Rating.ToString());
            if (metadata is IWebCompletion webCompletion) {
                str += AppendValue(webCompletion.Url);
                str += AppendValue(webCompletion.CompletionCode);
            }
            if (metadata.Sprite != null) {
                str += AppendValue(metadata.Sprite.texture.width.ToString());
                str += AppendValue(metadata.Sprite.texture.height.ToString());
                str += AppendValue(SpriteSerializer.Serialize(metadata.Sprite));
            }

            return str;
        }

        private const string CaseInfoDivider = "|";
        private const string CategoryDivider = ";";
        protected virtual string AppendValue(bool value) => CaseInfoDivider + (value ? "1" : "0");
        protected virtual string AppendValue(string value) => CaseInfoDivider + UnityWebRequest.EscapeURL(value);
        protected virtual string AppendValue(IEnumerable<string> values)
            => CaseInfoDivider + string.Join(CategoryDivider, values);
        protected virtual string AppendValue(Name name) => $"{CaseInfoDivider}" +
            $"{UnityWebRequest.EscapeURL(name.Honorific)}{CategoryDivider}" +
            $"{UnityWebRequest.EscapeURL(name.FirstName)}{CategoryDivider}" +
            $"{UnityWebRequest.EscapeURL(name.LastName)}";
    }
}