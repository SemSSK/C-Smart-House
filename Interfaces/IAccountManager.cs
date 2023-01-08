using FluentResults;

namespace Interfaces
{
    public interface IAccountManager
    {
        Result<User> Login(string username, string password);
        void AddUser(string username, string password);
    }
}