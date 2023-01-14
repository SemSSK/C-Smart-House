using System;
using System.Collections.Generic;
using System.Linq;
using FluentResults;
using Interfaces;

namespace UserServer
{
    public class AccountsManager :MarshalByRefObject , IAccountManager
    {
        private List<User> accounts = new List<User>
        {
            new User("Admin","1111",true)
        };
        
        public User Login(string username, string password)
        {
            var result  = GetUsers().FindAll(user => username.Equals(user.username) &&
                                  password.Equals(user.password));
            return result.Count != 0 ? result.First() : null;
        }

        public void AddUser(string username, string password) => new DataBaseAccessor()
            .AddUser(username,password);

        public List<User> GetUsers() => new DataBaseAccessor().GetUsers();

        public void RemoveUser(int id) => new DataBaseAccessor().DeleteUser(id);
    }
}