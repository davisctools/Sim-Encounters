

namespace ClinicalTools.SimEncounters
{
    public class User
    {
        public static User Guest { get; } = new User();

        public bool IsGuest { get; }
        public int AccountId { get; }
        public string Email { get; set; }
        public string Username { get; set; }
        public Name Name { get; set; }
        public UserStatus Status { get; set; } = new UserStatus();

        // Creates guest user
        protected User()
        {
            IsGuest = true;
            AccountId = 0;
            Username = "Guest";
        }

        public User(int accountId) => AccountId = accountId;
    }
}