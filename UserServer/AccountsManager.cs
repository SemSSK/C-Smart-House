using System;
using System.Collections.Generic;
using System.Linq;
using FluentResults;
using Interfaces;

namespace UserServer
{
    public class AccountsManager :MarshalByRefObject , IAccountManager
    {
        private static Dictionary<string, User> LoggedInUsers = new Dictionary<string, User>();
        public User Login(string username, string password)
        {
            var result  = GetUsers().FindAll(user => username.Equals(user.username) &&
                                  password.Equals(user.password));
            var u = result.Count != 0 ? result.First() : null;
            if (!(u is null))
            {
                LoggedInUsers.Add(u.username,u);
            }
            return u;
        }

        public void LogOut(string key)
        {
            if (CheckState(key) != LoggedInStated.LoggedOut)
            {
                LoggedInUsers.Remove(key);
            }
        }

        public void AddUser(string username, string password) => new DataBaseAccessor()
            .AddUser(username,password);

        public void UpdateUser(int id, string username, string password)
        {
            Console.WriteLine("Going to use the database");
            new DataBaseAccessor().UpdateUser(new User()
            {
                username = username,
                Id = id,
                password = password
            });
        }

        public List<User> GetUsers() => new DataBaseAccessor().GetUsers();

        public void RemoveUser(int id) => new DataBaseAccessor().DeleteUser(id);
        public LoggedInStated CheckState(string key)
        {
            if (!LoggedInUsers.ContainsKey(key))
            {
                return LoggedInStated.LoggedOut;
            }
            var user = LoggedInUsers[key];
            return user.isAdmin ? LoggedInStated.AsAdmin : LoggedInStated.AsVisitor;
        }
    }
}