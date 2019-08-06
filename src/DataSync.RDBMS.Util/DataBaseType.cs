using System.Xml.Linq;

namespace DataSync.RDBMS.Util
{
    public class DataBaseType
    {
        public DataBaseType(string typeName, string driveClassTypeName)
        {
            TypeName = typeName;
            DriveClassTypeName = driveClassTypeName;
        }

        public string TypeName { get; }

        public string DriveClassTypeName { get; }

        public static DataBaseType MySql =
            new DataBaseType("mysql", "MySql.Data.MySqlClient.MySqlConnection, MySqlConnector");
    }
}