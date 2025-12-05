namespace Supercell.Laser.Server.Discord.Commands
{
    using System.Net;
    using NetCord.Services.Commands;
    using Supercell.Laser.Server.Settings;
    public class IPBan : CommandModule<CommandContext>
    {
        [Command("banip")]
        public static string BanIpCommand([CommandParameter(Remainder = true)] string ipAddress)
        {
            if (!Configuration.Instance.antiddos)
            {
                return "Anti-DDoS is disabled. Enable it in config.json to use this command.";
            }

            if (!IPAddress.TryParse(ipAddress, out _))
            {
                return "Invalid IP address format.";
            }

            if (IsIpBanned(ipAddress))
            {
                return $"IP {ipAddress} уже заблокирован.";
            }

            try
            {
                File.AppendAllText("ipblacklist.txt", ipAddress + Environment.NewLine);
                return $"IP {ipAddress} был заблокирован.";
            }
            catch (Exception ex)
            {
                return $"Failed to ban IP address: {ex.Message}";
            }
        }
        private static bool IsIpBanned(string ipAddress)
        {
            if (!File.Exists("ipblacklist.txt"))
            {
                return false;
            }

            string[] bannedIps = File.ReadAllLines("ipblacklist.txt");
            return bannedIps.Contains(ipAddress);
        }
    }
}