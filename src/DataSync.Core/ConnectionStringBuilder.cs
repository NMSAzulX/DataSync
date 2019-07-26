namespace DataSync.Core
{
    /// <summary>
    /// 连接属性
    /// </summary>
    public abstract class ConnectionStringBuilder
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }

        public abstract string BuildConnectionString();
    }
}