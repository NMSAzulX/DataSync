using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var conn = new MySqlConnection(
                "Database='mysql';Data Source=my.com;password=1qazZAQ!;User ID=root;Port=3306;");
            var result = conn.Query("SELECT * FROM cnblogs.news").ToList();
            var dict = (IDictionary<string, object>) (result[0]);
            Console.Read();
        }
    }
}