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

    }
}