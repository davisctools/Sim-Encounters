using UnityEngine;
using UnityEngine.Networking;


namespace ClinicalTools.SimEncounters
{
    public class RegisterUser
    {
        protected IUrlBuilder WebAddress { get; }
        protected IServerReader ServerReader { get; }
        public RegisterUser(IUrlBuilder webAddress, IServerReader serverReader)
        {
            WebAddress = webAddress;
            ServerReader = serverReader;
        }

        private const string phpFile = "Login.php";
        private const string actionVariable = "ACTION";
        private const string registerAction = "register";
        private const string usernameVariable = "username";
        private const string passwordVariable = "password";
        private const string emailVariable = "email";

        /**
         * Downloads all available and applicable menu files to display on the main manu.
         * Returns them as a MenuCase item
         */
        public void Register(string username, string password, string email)
        {
            var url = WebAddress.BuildUrl(phpFile);
            var form = CreateForm(username, password, email);

            var webRequest = UnityWebRequest.Post(url, form);
            var serverResult = ServerReader.Begin(webRequest);
            serverResult.AddOnCompletedListener(ProcessResults);
        }

        public WWWForm CreateForm(string username, string password, string email)
        {
            var form = new WWWForm();

            form.AddField(actionVariable, registerAction);

            form.AddField(usernameVariable, username);
            form.AddField(passwordVariable, password);
            form.AddField(emailVariable, email);

            return form;
        }

        private void ProcessResults(TaskResult<string> serverResults)
        {
            if (serverResults.IsError()) {
                //MessageHandler.ShowMessage(serverResults.Message, true);
                return;
            }
            if (serverResults.Value.StartsWith("Connection Granted")) {
                //MessageHandler.ShowMessage("Success. Please check email (or spam folder) for verification", false);
                return;
            }

            var error = serverResults.Value.Split(new string[] { "--" }, System.StringSplitOptions.None)[0];
            //MessageHandler.ShowMessage(error, true);
        }
    }
}
