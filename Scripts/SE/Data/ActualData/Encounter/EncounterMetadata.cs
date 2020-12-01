﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public interface INamed
    {
        Name Name { get; set; }
    }
    public interface IWebCompletion
    {
        string Url { get; set; }
        string CompletionCode { get; set; }
    }
    public enum Difficulty
    {
        Beginner, Intermediate, Advanced
    }

    [Serializable]
    public class EncounterMetadata
    {
        public virtual float Rating { get; set; } = -1;
        public virtual int RecordNumber { get; set; }
        public virtual string Filename { get; set; }
        public virtual Name AuthorName { get; set; }
        public virtual int AuthorAccountId { get; set; }
        public virtual string Title { get; set; }
        public virtual long DateModified { get; set; }
        public virtual string Subtitle { get; set; }
        public virtual string Description { get; set; }
        public virtual List<string> Categories { get; } = new List<string>();
        public virtual string Audience { get; set; }
        public virtual Difficulty Difficulty { get; set; }
        public virtual string EditorVersion { get; set; } = "0";
        public virtual bool IsTemplate { get; set; }
        public virtual bool IsPublic { get; set; }
        public virtual Sprite Sprite { get; set; }
        public virtual string GetDesiredFilename()
        {
            var desiredFilename = $"{RecordNumber}_{Title}";
            var filename = "";
            foreach (var ch in desiredFilename) {
                if (char.IsLetterOrDigit(ch) || ch == '-' || ch == '_')
                    filename += ch;
                else if (char.IsWhiteSpace(ch))
                    filename += '_';
            }

            return filename;
        }

        public EncounterMetadata() { }

        public EncounterMetadata(EncounterMetadata baseEncounterInfo)
        {
            Categories.AddRange(baseEncounterInfo.Categories);
            Subtitle = baseEncounterInfo.Subtitle;
            Description = baseEncounterInfo.Description;
            Audience = baseEncounterInfo.Audience;
            Difficulty = baseEncounterInfo.Difficulty;
            EditorVersion = baseEncounterInfo.EditorVersion;
        }

        public virtual string GetRecordNumberString() => RecordNumber.ToString("D6");

        private readonly DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        public virtual long ResetDateModified() => DateModified = (long)(DateTime.UtcNow - unixEpoch).TotalSeconds;
    }
}