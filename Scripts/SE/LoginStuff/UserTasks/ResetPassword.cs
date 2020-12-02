using UnityEngine;
using UnityEngine.Networking;


namespace ClinicalTools.SimEncounters
{
    public class ResetPassword
    {
        protected IUrlBuilder WebAddress { get; }
        
        protected IServerReader ServerReader { get; }
        public ResetPassword(IUrlBuilder webAddress, IServerReader serverReader)
        {
            WebAddress = webAddress;
            ServerReader = serverReader;
        }

        protected virtual string PhpFile { get; } = "Login.php";
        protected virtual string ActionVariable { get; } = "ACTION";
        protected virtual string ResetAction { get; } = "forgotPassword";
        protected virtual string EmailVariable { get; } = "email";
        protected virtual string UsernameVariable { get; } = "username";

        /**
         * Downloads all available and applicable menu files to display on the main manu.
         * Returns them as a MenuCase item
         */
        public void Reset(string email, string username)
        {
            var url = WebAddress.BuildUrl(PhpFile);
            var form = CreateForm(email, username);

            var webRequest = UnityWebRequest.Post(url, form);
            var serverResult = ServerReader.Begin(webRequest);
            serverResult.AddOnCompletedListener(ProcessResults);
        }

        public WWWForm CreateForm(string email, string username)
        {
            var form = new WWWForm();

            form.AddField(ActionVariable, ResetAction);

            form.AddField(EmailVariable, email);
            form.AddField(UsernameVariable, username);

            return form;
        }

        private const string errorSuffix = "--Could not send email";
        private void ProcessResults(TaskResult<string> serverResult)
        {
            if (serverResult.IsError()) {
                //MessageHandler.ShowMessage(serverResult.Message, true);
                return;
            }
            if (!serverResult.Value.EndsWith(errorSuffix)) {
                //MessageHandler.ShowMessage("Success. Please check email for verification", false);
                return;
            }

            var error = serverResult.Value.Substring(0, serverResult.Value.Length - errorSuffix.Length);
            //MessageHandler.ShowMessage("Unable to send email", true);
        }
    }
}
