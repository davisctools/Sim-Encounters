using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class UserFileManager2 : IEncounterFileWriter, IEncounterFileReader
    {
        protected virtual string LocalSavesPath => Path.Combine(Application.persistentDataPath, "LocalSaves");

        private readonly IFilenameInfo filenameInfo;
        public UserFileManager2(IFilenameInfo filenameInfo)
        {
            this.filenameInfo = filenameInfo;
        }

        public virtual void WriteTextFile(User user, EncounterMetadata metadata, EncounterDataFileType fileType, string contents)
        {
            var filepath = GetFilepath(user, metadata, fileType);
            var directory = Path.GetDirectoryName(filepath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllText(filepath, contents);
        }

        public virtual WaitableTask<string> ReadTextFile(User user, EncounterMetadata metadata, EncounterDataFileType fileType)
        {
            var filepath = GetFilepath(user, metadata, fileType);
            if (!File.Exists(filepath))
                return new WaitableTask<string>(new Exception("File doesn't exist"));
            var text = File.ReadAllText(filepath);
            return new WaitableTask<string>(text);
        }

        public virtual WaitableTask<string[]> ReadTextFiles(User user, EncounterDataFileType fileType)
        {
            var filepaths = GetFilepaths(user, fileType);
            var texts = new string[filepaths.Length];
            for (int i = 0; i < filepaths.Length; i++)
                texts[i] = File.ReadAllText(filepaths[i]);

            return new WaitableTask<string[]>(texts);
        }

        protected virtual string GetFilepath(User user, EncounterMetadata metadata, EncounterDataFileType fileType)
        {
            var folder = GetSaveFolder(user, metadata);
            var filename = filenameInfo.GetFilename(fileType);
            return Path.Combine(folder, filename);
        }
        protected virtual string GetFilepath(string encounterDirectory, EncounterDataFileType fileType)
        {
            var folder = GetSaveFolder(encounterDirectory);
            var filename = filenameInfo.GetFilename(fileType);
            return Path.Combine(folder, filename);
        }

        protected virtual string[] GetFilepaths(User user, EncounterDataFileType fileType)
        {
            var paths = GetEncounterDirectories(user);
            for (int i = 0; i < paths.Length; i++)
                paths[i] = GetFilepath(paths[i], fileType);

            return paths;
        }
        protected virtual string[] GetEncounterDirectories(User user)
        {
            var folder = GetEncountersFolder(user);
            if (!Directory.Exists(folder))
                return new string[0];

            return Directory.GetDirectories(folder);
        }

        public virtual void DeleteFiles(User user, EncounterMetadata metadata)
        {
            var encounterFolder = GetEncounterFolder(user, metadata);
            Directory.Delete(encounterFolder, true);
        }

        protected virtual string EncountersFolder { get; set; } = "encounters";
        public virtual string GetEncountersFolder(User user)
            => Path.Combine(GetUserFolder(user), EncountersFolder);
        public virtual string GetEncounterFolder(User user, EncounterMetadata metadata)
            => Path.Combine(GetEncountersFolder(user), metadata.GetDesiredFilename());
        protected virtual string SaveFolder { get; set; } = "save";
        public virtual string GetSaveFolder(User user, EncounterMetadata metadata)
            => Path.Combine(GetEncounterFolder(user, metadata), SaveFolder);
        public virtual string GetSaveFolder(string encounterDirectory)
            => Path.Combine(encounterDirectory, SaveFolder);
        protected virtual string ImagesFolder { get; set; } = "images";
        public virtual string GetImagesFolder(User user, EncounterMetadata metadata)
            => Path.Combine(GetSaveFolder(user, metadata), ImagesFolder);


        ///<summary>
        ///Returns a truncated md5 hash to represent unique folders for users. This returns only the folder
        ///</summary>
        ///<param name="accountId">User account id</param>
        protected virtual string GetUserFolder(User user)
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


        public virtual WaitableTask<Texture2D> ReadTextureFile(User user, EncounterMetadata metadata, string filename)
        {
            var imagesFolder = GetImagesFolder(user, metadata);
            var path = Path.Combine(imagesFolder, filename);

            var fileBytes = File.ReadAllBytes(path);
            var texture = new Texture2D(2, 2);
            texture.LoadRawTextureData(fileBytes);

            return new WaitableTask<Texture2D>(texture);
        }

        public virtual void WriteTextureFile(User user, EncounterMetadata metadata, EncounterImage image)
        {
            byte[] textureBytes;
            if (image.Filename.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase))
                textureBytes = image.Sprite.texture.EncodeToPNG();
            else
                textureBytes = image.Sprite.texture.EncodeToJPG();


            var imagesFolder = GetImagesFolder(user, metadata);
            var path = Path.Combine(imagesFolder, image.Filename);
            File.WriteAllBytes(path, textureBytes);
        }
    }
}