namespace Supercell.Laser.Server.Discord.Commands
{
    using System.Diagnostics;
    using System.Net;
    using NetCord.Services.Commands;

    public class UnBanIP : CommandModule<CommandContext>
    {
        [Command("unbanip")]
        public static string UnbanIpCommand([CommandParameter(Remainder = true)] string ipAddress)
        {
            if (!IPAddress.TryParse(ipAddress, out _))
            {
                return "Невалидный формат IP-Адреса.";
            }

            if (!IsIpBanned(ipAddress))
            {
                return $"IP-Адрес {ipAddress} не был заблокирован.";
            }

            try
            {
                // Убираем блокировку через iptables
                ExecuteUnbanIp(ipAddress);

                // Удаляем IP из локального файла
                RemoveIpFromBlacklist(ipAddress);

                return $"IP-Адрес {ipAddress} был разблокирован.";
            }
            catch (Exception ex)
            {
                return $"Failed to unban IP address: {ex.Message}";
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

        private static void ExecuteUnbanIp(string ip)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"iptables -D INPUT -s {ip} -j DROP\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            Process.Start(psi);
        }

        private static void RemoveIpFromBlacklist(string ipAddress)
        {
            if (!File.Exists("ipblacklist.txt"))
            {
                return;
            }

            string[] bannedIps = File.ReadAllLines("ipblacklist.txt");
            File.WriteAllLines("ipblacklist.txt", bannedIps.Where(ip => ip != ipAddress));
        }
    }
}