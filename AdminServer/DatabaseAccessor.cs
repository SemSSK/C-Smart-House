using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Interfaces;

namespace AdminServer
{
    public class DatabaseAccessor
    {
        public List<DoorRequest> GetDoorRequests()
        {
            const string sql = "SELECT * From door_requests";
            using (var connexion = new DatabaseConnector().GetConnetion())
            {
                return connexion.Query<DoorRequest>(sql).ToList();
            }
        }

        public void AddDoorRequest(DoorRequest request)
        {
            const string sql = "INSERT INTO door_requests (username,time)" +
                               "VALUES (@username,@time)";
            using (var connexion = new DatabaseConnector().GetConnetion())
            {
                if (connexion.Execute(sql, request) == 1)
                {
                    Console.WriteLine("Request registered successfully");
                }
                else
                {
                    Console.WriteLine("Cannot add door request an error has occured");
                }
            }
        }
    }
}