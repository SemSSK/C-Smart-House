using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;

namespace AdminServer
{
    public class ConnexionService
    {
        private IAccountManager accountManager;

        public ConnexionService()
        {
            accountManager = (IAccountManager) Activator.GetObject(
                typeof(IAccountManager),
                "tcp://localhost:3000/accounts");
        }

        public User Login(string username, string password) =>
            accountManager.Login(username, password);

        public void Logout(string key) =>
            accountManager.LogOut(key);

        public bool CheckIfLoggedIn(string key) =>
            accountManager.CheckState(key) == LoggedInStated.AsAdmin;

        public User GetUser(int id){
            var users = GetUsers().FindAll(user => user.Id == id);
            return users.Count == 0 ? null : users.First();
        }

        public void DeleteUser(int id) =>
            accountManager.RemoveUser(id);

        public void AddUser(string username, string password) => accountManager.AddUser(username, password);
        public void UpdateUser(User user) => accountManager.UpdateUser(user.Id,user.username,user.password);
        public List<User> GetUsers() => accountManager.GetUsers();

    }
}