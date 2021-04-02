namespace SteamSwitcher
{
    public class Models
    {
        public class DatabaseSettings
        {
            public string ConnectionString { get; set; }
            public string DatabaseName { get; set; }

            public DatabaseSettings(string connectionString, string databaseName)
            {
                ConnectionString = connectionString;
                DatabaseName = databaseName;
            }
        }

        public class SteamUser
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Hint { get; set; }
        }
    }
}