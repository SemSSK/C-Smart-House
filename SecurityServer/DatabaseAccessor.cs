using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Interfaces;

namespace SecurityServer
{
    public class DatabaseAccessor
    {
        public List<string> GetMoCaps()
        {
            const string sql = "SELECT time FROM mocaps";
            using (var connexion = new DatabaseConnector().GetConnetion())
            {
                return connexion.Query<string>(sql).ToList();
            }
        }

        public void AddMoCap(string time)
        {
            const string sql = "INSERT INTO mocaps (time) " +
                               "VALUES (@time)";
            using (var connexion = new DatabaseConnector().GetConnetion())
            {
                if (connexion.Execute(sql, new { time = time }) == 1)
                {
                    Console.WriteLine("Movement Capture inserted");
                }
                else
                {
                    Console.WriteLine("Cannot insert Movement capture an error has occured");
                }
            }
        }
    }
}