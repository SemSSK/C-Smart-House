using System;
using System.Collections.Generic;
using FluentResults;

namespace Interfaces
{
    public interface IAccountManager
    {
        User Login(string username, string password);
        void LogOut(string key);
        void AddUser(string username, string password);
        void UpdateUser(int id,string username,string password);
        List<User> GetUsers();
        void RemoveUser(int id);
        LoggedInStated CheckState(string key);
    }
}