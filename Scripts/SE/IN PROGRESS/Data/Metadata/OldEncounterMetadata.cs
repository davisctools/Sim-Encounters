using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class OldEncounterMetadata
    {

        public virtual float Rating { get; set; } = -1;
        public virtual int RecordNumber { get; set; }
        public virtual string Filename { get; set; }
        public virtual Author Author { get; set; }
        public virtual string Title { get; set; }
        public virtual long DateModified { get; set; }
        public virtual string Subtitle { get; set; }
        public virtual string Description { get; set; }
        public virtual List<string> Categories { get; } = new List<string>();
        public virtual string Audience { get; set; }
        public virtual EncounterDifficulty Difficulty { get; set; }
        public virtual string EditorVersion { get; set; } = "0";
        public virtual bool IsTemplate { get; set; }
        public virtual bool IsPublic { get; set; }
        public virtual EncounterImage Image { get; set; }
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

        public OldEncounterMetadata() { }

        public OldEncounterMetadata(OldEncounterMetadata baseEncounterInfo)
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