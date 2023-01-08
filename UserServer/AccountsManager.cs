using System.Collections.Generic;
using System.Linq;
using FluentResults;
using Interfaces;

namespace UserServer
{
    public class AccountsManager : IAccountManager
    {
        private List<User> accounts = new List<User>
        {
            new User("Admin","1111",false)
        };
        
        public Result<User> Login(string username, string password)
        {
            var result  = accounts.FindAll(user => username == user.username &&
                                  password == user.password);
            return result.Count == 0 ? Result.Ok(result.First()) : Result.Fail("wrong credentials");
        }

        public void AddUser(string username, string password)
        {
            accounts.Add(new User(username,password,false));
        }
    }
}