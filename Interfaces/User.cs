using System;

namespace Interfaces
{
    public class User : MarshalByRefObject
    {
        public User(){}
        public User(string username, string password, bool isAdmin)
        {
            Id = 0;
            this.username = username;
            this.password = password;
            this.isAdmin = isAdmin;
        }

        
        public int Id { get; set; }
        public string username {get; set; }
        public string password { get; set; }
        public bool isAdmin { get; set; } = false;
    }
}