using System;
using FluentResults;
using Interfaces;

namespace Visitor
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

        public bool CheckIfLoggedIn(string key) =>
            accountManager.CheckState(key) == LoggedInStated.AsVisitor;
        public void Logout(string key) =>
            accountManager.LogOut(key);

    }
}