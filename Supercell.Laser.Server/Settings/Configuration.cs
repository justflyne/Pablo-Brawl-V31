namespace Supercell.Laser.Server.Settings
{
    using System.IO;
    using Newtonsoft.Json;

    public class Configuration
    {
        public static Configuration Instance;

        [JsonProperty("port")] public readonly int Port;

        [JsonProperty("mysql_host")] public readonly string MysqlHost;
        [JsonProperty("mysql_port")] public readonly int MysqlPort;
        [JsonProperty("mysql_username")] public readonly string MysqlUsername;
        [JsonProperty("mysql_password")] public readonly string MysqlPassword;
        [JsonProperty("mysql_database")] public readonly string MysqlDatabase;
        [JsonProperty("anti-ddos")] public readonly bool antiddos;
        [JsonProperty("BotToken")] public readonly string BotToken;
        [JsonProperty("ChannelId")] public readonly ulong ChannelId;        

        public static Configuration LoadFromFile(string filename)
        {
            return JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(filename));
        }
    }
}