using System;
using DataSync.Core;

namespace DataSync.MySqlReader
{
    public class MySqlConnectionStringBuilder : ConnectionStringBuilder
    {
        public override string BuildConnectionString()
        {
            var database = string.IsNullOrWhiteSpace(Database) ? "mysql" : Database;
            return $"Database='{database}';Data Source={Host};password={Password};User ID={User};Port={Port};";
        }
    }
}