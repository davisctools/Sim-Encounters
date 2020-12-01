using System;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class UserDeserializer : IStringDeserializer<User>
    {
        private const int UserParts = 6;
        private const int AccountIdIndex = 2;
        private const int UsernameIndex = 3;
        private const int EmailIndex = 4;
        private const int FirstNameIndex = 6;
        private const int LastNameIndex = 7;
        private const int HonorificIndex = 8;
        public User Deserialize(string userText)
        {
            var splitChars = new string[] { "--" };
            var userParts = userText.Split(splitChars, StringSplitOptions.None);
            if (userParts.Length < UserParts)
                return null;

            if (!int.TryParse(userParts[AccountIdIndex], out int accountId))
                return null;

            return new User(accountId) {
                Username = UnityWebRequest.UnEscapeURL(userParts[UsernameIndex]),
                Email = UnityWebRequest.UnEscapeURL(userParts[EmailIndex]),
                Name = new Name(UnityWebRequest.UnEscapeURL(userParts[HonorificIndex]),
                                UnityWebRequest.UnEscapeURL(userParts[FirstNameIndex]), 
                                UnityWebRequest.UnEscapeURL(userParts[LastNameIndex]))
            };
        }

    }
}