using FluentResults;

namespace Interfaces
{
    public interface IAccountManager
    {
        User Login(string username, string password);
        void AddUser(string username, string password);
    }
}