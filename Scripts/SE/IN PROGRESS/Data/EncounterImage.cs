using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class EncounterImage
    {
        public long DateModified { get; set; } = 0;
        public int Id { get; set; } = 0;
        public string Key { get; set; }
        public string Filename { get; set; }

        public Sprite Sprite { get; set; }
        public event Action<EncounterImage> Updated;
         
        public EncounterImage() { }
        public EncounterImage(string key) => Key = key;

        public virtual void SetUpdated(EncounterImage image)
        {
            if (image.Key == image.Key)
                Updated?.Invoke(image);
        }
    }
}