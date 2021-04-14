using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class EncounterMutableMetadata
    {
        public virtual float Rating { get; set; } = -1;
        public virtual int SaveId { get; set; }
        public virtual string Filename { get; set; }
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

        public EncounterMutableMetadata() { }

        public EncounterMutableMetadata(EncounterMutableMetadata baseMetadata)
        {
            Categories.AddRange(baseMetadata.Categories);
            Subtitle = baseMetadata.Subtitle;
            Description = baseMetadata.Description;
            Audience = baseMetadata.Audience;
            Difficulty = baseMetadata.Difficulty;
            EditorVersion = baseMetadata.EditorVersion;
        }

        private readonly DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        public virtual long ResetDateModified() => DateModified = (long)(DateTime.UtcNow - unixEpoch).TotalSeconds;
    }
}