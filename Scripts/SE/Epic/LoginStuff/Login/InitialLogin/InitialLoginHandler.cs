using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class InitialLoginHandler : BaseInitialLoginHandler
    {
        public ManualLogin ManualLogin { get => manualLogin; set => manualLogin = value; }
        [SerializeField] private ManualLogin manualLogin;

        protected ILoginHandler AutoLogin { get; set; }
        protected StayLoggedIn StayLoggedIn { get; set; }
        [Inject]
        public virtual void Inject(StayLoggedIn stayLoggedIn, ILoginHandler autoLogin)
        {
            StayLoggedIn = stayLoggedIn;
            AutoLogin = autoLogin;
        }

        protected virtual WaitableTask<User> CurrentWaitableResult { get; set; }
        public override WaitableTask<User> InitialLogin(ILoadingScreen loadingScreen)
        {
            CurrentWaitableResult = new WaitableTask<User>();

            if (StayLoggedIn.Value)
                TryAutoLogin();
            else
                ShowManualLogin();

            return CurrentWaitableResult;
        }

        public override WaitableTask<User> Login()
        {
            StayLoggedIn.SetValue(false);

            CurrentWaitableResult = new WaitableTask<User>();

            ShowManualLogin();

            return CurrentWaitableResult;
        }

        protected virtual void TryAutoLogin()
        {
            var autoLoginResult = AutoLogin.Login();
            autoLoginResult.AddOnCompletedListener(ProcessAutoLoginResult);
        }
        protected virtual void ProcessAutoLoginResult(TaskResult<User> user)
        {
            if (user.IsError() || user.Value == null)
                ShowManualLogin();
            else
                CurrentWaitableResult.SetResult(user.Value);
        }

        protected virtual void ShowManualLogin()
        {
            gameObject.SetActive(true);
            var loginResult = ManualLogin.Login();
            loginResult.AddOnCompletedListener(ProcessManualLoginResult);
        }
        protected virtual void ProcessManualLoginResult(TaskResult<User> user) => CurrentWaitableResult.SetResult(user.Value);
    }
}