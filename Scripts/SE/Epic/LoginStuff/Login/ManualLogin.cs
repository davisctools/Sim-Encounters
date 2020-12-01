using ClinicalTools.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ManualLogin : BaseLoginHandler
    {
        public TMP_InputField UsernameField { get => usernameField; set => usernameField = value; }
        [SerializeField] private TMP_InputField usernameField;
        public TMP_InputField PasswordField { get => passwordField; set => passwordField = value; }
        [SerializeField] private TMP_InputField passwordField;
        public Toggle StayLoggedInToggle { get => stayLoggedInToggle; set => stayLoggedInToggle = value; }
        [SerializeField] private Toggle stayLoggedInToggle;
        public Button LoginButton { get => loginButton; set => loginButton = value; }
        [SerializeField] private Button loginButton;
        public Button GuestButton { get => guestButton; set => guestButton = value; }
        [SerializeField] private Button guestButton;

        protected IPasswordLoginHandler PasswordLoginHandler { get; private set; }
        protected StayLoggedIn StayLoggedIn { get; set; }
        protected BaseMessageHandler MessageHandler { get; set; }
        [Inject]
        public virtual void Inject(IPasswordLoginHandler passwordLoginHandler, StayLoggedIn stayLoggedIn, BaseMessageHandler messageHandler)
        {
            PasswordLoginHandler = passwordLoginHandler;
            StayLoggedIn = stayLoggedIn; 
            MessageHandler = messageHandler;
        }

        protected virtual void Awake()
        {
            LoginButton.onClick.AddListener(PasswordLogin);
            GuestButton.onClick.AddListener(GuestLogin);
        }

        protected virtual WaitableTask<User> CurrentWaitableResult { get; set; }
        public override WaitableTask<User> Login()
        {
            gameObject.SetActive(true);

            CurrentWaitableResult = new WaitableTask<User>();

            UsernameField.text = "";
            PasswordField.text = "";
            StayLoggedInToggle.isOn = false;

            return CurrentWaitableResult;
        }

        private void GuestLogin()
        {
            if (CurrentWaitableResult?.IsCompleted() == false)
                CurrentWaitableResult.SetResult(User.Guest);
            gameObject.SetActive(false);
        }

        private void PasswordLogin()
        {
            var username = UsernameField.text;
            var email = UsernameField.text;
            var password = PasswordField.text;
            var user = PasswordLoginHandler.Login(username, email, password);
            user.AddOnCompletedListener(ServerUserResponse);
        }

        protected void ServerUserResponse(TaskResult<User> result)
        {
            if (result.IsError()) {
                MessageHandler.ShowMessage($"Could not login: {result.Exception.Message}", MessageType.Error);
                return;
            }

            gameObject.SetActive(false);
            if (StayLoggedInToggle == null || StayLoggedInToggle.isOn)
                StayLoggedIn.SetValue(true);
            PlayerPrefs.Save();
            CurrentWaitableResult.SetResult(result.Value);
        }
    }
}