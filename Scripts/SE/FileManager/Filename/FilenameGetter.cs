﻿namespace ClinicalTools.SimEncounters
{
    public class FilenameGetter : IFilenameGetter
    {
        private readonly IFileExtensionGetter extensionGetter;
        public FilenameGetter(IFileExtensionGetter extensionGetter) 
            => this.extensionGetter = extensionGetter;

        public string GetFilename(FileType fileType, EncounterMetadata metadata)
        {
            var filenameWithoutExtension = GetFilenameWithoutExtension(fileType, metadata);
            var extension = extensionGetter.GetExtension(fileType);

            return $"{filenameWithoutExtension}.{extension}";
        }

        public string GetFilenameWithoutExtension(FileType fileType, EncounterMetadata metadata)
        {
            if (fileType == FileType.BasicStatus || fileType == FileType.DetailedStatus)
                return metadata.RecordNumber.ToString();
            else
                return metadata.Filename;
        }
    }
}