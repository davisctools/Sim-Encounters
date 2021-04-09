using System.Collections;
using System.Collections.Generic;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public enum EncounterDataFileType
    {
        Data, Images, Metadata
    }

    public interface IFilenameInfo
    {
        string ImagesFolderName { get; }

        string GetFilename(EncounterDataFileType fileType);
    }
}