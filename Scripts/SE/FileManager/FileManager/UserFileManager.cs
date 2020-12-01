using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class UserFileManager : IFileManager
    {
        protected virtual string LocalSavesPath => Path.Combine(Application.persistentDataPath, "LocalSaves");

        private readonly IFilenameGetter filenameGetter;
        private readonly IFileExtensionGetter fileExtensionGetter;
        public UserFileManager(IFilenameGetter filenameGetter, IFileExtensionGetter fileExtensionGetter)
        {
            this.filenameGetter = filenameGetter;
            this.fileExtensionGetter = fileExtensionGetter;
        }

        public void SetFileText(User user, FileType fileType, EncounterMetadata metadata, string contents)
        {
            var filepath = GetFilepath(user, fileType, metadata);
            var directory = Path.GetDirectoryName(filepath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllText(filepath, contents);
        }
        public WaitableTask<string> GetFileText(User user, FileType fileType, EncounterMetadata metadata)
        {
            var filepath = GetFilepath(user, fileType, metadata);
            if (!File.Exists(filepath))
                return new WaitableTask<string>(new Exception("File doesn't exist"));
            var text = File.ReadAllText(filepath);
            return new WaitableTask<string>(text);
        }

        public WaitableTask<string[]> GetFilesText(User user, FileType fileType)
        {
            var filepaths = GetFilepaths(user, fileType);
            var texts = new string[filepaths.Length];
            for (int i = 0; i < filepaths.Length; i++)
                texts[i] = File.ReadAllText(filepaths[i]);

            return new WaitableTask<string[]>(texts);
        }

        protected string GetFilepath(User user, FileType fileType, EncounterMetadata metadata)
        {
            var folder = GetFolder(user, fileType);
            var filename = filenameGetter.GetFilename(fileType, metadata);
            return Path.Combine(folder, filename);
        }

        protected string[] GetFilepaths(User user, FileType fileType)
        {
            var folder = GetFolder(user, fileType);
            if (!Directory.Exists(folder))
                return new string[0];

            var extension = fileExtensionGetter.GetExtension(fileType);
            var filepaths = Directory.GetFiles(folder, $"*.{extension}");
            filepaths = filepaths.Where((path) => path.EndsWith(extension, StringComparison.InvariantCultureIgnoreCase)).ToArray();

            return filepaths;
        }

        public void UpdateFilename(User user, EncounterMetadata metadata)
        {
            string oldFilePrefix = metadata.Filename;
            string newFilePrefix = metadata.GetDesiredFilename();
            if (newFilePrefix == oldFilePrefix)
                return;

            metadata.Filename = newFilePrefix;

            var folder = GetFolder(user, FileType.Data);
            if (!Directory.Exists(folder))
                return;
            var files = Directory.GetFiles(folder, $"{oldFilePrefix}*");
            foreach (var file in files) {
                // a better replacement should be used to prevent extra things from replaced
                // on Windows, this would only cause an error if the user folder shared a name with the file
                File.Move(file, file.Replace(oldFilePrefix, newFilePrefix));
            }
        }

        public void DeleteFiles(User user, EncounterMetadata metadata)
        {
            var folder = GetFolder(user, FileType.Data);
            if (!Directory.Exists(folder))
                return;
            var files = Directory.GetFiles(folder, $"{metadata.Filename}*");
            foreach (var file in files) {
                foreach (FileType fileType in Enum.GetValues(typeof(FileType))) {
                    if (!file.EndsWith($".{fileExtensionGetter.GetExtension(fileType)}",
                        StringComparison.InvariantCultureIgnoreCase)) {
                        continue;
                    }

                    File.Delete(file);
                    break;
                }
            }
        }

        public string GetFolder(User user, FileType fileType)
        {
            string userFolder = GetUserFolder(user);
            string subfolder = GetSubfolder(fileType);
            return Path.Combine(userFolder, subfolder);
        }

        ///<summary>
        ///Returns a truncated md5 hash to represent unique folders for users. This returns only the folder
        ///</summary>
        ///<param name="accountId">User account id</param>
        protected string GetUserFolder(User user)
        {
            string accountStr;
            using (MD5 md5 = MD5.Create()) {
                byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(user.AccountId.ToString()));
                StringBuilder sb = new StringBuilder();
                foreach (var b in bytes)
                    sb.Append(b.ToString("x2"));

                accountStr = sb.ToString().Substring(7, 10); //Return a random 10 digit substring of the hash to represent the folder name
            }

            var path = Path.Combine(LocalSavesPath, accountStr);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }

        protected virtual string StatusSubfolder { get; } = "statuses";
        protected virtual string ContentSubfolder { get; } = "encounters";
        public string GetSubfolder(FileType fileType)
        {
            if (fileType == FileType.BasicStatus || fileType == FileType.DetailedStatus)
                return StatusSubfolder;
            else
                return ContentSubfolder;

        }
    }
}