namespace ClinicalTools.SimEncounters
{
    public class LoginHandler : ILoginHandler
    {
        protected ILoginHandler AutoLogin { get; }
        protected ILoginHandler ManualLogin { get; }

        public LoginHandler(ILoginHandler autoLogin, ILoginHandler manualLogin)
        {
            AutoLogin = autoLogin;
            ManualLogin = manualLogin;
        }

        public WaitableTask<User> Login()
        {
            var user = new WaitableTask<User>();
            var autoLoginUser = AutoLogin.Login();
            autoLoginUser.AddOnCompletedListener((result) => AutoLoginCompleted(user, result));

            return user;
        }

        private void AutoLoginCompleted(WaitableTask<User> result, TaskResult<User> autoLoginUser)
        {
            if (!autoLoginUser.IsError()) {
                result.SetResult(autoLoginUser.Value);
                return;
            }

            var manualLoginUser = ManualLogin.Login();
            manualLoginUser.AddOnCompletedListener((manualLoginResult) => ManualLoginCompleted(result, manualLoginResult));
        }

        private void ManualLoginCompleted(WaitableTask<User> result, TaskResult<User> manualLoginUser)
        {
            if (manualLoginUser.IsError())
                result.SetError(manualLoginUser.Exception);
            else
                result.SetResult(manualLoginUser.Value);
        }
    }
}