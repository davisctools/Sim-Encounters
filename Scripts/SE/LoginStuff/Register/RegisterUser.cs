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

        protected virtual string PhpFile { get; } = "Login.php";
        protected virtual string ActionVariable { get; } = "ACTION";
        protected virtual string RegisterAction { get; } = "register";
        protected virtual string UsernameVariable { get; } = "username";
        protected virtual string PasswordVariable { get; } = "password";
        protected virtual string EmailVariable { get; } = "email";

        /**
         * Downloads all available and applicable menu files to display on the main manu.
         * Returns them as a MenuCase item
         */
        public void Register(string username, string password, string email)
        {
            var url = WebAddress.BuildUrl(PhpFile);
            var form = CreateForm(username, password, email);

            var webRequest = UnityWebRequest.Post(url, form);
            var serverResult = ServerReader.Begin(webRequest);
            serverResult.AddOnCompletedListener(ProcessResults);
        }

        public WWWForm CreateForm(string username, string password, string email)
        {
            var form = new WWWForm();

            form.AddField(ActionVariable, RegisterAction);

            form.AddField(UsernameVariable, username);
            form.AddField(PasswordVariable, password);
            form.AddField(EmailVariable, email);

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
