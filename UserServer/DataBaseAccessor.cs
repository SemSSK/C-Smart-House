using System;
using System.Collections.Generic;
using Dapper;
using Interfaces;

namespace UserServer
{
    public class DataBaseAccessor
    {
        public List<User> GetUsers()
        {
            const string sql = @"SELECT * FROM users";
            using (var connexion = new DatabaseConnector().GetConnetion())
            {
                return connexion.Query<User>(sql).AsList();
            }
        }

        public void AddUser(string username,string password)
        {
            const string sql = "INSERT INTO users (username,password,isAdmin)" +
                               "VALUES (@username,@password,DEFAULT)";
            using (var connection = new DatabaseConnector().GetConnetion())
            {
                if (connection.Execute(sql,new User{username = username,password = password}) == 1)
                {
                    Console.WriteLine("User {0}: Inserted Successfully",username);
                }
                else
                {
                    Console.WriteLine("A problem has occured");
                }
            }
        }

        public void DeleteUser(int id)
        {
            const string sql = "DELETE FROM users Where id = @id";
            using (var connection = new DatabaseConnector().GetConnetion())
            {
                if (connection.Execute(sql, new User{Id = id}) == 1)
                {
                    Console.WriteLine("User deleted successfully");
                }
                else
                {
                    Console.WriteLine("Deletion Failed");
                }
            }
        }
    }
}