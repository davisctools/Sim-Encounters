using System;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class AutoLogin : ILoginHandler
    {
        protected IServerReader ServerReader { get; }
        protected IUrlBuilder WebAddress { get; }
        protected UserDeserializer UserDeserializer { get; }

        public AutoLogin(IServerReader serverReader, IUrlBuilder webAddress, UserDeserializer userDeserializer)
        {
            ServerReader = serverReader;
            WebAddress = webAddress;
            UserDeserializer = userDeserializer;
        }

        public virtual WaitableTask<User> Login()
        {
            var webRequest = GetWebRequest();
            var serverResult = ServerReader.Begin(webRequest);

            var user = new WaitableTask<User>();
            serverResult.AddOnCompletedListener((result) => ProcessResults(user, result));

            return user;
        }

        protected virtual UnityWebRequest GetWebRequest()
        {
            var address = WebAddress.BuildUrl("Login.php");
            WWWForm form = new WWWForm();
            form.AddField("ACTION", "checkSession");
            form.AddField("deviceid", SystemInfo.deviceUniqueIdentifier);
            return UnityWebRequest.Post(address, form);
        }

        private void ProcessResults(WaitableTask<User> result, TaskResult<string> serverResult)
        {
            if (serverResult.IsError() || string.IsNullOrWhiteSpace(serverResult.Value)) {
                result.SetError(serverResult.Exception);
                return;
            }

            var user = UserDeserializer.Deserialize(serverResult.Value);

            if (user == null)
                result.SetError(new Exception($"Could not parse user: {serverResult.Value}"));
            else
                result.SetResult(user);
        }
    }
}