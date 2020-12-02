using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class ResendEmail
    {
        protected IUrlBuilder WebAddress { get; }
        protected IServerReader ServerReader { get; }
        public ResendEmail(IUrlBuilder webAddress, IServerReader serverReader)
        {
            WebAddress = webAddress;
            ServerReader = serverReader;
        }

        protected virtual string PhpFile { get; } = "Login.php";
        protected virtual string ActionVariable {get;}= "ACTION";
        protected virtual string ResendAction { get; } = "resendActivation";
        protected virtual string EmailVariable { get; } = "email";

        /**
         * Downloads all available and applicable menu files to display on the main manu.
         * Returns them as a MenuCase item
         */
        public void Resend(string email)
        {
            var url = WebAddress.BuildUrl(PhpFile);
            var form = CreateForm(email);

            var webRequest = UnityWebRequest.Post(url, form);
            var serverResult = ServerReader.Begin(webRequest);
            serverResult.AddOnCompletedListener(ProcessResults);
        }

        public WWWForm CreateForm(string email)
        {
            var form = new WWWForm();

            form.AddField(ActionVariable, ResendAction);

            form.AddField(EmailVariable, email);

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
                //MessageHandler.ShowMessage(serverResult.Message, false);
                return;
            }

            var error = serverResult.Value.Substring(0, serverResult.Value.Length - errorSuffix.Length);
            //MessageHandler.ShowMessage(error, true);
        }
    }
}
