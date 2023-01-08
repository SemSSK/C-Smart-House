namespace Interfaces
{
    public class User
    {
        public User(string username, string password, bool isAdmin)
        {
            this.username = username;
            this.password = password;
            this.isAdmin = isAdmin;
        }

        public string username {get; set; }
        public string password { get; set; }
        public bool isAdmin { get; set; } = false;
    }
}